using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Projectile : MonoBehaviour {

	public LayerMask collisionMask;
	float speed = 10;
    float damage = 1;
	
	float lifeTime = 1.5f;
	float skinWidth = .1f; // 총알과 AI에 정확한 거리에서 인식하게 하기 위한 변수

	private void Start() {
		Destroy(gameObject, lifeTime);
		
		Collider[] initialCollisions = Physics.OverlapSphere(transform.position, .1f, collisionMask);
		if(initialCollisions.Length > 0){
			OnHitObject(initialCollisions[0],transform.position);
			// 오브젝트와 충돌시 총알 제거
		}
	}

	public void SetSpeed(float newSpeed) {
		speed = newSpeed;
		
	}
	
	void Update () {
		float moveDistance = speed * Time.deltaTime;
		CheckCollisions (moveDistance);
		transform.Translate (Vector3.forward * moveDistance);
	}


	void CheckCollisions(float moveDistance) {
		Ray ray = new Ray (transform.position, transform.forward);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, moveDistance + skinWidth, collisionMask, QueryTriggerInteraction.Collide)) {
			OnHitObject(hit.collider, hit.point);
		}
	}

	void OnHitObject(Collider c, Vector3 hitPoint) {
		IDamageable damageableObject = c.GetComponent<IDamageable> ();
		if (damageableObject != null) {
			damageableObject.TakeHit(damage, hitPoint, transform.forward);
		}
		GameObject.Destroy (gameObject);
	}
}