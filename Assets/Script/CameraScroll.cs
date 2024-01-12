using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScroll : MonoBehaviour
{
    enum MODE//モード
    {
        FIX,
        MOVED,
        FOLLOW
    };
    SphereBooster sphereBooster;
    GameObject fades;
    Fade fade;
    public new Transform camera;
    //デバッグ
    public Vector3 fix;
    public Quaternion fixRot;
    
    public Text text; //モード確認用
    MODE mode;//モード管理
    bool changeflag = false;//true:フェードイン（チェンジ不可）false:チェンジ可能

    //フォロー
    public GameObject Sphere;//キャラクター
    private Vector3 offset;//カメラとの距離

    // Start is called before the first frame update
    void Start()
    {
        fades = GameObject.Find("FadePanel");
        fade = fades.GetComponent<Fade>();
        sphereBooster = GetComponent<SphereBooster>();
        camera = GetComponent<Transform>();
        offset = transform.position - Sphere.transform.position;
        mode = MODE.FIX;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        switch (mode)
        {
            case MODE.FIX:
                camera.transform.position = fix;
                camera.transform.rotation = fixRot;
                break;
            case MODE.MOVED:
                CameraMove();
                CameraRotation();
                break;
            case MODE.FOLLOW:
                FollowMove();
                break;
            default:
                return;
        }
        ChangeMode();

    }

    void CameraMove()
    {
        if(Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(0.0f, 0.0f, 0.01f);//前方
        }
        if(Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(-0.01f, 0.0f, 0.0f);//左
        }
        if(Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(0.0f, 0.0f, -0.01f);//後方
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(0.01f, 0.0f, 0.0f);//右
        }
        if (Input.GetKey(KeyCode.E))
        {
            this.transform.Translate(0.0f, 0.01f, 0.0f);//上昇
        }
        if(Input.GetKey(KeyCode.Q))
        {
            this.transform.Translate(0.0f, -0.01f, 0.0f);//下降
        }
    }

    void CameraRotation()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.Rotate(0.0f, 0.1f, 0.0f);
        }
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.Rotate(0.0f, -0.1f, 0.0f);
        }
    }

    void FollowMove()
    {
        transform.position = Sphere.transform.position + offset;
    }

    void ChangeMode()
    {
        if (!fade.isFadeout&&!fade.isFadeIn)
        {
            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                fade.isFadeout = true;
                changeflag = true;
            }
        }
        if(changeflag&&fade.isFadeIn)
        {
            switch (mode)
            {
                case MODE.FIX:
                    mode = MODE.MOVED;
                    text.text = "CameraMove:MOVED";
                    break;
                case MODE.MOVED:
                    mode = MODE.FOLLOW;
                    text.text = "CameraMove:FOLLOW";
                    break;
                case MODE.FOLLOW:
                    mode = MODE.FIX;
                    text.text = "CameraMove:FIX";
                    break;
            }
            changeflag = false;
        }
    }
}
