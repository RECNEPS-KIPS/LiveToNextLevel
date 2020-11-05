using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// //Damage接口
/// </summary>
public interface IDamageable {
    void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDir);//受击
    void TakeDamage(float damage);//受击
}
