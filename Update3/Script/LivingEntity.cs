using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어와 몬스터 체력 통합관리 
public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHelath;
    protected float health;
    protected bool death;

    public event System.Action OnDeath;

    protected virtual void Start(){
        // 자식이 (override 를 이용해 호출 할 수 있게 virtual 추가)
        health = startingHelath;
    }

    public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection){
        //hit 변수 처리용
        TakeDamage(damage);
    }
    public virtual void TakeDamage(float damage){
        health -= damage;
        if(health <= 0 && !death){
            Die();
        }
    }
    protected void Die(){
        death=true;
        if(OnDeath != null){
            OnDeath();
        }
        GameObject.Destroy(gameObject);
    }
}
