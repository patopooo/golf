using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{

	// 『Sphere』オブジェクトへの参照
	[SerializeField]
	GameObject player;

	// 『Slider』オブジェクトへの参照
	[SerializeField]
	GameObject sliderObject;

	// 『ZoomText』オブジェクトへの参照
	[SerializeField]
	GameObject zoomTextObject;

	// 『Sphere』オブジェクトと『MainCamera』オブジェクトの距離
	Vector3 offset;

	// Sliderコンポーネントの参照
	Slider zoomSlider;

	// Textコンポーネントの参照
	Text zoomText;

	// 拡大率のテキストで変数以外の部分を定義
	string zoomTextPrefix = "Magnification : ";
	string zoomTextSuffix = "%";

	// 拡大率の最小値と最大値
	const int OffsetMin = 50;
	const int OffsetMax = 150;

	// カメラの拡大率(%)
	[SerializeField, Range(OffsetMin, OffsetMax)]
	int magnify = 100;

	void Start()
	{
		// オフセットを計算する
		offset = gameObject.transform.position - player.transform.position;

		// 参照を取得
		zoomSlider = sliderObject.GetComponent<Slider>();
		zoomText = zoomTextObject.GetComponent<Text>();
	}

	void LateUpdate()
	{
		// カメラの拡大率に応じたオフセットを取得
		Vector3 magnifiedOffset = GetMagnifiedOffset();

		// 『Sphere』オブジェクトとオフセットからカメラの現在位置を計算
		gameObject.transform.position = player.transform.position + magnifiedOffset;
	}

	Vector3 GetMagnifiedOffset()
	{
		// 規格化されたオフセットを取得
		Vector3 normalizedOffset = offset.normalized;

		// 『Sphere』オブジェクトとカメラの距離を取得
		float offsetDistance = offset.magnitude;

		// offsetDistanceに拡大率をかけて補正後の距離を取得
		float magnifiedDistance = offsetDistance * (200f - magnify) / 100f;

		// 規格化されたベクトルと拡大後の距離からオフセットを返す
		Vector3 magnifiedOffset = magnifiedDistance * normalizedOffset;
		return magnifiedOffset;
	}

	public void OnChangedMagnifyValue()
	{
		// Sliderの値を拡大率に反映
		magnify = (int)zoomSlider.value;

		// ZoomTextに文字列を設定
		zoomText.text = zoomTextPrefix + magnify.ToString() + zoomTextSuffix;
	}
}