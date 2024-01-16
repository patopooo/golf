using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideManager : MonoBehaviour
{

	// �wSphere�x�I�u�W�F�N�g�ւ̎Q��
	[SerializeField]
	GameObject player;

	// SphereBooster�ւ̎Q�Ƃ��L���b�V��
	SphereBooster sphereBooster;

	// �wSphere�x�I�u�W�F�N�g��Rigidbody�ւ̎Q�Ƃ��L���b�V��
	Rigidbody sphereRb;

	// �C���X�^���X�����ꂽGuide�I�u�W�F�N�g�̃��X�g
	List<GameObject> guideList;

	// ��ʂɃv���b�g����K�C�h�̐����`
	int prots = 8;

	void Start()
	{
		sphereBooster = player.GetComponent<SphereBooster>();
		sphereRb = player.GetComponent<Rigidbody>();
		guideList = new List<GameObject>();

		// Prefab���C���X�^���X�����郁�\�b�h���Ă�
		InstantiateGuidePrefabs();
		SetGuidePositions();
	}

	void Update()
	{

	}

	void FixedUpdate()
	{

	}

	void InstantiateGuidePrefabs()
	{
		// �wGuideParent�x�̈ʒu���wSphere�x�I�u�W�F�N�g�̈ʒu�ֈړ�
		gameObject.transform.position = player.transform.position;

		// �wGuideParent�x�̈ʒu��guidePos�ɃZ�b�g
		Vector3 guidePos = gameObject.transform.position;

		// Prefab�����[�h����
		GameObject guidePrefab = (GameObject)Resources.Load("Prefabs/Guide");

		for (int i = 0; i < prots; i++)
		{
			// Prefab���C���X�^���X��
			GameObject guideObject = (GameObject)Instantiate(guidePrefab, guidePos, Quaternion.identity);

			// �C���X�^���X�������I�u�W�F�N�g��GuideParent�̎q�I�u�W�F�N�g�ɂ���
			guideObject.transform.SetParent(gameObject.transform);

			// �I�u�W�F�N�g����ݒ肷��
			guideObject.name = "Guide_" + i.ToString();

			// ���X�g�֒ǉ�
			guideList.Add(guideObject);
		}
	}

	public void SetGuidePositions()
	{
		// �wGuideParent�x�̈ʒu���wSphere�x�I�u�W�F�N�g�̈ʒu�ֈړ�
		gameObject.transform.position = player.transform.position;

		// �wGuideParent�x�̈ʒu���J�n�ʒu�ɐݒ�
		Vector3 startPos = gameObject.transform.position;

		// ���X�g�̌���
		if (guideList == null || guideList.Count == 0)
		{
			return;
		}

		// �����w�I�ȃp�����[�^���擾
		// �wSphere�x�I�u�W�F�N�g�ɉ�����
		Vector3 force = sphereBooster.GetBoostForce();

		// �wSphere�x�I�u�W�F�N�g�̎���
		float mass = sphereRb.mass;

		// Unity�̐��E�ɓ����d��
		Vector3 gravity = Physics.gravity;

		// �wSphere�x�I�u�W�F�N�g���Ε����˂���鎞�̏����x
		Vector3 speed = force / mass;

		// �v���b�g���ɉ����āA�e�v���b�g�̎��������X�g�Ɋi�[
		List<float> timeProtsList = GetTimeProtsList(speed, gravity, prots);

		// ���X�g�̌���
		if (timeProtsList == null || timeProtsList.Count == 0)
		{
			return;
		}

		// �������X�g�����ɁA�v���b�g����K�C�h�̈ʒu��ݒ�
		for (int i = 0; i < prots; i++)
		{
			// ���X�g���玞���̒l�����o��
			float time = timeProtsList[i];

			// ���X�g�őΉ�����C���f�b�N�X�̃K�C�h�I�u�W�F�N�g�ɂ��Ĉʒu��ݒ�
			guideList[i].transform.position = GetExpectedPosition(startPos, speed, gravity, time);
		}
	}

	List<float> GetTimeProtsList(Vector3 speed, Vector3 gravity, int prots)
	{
		// �Ε����ˌ�A�n�ʂɓ��B���鎞�����v�Z
		float landingTime = -2.0f * speed.y / gravity.y;

		// �����i�[�p�̃��X�g���쐬
		List<float> timeProtsList = new List<float>();

		// �K�C�h�̃v���b�g����0�Ȃ�쐬����̒���0�̃��X�g��Ԃ�
		if (prots <= 0)
		{
			return timeProtsList;
		}

		// �v���b�g���ɉ����āA�K�C�h��\������ʒu���v�Z���邽�߂̎��������X�g�ɒǉ�
		for (int i = 1; i <= prots; i++)
		{
			float timeProt = i * landingTime / prots;
			timeProtsList.Add(timeProt);
		}
		return timeProtsList;
	}

	Vector3 GetExpectedPosition(Vector3 startPos, Vector3 speed, Vector3 gravity, float time)
	{
		// ���������ɁA�K�C�h�̈ʒu���v�Z����
		Vector3 position = (speed * time) + (gravity * 0.5f * Mathf.Pow(time, 2));
		Vector3 guidePos = startPos + position;
		return guidePos;
	}

	public void SetGuidesState(bool isActive)
	{
		// ���X�g�ɑ��݂���K�C�h�������ɉ����ĕ\��/��\��
		foreach (GameObject guide in guideList)
		{
			guide.SetActive(isActive);
		}
	}

}