#### LiveToNextLevel
##### 1.随机连通地图:
洗牌算法(Fisher Yates &amp; Knuth - Pursten Feld),地图与障碍物的随机生成,涉及FIFO,Lerp
1.洗牌原理,遍历目标列表,每次遍历随机一个索引元素与当前遍历索引交换位置
2.获取不重复随机对象,乱序列表转队列,队头出队再入队,保证取随机值的同时不会取到重复值,也保证了列表的完整性
##### 2.武器系统
##### 3.Player和Enemy类
##### 4.Bullet类,监听事件与伤害机制
##### 5.部分接口说明
###### <1>Physics.OverlapSphere方法
```csharp
Physics.OverlapSphere(transform.position, 0.1f, collisionMask);
//说明:返回以参数1为原点和参数2为半径的球体内'满足条件'的碰撞体集合,该球体为3D相交球
//形参:position:3D相交球的球心
//radius:3D相交球的球半径
//layerMask 在某个Layer层上进行碰撞体检索,例如当前选中Player层,则只会返回周围半径内 Layer标示为Player的GameObject的碰撞体集合
//返回值:Collider[];//排好序的数据,索引越大说明目标碰撞体距离监测点越远
```    
##### 机制:
###### <1>类挂机检测:玩家长时间在某一处静止不动,或者攻击时持续逗留在某地超过一定时间被检测到,在玩家附近生成敌人,增加紧张感
