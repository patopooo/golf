using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SphereBooster : MonoBehaviour
{

	// DistanceText�I�u�W�F�N�g�ւ̎Q��
	[SerializeField]
	GameObject distanceTextObject;

	// HighScoreText�I�u�W�F�N�g�ւ̎Q��
	[SerializeField]
	GameObject highScoreTextObject;

	// PowerMeter�I�u�W�F�N�g�ւ̎Q��
	[SerializeField]
	GameObject powerMeterObject;

	// �p�x��\�����I�u�W�F�N�g
	[SerializeField]
	GameObject angleArrowObject;

	// �p�x�ύX�{�^��(��)
	[SerializeField]
	GameObject angleArrowUpButtonObject;

	// �p�x�ύX�{�^��(��)
	[SerializeField]
	GameObject angleArrowDownButtonObject;

	// GuideParent�I�u�W�F�N�g�ւ̎Q��
	[SerializeField]
	GameObject guideManagerObject;

	// ���˃{�^���I�u�W�F�N�g�ւ̎Q��
	[SerializeField]
	GameObject boostButtonObject;

	// �S�[���pUI�e�L�X�g�I�u�W�F�N�g�ւ̎Q��
	[SerializeField]
	GameObject goalTextObject;

	// ������͂̑傫��
	float forceMagnitude = 0f;

	// X������̊p�x
	float forceAngle = 45.0f;

	// �͂����������
	Vector3 forceDirection = new Vector3(1.0f, 1.0f, 0f);

	// ��s���t���O
	bool isFlying = false;

	// �{�^�������t���O
	bool isBoostPressed = false;

	// �������蒆�t���O
	bool isCheckingDistance = false;

	// Sphere�I�u�W�F�N�g�̒�~�ʒu�i�[�p�x�N�g��
	Vector3 stopPosition = Vector3.zero;

	// Rigidbody�R���|�[�l���g�ւ̎Q�Ƃ��L���b�V��
	Rigidbody rb;

	// DistanceText�I�u�W�F�N�g��Text�R���|�[�l���g�ւ̎Q�Ƃ��L���b�V��
	Text distanceText;

	// UI�e�L�X�g�̃v���t�B�b�N�X
	string distancePrefix = "Distance : ";

	// UI�e�L�X�g�̃T�t�B�b�N�X
	string distanceSuffix = " m";

	// HighScoreText�I�u�W�F�N�g��Text�R���|�[�l���g�ւ̎Q�Ƃ��L���b�V��
	Text highScoreText;

	// �n�C�X�R�A�\���̃v���t�B�b�N�X
	string highScorePrefix = "High Score : ";

	// �n�C�X�R�A�\���̃T�t�B�b�N�X
	string highScoreSuffix = " m";

	// �n�C�X�R�A�̋���
	float highScoreDistance = 0f;

	// ��������p�I�u�W�F�N�g�̃^�O
	string fallCheckerTag = "FallChecker";

	// Slider�R���|�[�l���g�ւ̎Q��
	Slider powerMeterSlider;

	// ���[�^�[�̑���
	[SerializeField]
	float meterSpeed = 0.2f;

	// ���[�^�[���ő�l�ɂȂ������̃f�B���C
	[SerializeField]
	float delayTime = 0.08f;
	float waitTime = 0f;

	// ���[�^�[������������������(true�ő�����)
	bool isMeterIncreasing = true;

	// �p�x����������������
	const int AngleIncreasing = 1;
	const int AngleDecreasing = -1;

	// �p�x�̑���
	int angleDelta;

	// ���I�u�W�F�N�g��Rect Transform�ւ̎Q�Ƃ̃L���b�V��
	RectTransform arrowRt;

	// �p�x�̏���Ɖ����̒�`
	const float MaxAngle = 180f;
	const float MinAngle = 0f;

	// Button�R���|�[�l���g�ւ̎Q�Ƃ̃L���b�V��
	Button angleUpButton;
	Button angleDownButton;

	// GuideManager�ւ̎Q�Ƃ̃L���b�V��
	GuideManager guideManager;

	// �{�[���𔭎˂��钼�O�̈ʒu
	Vector3 prePosition = Vector3.zero;

	// ���˃{�^���I�u�W�F�N�g�ւ̎Q��
	Button boostButton;

	// �{�^�����������Ԃ��ǂ����̃t���O
	bool canButtonPress = true;

	// �S�[���̃^�O
	string goalTag = "Finish";

	// �S�[���ڐG�t���O
	bool isTouchingGoal = false;

	// �S�[���ς݃t���O
	bool hasReachedGoal = false;

	void Start()
	{
		rb = gameObject.GetComponent<Rigidbody>();
		distanceText = distanceTextObject.GetComponent<Text>();
		highScoreText = highScoreTextObject.GetComponent<Text>();
		powerMeterSlider = powerMeterObject.GetComponent<Slider>();

		arrowRt = angleArrowObject.GetComponent<RectTransform>();
		angleUpButton = angleArrowUpButtonObject.GetComponent<Button>();
		angleDownButton = angleArrowDownButtonObject.GetComponent<Button>();
		guideManager = guideManagerObject.GetComponent<GuideManager>();

		boostButton = boostButtonObject.GetComponent<Button>();

		// DistanceText��HighScoreText�̏����l���Z�b�g
		SetDistanceText(0f);
		SetHighScoreText(0f);
	}

	void Update()
	{
		// �L�[�{�[�h����̓��͂��Ď�
		CheckInput();

		// forceAngle�̕ύX�𔽉f����
		CalcForceDirection();

		// powerMeter�𓮂���
		MovePowerMeter();

		// �p�x��ύX����
		ChangeForceAngle();
		ChangeArrowAngle();
	}

	void FixedUpdate()
	{
		// �����̑���
		CheckDistance();

		// �S�[���������ǂ����̊m�F
		CheckSphereState();

		if (!isBoostPressed)
		{
			// �L�[�܂��̓{�^����������Ă��Ȃ����
			// �����̐؂�ւ�������������
			return;
		}

		// Boost�{�^���������ꂽ�ꍇ�Ɉȉ��̏������s��
		canButtonPress = false;
		boostButton.interactable = canButtonPress;

		// �{�[���̔��ˏ���
		BoostSphere();

		// ��s���t���O�̐؂�ւ�
		isFlying = true;

		// �ǂ���̏��������Ă��{�^�������t���O��false��
		isBoostPressed = false;
	}

	void StopFlying()
	{
		// �^���̒�~
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;

		// �{�[�����ˑO�̈ʒu�Ɉړ�������
		gameObject.transform.position = prePosition;

		// �{�[����~��̏������Ă�
		ReadyToBoost();
	}

	void BoostSphere()
	{
		// �{�[�������˂���鎞�̈ʒu���L�^
		prePosition = transform.position;

		// �����Ɨ͂̌v�Z
		Vector3 force = forceMagnitude * forceDirection;

		// �͂������郁�\�b�h
		rb.AddForce(force, ForceMode.Impulse);

		// �������蒆�t���O��True�ɃZ�b�g
		isCheckingDistance = true;

		// �p�x�����\���ɂ���
		angleArrowObject.SetActive(false);

		// �{�^���������Ȃ��悤�ɂ���
		SetAngleButtonState(false);

		// �K�C�h���\���ɂ���
		guideManager.SetGuidesState(false);
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

		// ���݈ʒu�܂ł̋������v�Z����
		Vector3 currentPosition = gameObject.transform.position;
		float distance = GetDistanceInXZ(prePosition, currentPosition);

		// UI�e�L�X�g�ɕ\��
		SetDistanceText(distance);

		if (rb.IsSleeping())
		{
			// �n�C�X�R�A�̃`�F�b�N
			stopPosition = currentPosition;
			float currentDistance = GetDistanceInXZ(prePosition, stopPosition);

			if (currentDistance > highScoreDistance)
			{
				highScoreDistance = currentDistance;
			}
			SetHighScoreText(highScoreDistance);

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

	void SetDistanceText(float distance)
	{
		// �󂯎���������̒l���g���ĉ�ʂɕ\������e�L�X�g���Z�b�g
		distanceText.text = distancePrefix + distance.ToString("F2") + distanceSuffix;
	}

	void SetHighScoreText(float distance)
	{
		// �󂯎�����n�C�X�R�A�̒l���g���ĉ�ʂɕ\������e�L�X�g���Z�b�g
		highScoreText.text = highScorePrefix + distance.ToString("F2") + highScoreSuffix;
	}

	void OnTriggerEnter(Collider other)
	{
		// �Փ˂�������̃^�O���m�F����
		if (other.gameObject.tag == fallCheckerTag)
		{
			// ���肪��������p�I�u�W�F�N�g���������̏���
			StopFlying();
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == goalTag)
		{
			// ���肪�S�[���I�u�W�F�N�g���������̏���
			isTouchingGoal = true;
		}
	}

	void OnCollisionExit(Collision other)
	{
		if (other.gameObject.tag == goalTag)
		{
			// ���肪�S�[���I�u�W�F�N�g���������̏���
			isTouchingGoal = false;
		}
	}

	public Vector3 GetBoostForce()
	{
		// �͂̑傫���ƕ������������x�N�g����Ԃ�
		CalcForceDirection();

		// �O���̃X�N���v�g����Ă΂ꂽ���ɁA�X���C�_�[�̎Q�Ƃ��Ȃ���΃Z�b�g����
		if (powerMeterSlider == null)
		{
			powerMeterSlider = powerMeterObject.GetComponent<Slider>();
		}
		return powerMeterSlider.maxValue * forceDirection;
	}

	void MovePowerMeter()
	{
		// ��s���t���O��false�̎��Ƀ��[�^�[���㉺������
		if (isFlying)
		{
			return;
		}

		// ���E�l�̒�`
		float boundaryValue = 0f;

		// forceMagnitude��meterSpeed�̒l�������Ă����ă��[�^�[���㉺������
		if (isMeterIncreasing)
		{
			powerMeterSlider.value += meterSpeed;
			boundaryValue = powerMeterSlider.maxValue;
		}
		else
		{
			powerMeterSlider.value -= meterSpeed;
			boundaryValue = powerMeterSlider.minValue;
		}

		// ���E�l�ɂȂ����班���~�߂���Ƀ��[�^�[���t�����ɓ�����
		if (Mathf.Approximately(powerMeterSlider.value, boundaryValue))
		{
			WaitAtBoundaryValue();
		}

		// �X���C�_�[�̌��ݒl��forceMagnitude�Ɋi�[
		forceMagnitude = powerMeterSlider.value;
	}

	void WaitAtBoundaryValue()
	{
		// �O�̃t���[�����Ă΂�āA��������������܂łɂ����������Ԃ����Z
		waitTime += Time.deltaTime;

		// waitTime��delayTime�𒴂����瑝���t���O�̔��]
		if (waitTime >= delayTime)
		{
			isMeterIncreasing = !isMeterIncreasing;
			waitTime = 0f;
		}
	}

	public void OnPressedAngleUpButton()
	{
		// ��s���͊p�x�ύX�{�^���𓮍삳���Ȃ��悤�ɂ���
		if (!isFlying)
		{
			// �p�x�㏸�{�^���������ꂽ���̏���
			angleDelta = AngleIncreasing;
		}
	}

	public void OnPressedAngleDownButton()
	{
		// ��s���͊p�x�ύX�{�^���𓮍삳���Ȃ��悤�ɂ���
		if (!isFlying)
		{
			// �p�x���~�{�^���������ꂽ���̏���
			angleDelta = AngleDecreasing;
		}
	}

	public void OnReleasedAngleButton()
	{
		// ��s���͊p�x�ύX�{�^���𓮍삳���Ȃ��悤�ɂ���
		if (!isFlying)
		{
			// �p�x�̑�����0�ɂ���
			angleDelta = 0;
		}
	}

	void ChangeForceAngle()
	{
		// �p�x�̑�����0�Ȃ�A�ȍ~�̏������s��Ȃ�
		if (angleDelta == 0)
		{
			return;
		}
		// forceAngle�Ɋp�x�̑�����float�^�Ƃ��ĉ��Z
		forceAngle += angleDelta * 1.0f;

		if (angleDelta == AngleIncreasing)
		{
			// forceAngle���ő�l�𒴂����ꍇ�ɂ͍ő�l���Z�b�g
			if (forceAngle >= MaxAngle)
			{
				forceAngle = MaxAngle;
			}
		}
		else if (angleDelta == AngleDecreasing)
		{
			// forceAngle���ŏ��l��菬�����ꍇ�ɂ͍ŏ��l���Z�b�g
			if (forceAngle <= MinAngle)
			{
				forceAngle = MinAngle;
			}
		}
	}

	void ChangeArrowAngle()
	{
		// �p�x�̖����{�[���̈ʒu�Ɉړ�
		arrowRt.position = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);

		// Z���𒆐S�ɁAforceAngle�̒l�̉�]������Ċp�x�̖��ɃZ�b�g
		arrowRt.rotation = Quaternion.AngleAxis(forceAngle, Vector3.forward);

		// GuideManager��SetGuidePositions()���Ă�ŃK�C�h���ړ�������
		if (!isFlying)
		{
			guideManager.SetGuidePositions();
		}
	}

	void SetAngleButtonState(bool isInteractable)
	{
		// �{�^���������邩�ǂ����̏�Ԃ��Z�b�g����
		angleUpButton.interactable = isInteractable;
		angleDownButton.interactable = isInteractable;
	}

	void CheckInput()
	{
		// �L�[�������ꂽ���̓������`

		// Input.GetKeyUp�̓L�[����x�����ꂽ��A���ꂪ�����ꂽ����True��Ԃ�
		if (Input.GetKeyUp(KeyCode.B) && canButtonPress)
		{
			isBoostPressed = true;
		}

		// ��L�[�������ꂽ���́A�p�x�ύX�{�^��(��)�������ꂽ���̏������Ă�
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			OnPressedAngleUpButton();
		}
		else if (Input.GetKeyUp(KeyCode.UpArrow))
		{
			OnReleasedAngleButton();
		}

		// ���L�[�������ꂽ���́A�p�x�ύX�{�^��(��)�������ꂽ���̏������Ă�
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			OnPressedAngleDownButton();
		}
		else if (Input.GetKeyUp(KeyCode.DownArrow))
		{
			OnReleasedAngleButton();
		}
	}

	void CheckSphereState()
	{
		if (!rb.IsSleeping() || hasReachedGoal)
		{
			return;
		}
		if (isTouchingGoal)
		{
			// �S�[���Ŏ~�܂������̏���
			StartCoroutine(GoalAnimation(0.8f));
			hasReachedGoal = true;
		}
		else
		{
			// �S�[���ȊO�Ŏ~�܂������̏���
			ReadyToBoost();
		}
	}

	void ReadyToBoost()
	{
		// ��s���t���O��False�ɃZ�b�g
		isFlying = false;

		// �������蒆�t���O��False�ɃZ�b�g
		isCheckingDistance = false;

		// ���݂̋��������Z�b�g
		SetDistanceText(0f);

		// �p�x����\������
		angleArrowObject.SetActive(true);

		// �{�^����������悤�ɂ���
		SetAngleButtonState(true);

		// �K�C�h��\������
		guideManager.SetGuidesState(true);

		// �{�[���̉^����Ԃ��m�F
		canButtonPress = true;
		boostButton.interactable = canButtonPress;
	}

	IEnumerator GoalAnimation(float time)
	{
		// �S�[�����̃A�j���[�V��������
		RectTransform rt = goalTextObject.GetComponent<RectTransform>();

		// �S�[���e�L�X�g�I�u�W�F�N�g�̏����ʒu�ƈړ���ʒu
		Vector3 initTextPos = rt.localPosition;
		Vector3 targetPos = Vector3.zero;

		// �����œn���ꂽ�b���𑫂�������(���ݎ����ɑ���)
		float finishTime = Time.time + time;

		while (true)
		{
			// ���������̎����ɒB�������̊m�F
			float diff = finishTime - Time.time;
			if (diff <= 0)
			{
				break;
			}
			// Lerp���v�Z���邽�߂Ɏ��Ԑi�s�x���v�Z
			float rate = 1 - Mathf.Clamp01(diff / time);

			// �����ʒu����ړ���ʒu�܂ł̒�����ŁA�����ɉ������ʒu���Z�b�g
			rt.localPosition = Vector3.Lerp(initTextPos, targetPos, rate);

			// 1�t���[���ҋ@
			yield return null;
		}
		// �ړ���ʒu���Z�b�g
		rt.localPosition = targetPos;
	}
}