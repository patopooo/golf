using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastForward : MonoBehaviour
{

	// 早送りのスピード
	[SerializeField]
	float scale = 2.0f;

	// ボタン押下フラグ
	bool isButtonPressed = false;

	bool isspace=false;

	void Start()
	{

	}

	void Update()
	{
		// スケールの適用
		CheckTimeScale();
	}

	void CheckTimeScale()
	{
		// スケール格納用ローカル変数
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
			// ボタンが押されている間は早送りする
			newTimeScale = scale;
		}
		
		// Unity世界の時間にスケールを適用
		Time.timeScale = newTimeScale;
	}

	public void OnPressedFastButton()
	{
		// 早送りボタンが押されたタイミングでフラグをtrueにする
		isButtonPressed = true;
	}

	public void OnReleasedFastButton()
	{
		// 早送りボタンが押されたタイミングでフラグをfalseにする
		isButtonPressed = false;
	}
}