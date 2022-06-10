using UnityEngine;
using System.Collections;
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
public class Enemy : LivingEntity {

	public enum State {Idle, Chasing, Attacking}; // 대기 / 추격 / 공격
	State currentState; // 상태를 통해 거리계산을 진행을 하는지 안하는지 구분

	NavMeshAgent pathfinder;
	Transform target;
	LivingEntity targetEntity;
	Material skinMaterial;

	Color originalColor;

	float attackDistanceThreshold = .5f; // 공격거리 제한
	float timeBetweenAttacks = 1;
	float damage = 1;

	float nextAttackTime;
	float myCollisionRadius;		// 충돌 반경 밖까지만 추적 할 수 있게 각각의 반지름을 구할 변수.
	float targetCollisionRadius;

	bool hasTarget;

	protected override void Start () {
        base.Start();
		pathfinder = GetComponent<NavMeshAgent> ();
		skinMaterial = GetComponent<Renderer>().material;
		originalColor = skinMaterial.color;

		if(GameObject.FindGameObjectWithTag ("Player") != null){
			currentState = State.Chasing;
			hasTarget = true;

			target = GameObject.FindGameObjectWithTag ("Player").transform;
			targetEntity = target.GetComponent<LivingEntity>();
			targetEntity.OnDeath += OnTargetDeath;

			myCollisionRadius = GetComponent<CapsuleCollider>().radius;
			targetCollisionRadius = GetComponent<CapsuleCollider>().radius;
			
			StartCoroutine (UpdatePath ());
		}
	}

	void Update () {
		if(hasTarget){
			if(Time.time > nextAttackTime){
					float sqlDstToTarget = (target.position - transform.position).sqrMagnitude; // 목표의 위치와 자신의 위치의 차에 제곱'
					if(sqlDstToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2)){
					nextAttackTime = Time.time + timeBetweenAttacks;	
					StartCoroutine(Attack());
				}
			}
		}
	}


	void OnTargetDeath(){
		hasTarget = false;
		currentState = State.Idle;
	}

	IEnumerator Attack(){
		currentState= State.Attacking;

		pathfinder.enabled =false;	// 공격중에 거리 계산을 막기 위해 선언
		Vector3 originalPosition = transform.position;	
		Vector3 dirToTarget = (target.position - transform.position).normalized; 
		Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius);		
		//originalPosition 에서 시작해서 attackPosition 으로 이동하고 다시 originalPosition으로 돌가기 떄문에,
		//값이 0에서 1로, 그리고 다시 1에서 0으로 돌아가야만 해서 대칭함수를 이용 (y = 4(-x^2 + x))
			
		float percent = 0;
		float attackSpeed =3;

		skinMaterial.color = Color.red;
		bool hasAppliedDamage = false;


		while(percent <=1){

			if(percent >= .5f && !hasAppliedDamage){
				hasAppliedDamage = true;
				targetEntity.TakeDamage(damage);
			}
			percent += Time.deltaTime * attackSpeed;
			float interpolation = (-Mathf.Pow(percent,2)+percent)*4; // 대칭함수(보간 값) 
			//보간 값이란 알려진 점들의 위치를 참조하여, 집합의 일정 범위의 점들(선)을 새롭게 그리는 방법
			//여기서는 원지점-> 공격지점으로 이동할때 참조할 대칭곡선을 만드는 참조점 의미.
			transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);

			yield return null;
		}

		skinMaterial.color = originalColor;
		currentState = State.Chasing;
		pathfinder.enabled = true; // 공격이 끝난 후 다시 거리 계산
	}

	IEnumerator UpdatePath() {
		float refreshRate = .25f;

		while (target != null) {
				if(currentState == State.Chasing){
				Vector3 dirToTarget = (target.position - transform.position).normalized; // 적이 공격후 플에이어와 겹침을 방지 위해 반지름 값을 구해 계산한 것
				Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius)/2;
				// 적AI 가 파괴된 후 길을 찾아갈려고 하는 오류 발생 여부를 막기 위해 조건부 투입
				if(!death){
				pathfinder.SetDestination (targetPosition);
				}
			}
			yield return new WaitForSeconds(refreshRate);
		}
	}
}