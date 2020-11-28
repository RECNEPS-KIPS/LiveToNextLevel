using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 生命体基类,持有生命特征的对象
/// </summary>
public class LivingEntity : MonoBehaviour, IDamageable {
    protected float HP;
    protected bool dead;//是否死亡
    public float startHP = 5;
    public event System.Action OnDeath;//生命体死亡事件
    protected virtual void Awake() {
        dead = false;
    }
    protected virtual void Start() {
        HP = startHP;
    }
    /// <summary>
    /// 生命体受击
    /// </summary>
    /// <param name="damage">伤害数值</param>
    public void TakeDamage(float damage) {
        //print("hit1");
        //print(HP + "_" + dead);
        HP -= damage;
        if (HP <= 0 && !dead) {
            Die();
        }
    }

    /// <summary>
    /// 扩展的生命体受击函数
    /// </summary>
    /// <param name="damage">伤害值</param>
    /// <param name="hitPoint">击中点</param>
    /// <param name="hitDir">击中方向</param>
    public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDir) {
        //print("hit2");
        TakeDamage(damage);
    }

    /// <summary>
    /// 处理死亡
    /// </summary>
    [ContextMenu("DestroySelf")]
    public virtual void Die() {
        //print("die");
        dead = true;
        if (OnDeath != null) {
            OnDeath();
        }
        GameObject.Destroy(gameObject);
    }
}
