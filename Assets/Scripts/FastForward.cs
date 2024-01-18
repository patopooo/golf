using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastForward : MonoBehaviour
{

	// ������̃X�s�[�h
	[SerializeField]
	float scale = 2.0f;

	// �{�^�������t���O
	bool isButtonPressed = false;

	bool isspace=false;

	void Start()
	{

	}

	void Update()
	{
		// �X�P�[���̓K�p
		CheckTimeScale();
	}

	void CheckTimeScale()
	{
		// �X�P�[���i�[�p���[�J���ϐ�
		float newTimeScale = 1.0f;

		if (Input.GetKeyDown(KeyCode.Space))
		{
			if(isspace)
            {
				isspace = false;
				return;
            }
			isspace = true;
		}

		if (isButtonPressed||isspace)
		{
			// �{�^����������Ă���Ԃ͑����肷��
			newTimeScale = scale;
		}
		
		// Unity���E�̎��ԂɃX�P�[����K�p
		Time.timeScale = newTimeScale;
	}

	public void OnPressedFastButton()
	{
		// ������{�^���������ꂽ�^�C�~���O�Ńt���O��true�ɂ���
		isButtonPressed = true;
	}

	public void OnReleasedFastButton()
	{
		// ������{�^���������ꂽ�^�C�~���O�Ńt���O��false�ɂ���
		isButtonPressed = false;
	}
}