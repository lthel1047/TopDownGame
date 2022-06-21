using UnityEngine;
using System.Collections;

[RequireComponent (typeof (PlayerController))]
[RequireComponent (typeof (GunController))]

// LivingEntity를 상속 받는 이유는 이미 해당스크립트가 IDamaneable / MonoBehaviour를 하속 중이 떄문에
// 위 같이 진행시 Start 메소드가 중복되어 실행되지 않을 수 있음 (override를 선언 후 base.Start()를 실행해 부모 메소드 호출 )
public class Player_ : LivingEntity {

	public float moveSpeed = 10;

	public Crosshairs crossHairs; // 조준점


	Camera viewCamera;
	PlayerController controller;
	GunController gunController;
	
	protected override void Start () {
		base.Start();
		controller = GetComponent<PlayerController> ();
		gunController = GetComponent<GunController> ();
		viewCamera = Camera.main;
	}

	void Update () {
		// 플레이어 움직임
		Vector3 moveInput = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));
		Vector3 moveVelocity = moveInput.normalized * moveSpeed;
		controller.Move (moveVelocity);

		// 플레이어 시점
		Ray ray = viewCamera.ScreenPointToRay (Input.mousePosition);
		Plane groundPlane = new Plane (Vector3.up, Vector3.zero * gunController.GunHeight);
		float rayDistance;

		if (groundPlane.Raycast(ray,out rayDistance)) {
			Vector3 point = ray.GetPoint(rayDistance);
			//Debug.DrawLine(ray.origin,point,Color.red);
			controller.LookAt(point);
			crossHairs.transform.position = point;
			crossHairs.DetectTargets(ray);
		}

		// 플레이어 공격
		if (Input.GetMouseButton(0)) {
			gunController.Shoot();
		}
	}
}