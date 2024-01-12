using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereBooster : MonoBehaviour
{

	// ��s���t���O
	bool isFlying = false;

	// �{�^�������t���O
	bool isBoostPressed = false;

	// Sphere�I�u�W�F�N�g�̏����ʒu�i�[�p�x�N�g��
	Vector3 initPosition = Vector3.zero;

	// Rigidbody�R���|�[�l���g�ւ̎Q�Ƃ��L���b�V��
	Rigidbody rb;

	// �͂����������
	Vector3 forceDirection = new Vector3(1.0f, 1.0f, 0f);

	// ������͂̑傫��
	float forceMagnitude = 10.0f;

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
	}

	void FixedUpdate()
	{
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
	}

	void BoostSphere()
	{
		// �����Ɨ͂̌v�Z
		Vector3 force = forceMagnitude * forceDirection;

		// �͂������郁�\�b�h
		rb.AddForce(force, ForceMode.Impulse);
	}

	public void OnPressedBoostButton()
	{
		isBoostPressed = true;
	}
}