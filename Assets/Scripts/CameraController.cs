using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{

	// �wSphere�x�I�u�W�F�N�g�ւ̎Q��
	[SerializeField]
	GameObject player;

	// �wSlider�x�I�u�W�F�N�g�ւ̎Q��
	[SerializeField]
	GameObject sliderObject;

	// �wZoomText�x�I�u�W�F�N�g�ւ̎Q��
	[SerializeField]
	GameObject zoomTextObject;

	// �wSphere�x�I�u�W�F�N�g�ƁwMainCamera�x�I�u�W�F�N�g�̋���
	Vector3 offset;

	// Slider�R���|�[�l���g�̎Q��
	Slider zoomSlider;

	// Text�R���|�[�l���g�̎Q��
	Text zoomText;

	// �g�嗦�̃e�L�X�g�ŕϐ��ȊO�̕������`
	string zoomTextPrefix = "Magnification : ";
	string zoomTextSuffix = "%";

	// �g�嗦�̍ŏ��l�ƍő�l
	const int OffsetMin = 50;
	const int OffsetMax = 150;

	// �J�����̊g�嗦(%)
	[SerializeField, Range(OffsetMin, OffsetMax)]
	int magnify = 100;

	void Start()
	{
		// �I�t�Z�b�g���v�Z����
		offset = gameObject.transform.position - player.transform.position;

		// �Q�Ƃ��擾
		zoomSlider = sliderObject.GetComponent<Slider>();
		zoomText = zoomTextObject.GetComponent<Text>();
	}

	void LateUpdate()
	{
		// �J�����̊g�嗦�ɉ������I�t�Z�b�g���擾
		Vector3 magnifiedOffset = GetMagnifiedOffset();

		// �wSphere�x�I�u�W�F�N�g�ƃI�t�Z�b�g����J�����̌��݈ʒu���v�Z
		gameObject.transform.position = player.transform.position + magnifiedOffset;
	}

	Vector3 GetMagnifiedOffset()
	{
		// �K�i�����ꂽ�I�t�Z�b�g���擾
		Vector3 normalizedOffset = offset.normalized;

		// �wSphere�x�I�u�W�F�N�g�ƃJ�����̋������擾
		float offsetDistance = offset.magnitude;

		// offsetDistance�Ɋg�嗦�������ĕ␳��̋������擾
		float magnifiedDistance = offsetDistance * (200f - magnify) / 100f;

		// �K�i�����ꂽ�x�N�g���Ɗg���̋�������I�t�Z�b�g��Ԃ�
		Vector3 magnifiedOffset = magnifiedDistance * normalizedOffset;
		return magnifiedOffset;
	}

	public void OnChangedMagnifyValue()
	{
		// Slider�̒l���g�嗦�ɔ��f
		magnify = (int)zoomSlider.value;

		// ZoomText�ɕ������ݒ�
		zoomText.text = zoomTextPrefix + magnify.ToString() + zoomTextSuffix;
	}
}