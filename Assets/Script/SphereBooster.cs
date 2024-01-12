using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereBooster : MonoBehaviour
{

	// ������͂̑傫��
	[SerializeField]
	float forceMagnitude = 10.0f;

	// X������̊p�x
	[SerializeField, Range(0f, 90f)]
	float forceAngle = 45.0f;

	// �͂����������
	Vector3 forceDirection = new Vector3(1.0f, 1.0f, 0f);

	// ��s���t���O
	bool isFlying = false;//false:���łȂ�

	// �{�^�������t���O
	bool isBoostPressed = false;

	// �������蒆�t���O
	bool isCheckingDistance = false;

	// Sphere�I�u�W�F�N�g�̒�~�ʒu�i�[�p�x�N�g��
	Vector3 stopPosition = Vector3.zero;

	// Sphere�I�u�W�F�N�g�̏����ʒu�i�[�p�x�N�g��
	Vector3 initPosition = Vector3.zero;

	// Rigidbody�R���|�[�l���g�ւ̎Q�Ƃ��L���b�V��
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
		// Input.GetKeyUp�̓L�[����x�����ꂽ��A���ꂪ�����ꂽ����True��Ԃ�
		if (Input.GetKeyUp(KeyCode.B))
		{
			isBoostPressed = true;
		}

		// forceAngle�̕ύX�𔽉f����
		CalcForceDirection();
	}

	void FixedUpdate()
	{
		Debug.Log("transform x : " + gameObject.transform.position.x.ToString());
		Debug.Log("IsSleeping : " + rb.IsSleeping());

		// �����̑���
		CheckDistance();

		if (!isBoostPressed)
		{
			// �L�[�܂��̓{�^����������Ă��Ȃ����
			// �����̐؂�ւ�������������
			return;
		}
		if (isFlying)
		{
			// ��s���̏���
			StopFlying();
		}
		else
		{
			// �{�[�����΂�����
			BoostSphere();
		}
		// ��s���t���O�̐؂�ւ�
		isFlying = !isFlying;

		// �ǂ���̏��������Ă��{�^�������t���O��false��
		isBoostPressed = false;
	}

	void StopFlying()
	{
		// �^���̒�~
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;

		// �����ʒu�Ɉړ�������
		gameObject.transform.position = initPosition;

		// �������蒆�t���O��False�ɃZ�b�g
		isCheckingDistance = false;
	}

	void BoostSphere()
	{
		// �����Ɨ͂̌v�Z
		Vector3 force = forceMagnitude * forceDirection;

		// �͂������郁�\�b�h
		rb.AddForce(force, ForceMode.Impulse);

		// �������蒆�t���O��True�ɃZ�b�g
		isCheckingDistance = true;
	}

	public void OnPressedBoostButton()
	{
		isBoostPressed = true;
	}

	void CalcForceDirection()
	{
		// ���͂��ꂽ�p�x�����W�A���ɕϊ�
		float rad = forceAngle * Mathf.Deg2Rad;

		// ���ꂼ��̎��̐������v�Z
		float x = Mathf.Cos(rad);
		float y = Mathf.Sin(rad);
		float z = 0f;

		// Vector3�^�Ɋi�[
		forceDirection = new Vector3(x, y, z);
	}

	void CheckDistance()
	{
		if (!isCheckingDistance)
		{
			// �������蒆�łȂ���Ή������Ȃ�
			return;
		}
		if (rb.IsSleeping())
		{
			// �X���[�v���[�h�ɓ��������Ƃ����m�����狗�����o��
			stopPosition = gameObject.transform.position;
			float distance = GetDistanceInXZ(initPosition, stopPosition);

			// �R���\�[���ɕ\��
			Debug.Log("�򋗗��� " + distance.ToString("F2") + " ���[�g���ł��B");

			// �������蒆�t���O���I�t��
			isCheckingDistance = false;
		}
	}

	float GetDistanceInXZ(Vector3 startPos, Vector3 stopPos)
	{
		// �J�n�ʒu�A��~�ʒu�̂��ꂼ��ŁAY�������������Čv�Z�pVector3���쐬
		Vector3 startPosCalc = new Vector3(startPos.x, 0f, startPos.z);
		Vector3 stopPosCalc = new Vector3(stopPos.x, 0f, stopPos.z);

		// 2��Vector3�f�[�^���狗�����Z�o
		float distance = Vector3.Distance(startPosCalc, stopPosCalc);
		return distance;
	}
}