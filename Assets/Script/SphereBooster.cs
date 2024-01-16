using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereBooster : MonoBehaviour
{

	// 加える力の大きさ
	[SerializeField]
	float forceMagnitude = 10.0f;

	// X軸からの角度
	[SerializeField, Range(0f, 90f)]
	float forceAngle = 45.0f;

	// 力を加える方向
	Vector3 forceDirection = new Vector3(1.0f, 1.0f, 0f);

	// 飛行中フラグ
	bool isFlying = false;//false:飛んでない

	// ボタン押下フラグ
	bool isBoostPressed = false;

	// 距離測定中フラグ
	bool isCheckingDistance = false;

	// Sphereオブジェクトの停止位置格納用ベクトル
	Vector3 stopPosition = Vector3.zero;

	// Sphereオブジェクトの初期位置格納用ベクトル
	Vector3 initPosition = Vector3.zero;

	// Rigidbodyコンポーネントへの参照をキャッシュ
	Rigidbody rb;

	public bool isFly()
    {
		return isFlying;

	}

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

		// forceAngleの変更を反映する
		CalcForceDirection();
	}

	void FixedUpdate()
	{
		Debug.Log("transform x : " + gameObject.transform.position.x.ToString());
		Debug.Log("IsSleeping : " + rb.IsSleeping());

		// 距離の測定
		CheckDistance();

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

		// 距離測定中フラグをFalseにセット
		isCheckingDistance = false;
	}

	void BoostSphere()
	{
		// 向きと力の計算
		Vector3 force = forceMagnitude * forceDirection;

		// 力を加えるメソッド
		rb.AddForce(force, ForceMode.Impulse);

		// 距離測定中フラグをTrueにセット
		isCheckingDistance = true;
	}

	public void OnPressedBoostButton()
	{
		isBoostPressed = true;
	}

	void CalcForceDirection()
	{
		// 入力された角度をラジアンに変換
		float rad = forceAngle * Mathf.Deg2Rad;

		// それぞれの軸の成分を計算
		float x = Mathf.Cos(rad);
		float y = Mathf.Sin(rad);
		float z = 0f;

		// Vector3型に格納
		forceDirection = new Vector3(x, y, z);
	}

	void CheckDistance()
	{
		if (!isCheckingDistance)
		{
			// 距離測定中でなければ何もしない
			return;
		}
		if (rb.IsSleeping())
		{
			// スリープモードに入ったことを検知したら距離を出力
			stopPosition = gameObject.transform.position;
			float distance = GetDistanceInXZ(initPosition, stopPosition);

			// コンソールに表示
			Debug.Log("飛距離は " + distance.ToString("F2") + " メートルです。");

			// 距離測定中フラグをオフに
			isCheckingDistance = false;
		}
	}

	float GetDistanceInXZ(Vector3 startPos, Vector3 stopPos)
	{
		// 開始位置、停止位置のそれぞれで、Y軸成分を除いて計算用Vector3を作成
		Vector3 startPosCalc = new Vector3(startPos.x, 0f, startPos.z);
		Vector3 stopPosCalc = new Vector3(stopPos.x, 0f, stopPos.z);

		// 2つのVector3データから距離を算出
		float distance = Vector3.Distance(startPosCalc, stopPosCalc);
		return distance;
	}
}