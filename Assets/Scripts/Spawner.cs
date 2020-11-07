//敌人AI制造机
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    /// <summary>
    /// 开发者模式参数
    /// </summary>
    public bool DEVELOP_MODE;

    public Wave[] waves;
    private Wave curWave;//当前波
    private int curWaveNum;//当前波数的数量
    public Enemy enemy;
    public int remainEnemiesToSpawn;//剩余需要创建的敌人
    private float nextSpawnTime;
    MapGenerator map;
    LivingEntity player;
    Transform playerTrs;//玩家位置
    float timeBetweenCheck = 2;//检测玩家是否类挂机的时间间隔
    float campMoveDistance = 1.5f;//避免类挂机检测至少需要移动的距离
    float nextCheckTime;//下一次检测时间
    Vector3 lastCampPos;//上一次玩家长时间停留的位置
    bool isCamp;
    bool isDisable;
    public event System.Action<int> OnNewWave;

    private int aliveEnemies;//剩余存活的敌人
    //public event System.Action<int> OnNewWave;
    void Start() {
        player = FindObjectOfType<Player>();//获取玩家
        player.OnDeath += OnPlayerDeath;//订阅玩家死亡事件
        playerTrs = player.transform;

        nextCheckTime = timeBetweenCheck + Time.time;
        lastCampPos = playerTrs.position;

        map = FindObjectOfType<MapGenerator>();//获取地图
        enemy = Resources.Load<GameObject>("Prefabs/Enemy").GetComponent<Enemy>();//获取敌人预制体
        NextWave();
    }

    void Update() {
        if (!isDisable) {
            //到达玩家静止检测时间点
            if (Time.time > nextCheckTime) {
                nextCheckTime = Time.time + timeBetweenCheck;
                //若玩家距离上次静止的位置小于检测距离,即玩家移动的距离在一定时间段过于小
                isCamp = Vector3.Distance(playerTrs.position, lastCampPos) < campMoveDistance;
                lastCampPos = playerTrs.position;
            }
            //剩余需要生成的敌人数大于0或者为无尽模式,当前时间满足生成时间
            if ((remainEnemiesToSpawn > 0 || curWave.infinate == true) && Time.time > nextSpawnTime) {
                remainEnemiesToSpawn--;
                nextSpawnTime = Time.time + curWave.timeBetweenSpawns;//为下一次生成时间赋值
                StartCoroutine("SpawnerEnemy");//生成敌人
            }
        }
        //开发者模式下按回车直接跳过这一波
        if (DEVELOP_MODE) {
            if (Input.GetKeyDown(KeyCode.Return)) {
                StopCoroutine("SpawnerEnemy");//停止生成敌人协程
                //销毁当前波的敌人
                foreach (var enemy in FindObjectsOfType<Enemy>()) {
                    Destroy(enemy.gameObject);
                }
                NextWave();
            }
        }
    }

    /// <summary>
    /// 生成敌人协程
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnerEnemy() {
        float spawnDelay = 1;//延迟
        float tileFlashSpeed = 4;//闪烁速度

        //随机一个贴片位置
        Transform randomTile = map.GetRandomOpenTile();
        //玩家类挂机行为存在,就在玩家附近生成敌人,迫使玩家移动起来
        if (isCamp) {
            randomTile = map.GetTileFromPosition(playerTrs.position);
        }
        Material tileMat = randomTile.GetComponent<Renderer>().material;
        Color oriColor = tileMat.color;
        Color flashColor = Color.red;
        float spawnTimer = 0;//缓动颜色
        while (spawnTimer < spawnDelay) {
            tileMat.color = Color.Lerp(oriColor, flashColor, Mathf.PingPong(spawnTimer * tileFlashSpeed, 1));
            spawnTimer += Time.deltaTime;
            yield return null;
        }

        Enemy spawnEnemy = GameObject.Instantiate(enemy, randomTile.position + Vector3.up, Quaternion.identity) as Enemy;
        spawnEnemy.OnDeath += OnEnemyDeath;//订阅事件
        spawnEnemy.SetCharacter(curWave.moveSpeed, curWave.hitsToKillPlayer, curWave.enemyHP, curWave.skinColor);
    }


    /// <summary>
    /// 处理敌人死亡的后续事务,若当前波敌人全部死亡,开启下一波
    /// </summary>
    void OnEnemyDeath() {
        aliveEnemies--;
        //print("enemy death");
        //存活敌人数为0,开启下一波
        if (aliveEnemies == 0) {
            NextWave();
        }
    }

    void OnPlayerDeath() {
        isDisable = true;
    }

    /// <summary>
    /// 开始制造下一波敌人
    /// </summary>
    void NextWave() {
        curWaveNum++;
        //print("wave num:" + curWaveNum);
        if (curWaveNum - 1 < waves.Length) {
            curWave = waves[curWaveNum - 1];

            remainEnemiesToSpawn = curWave.enemyCount;
            aliveEnemies = remainEnemiesToSpawn;

            //开始下一波,生成新地图
            if (OnNewWave != null) {
                OnNewWave(curWaveNum);
            }
            ResetPlayerPos();//重置玩家位置
        }
    }

    /// <summary>
    /// 重置玩家位置,将玩家放置到地图中心,防止生成的新地图过小,玩家位于地图边界外
    /// </summary>
    public void ResetPlayerPos() {
        playerTrs.position = map.GetTileFromPosition(Vector3.zero).position + new Vector3(0, 0.1f, 0);
    }

    [System.Serializable]
    //波数对象
    public class Wave {
        public bool infinate;//是否无限
        public int enemyCount;//一波包含敌人数
        public float timeBetweenSpawns;//两波生成间隔时间
        public float moveSpeed;//移速
        public int hitsToKillPlayer;
        public float enemyHP;
        public Color skinColor;

    }
}
