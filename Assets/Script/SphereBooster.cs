using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereBooster : MonoBehaviour
{

	// 飛行中フラグ
	bool isFlying = false;

	// ボタン押下フラグ
	bool isBoostPressed = false;

	// Sphereオブジェクトの初期位置格納用ベクトル
	Vector3 initPosition = Vector3.zero;

	// Rigidbodyコンポーネントへの参照をキャッシュ
	Rigidbody rb;

	// 力を加える方向
	Vector3 forceDirection = new Vector3(1.0f, 1.0f, 0f);

	// 加える力の大きさ
	float forceMagnitude = 10.0f;

	void Start()
	{
		initPosition = gameObject.transform.position;
		rb = gameObject.GetComponent<Rigidbody>();
	}

	void Update()
	{
		// Input.GetKeyUpはキーが一度押された後、それが離された時にTrueを返す
		if (Input.GetKeyUp(KeyCode.B))
		{
			isBoostPressed = true;
		}
	}

	void FixedUpdate()
	{
		if (!isBoostPressed)
		{
			// キーまたはボタンが押されていなければ
			// 処理の切り替えをせず抜ける
			return;
		}
		if (isFlying)
		{
			// 飛行中の処理
			StopFlying();
		}
		else
		{
			// ボールを飛ばす処理
			BoostSphere();
		}
		// 飛行中フラグの切り替え
		isFlying = !isFlying;

		// どちらの処理をしてもボタン押下フラグをfalseに
		isBoostPressed = false;
	}

	void StopFlying()
	{
		// 運動の停止
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;

		// 初期位置に移動させる
		gameObject.transform.position = initPosition;
	}

	void BoostSphere()
	{
		// 向きと力の計算
		Vector3 force = forceMagnitude * forceDirection;

		// 力を加えるメソッド
		rb.AddForce(force, ForceMode.Impulse);
	}

	public void OnPressedBoostButton()
	{
		isBoostPressed = true;
	}
}