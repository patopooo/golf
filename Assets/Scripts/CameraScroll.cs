using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScroll : MonoBehaviour
{
    enum MODE//モード
    {
        FIX,//ステージ全体の俯瞰
        MOVED,//自由にステージ移動可能
        FOLLOW//自動で追従
    };

    static Vector3 vec10 = new Vector3(10, 10, 10);

    SphereBooster sphereBooster;
    GameObject fades;
    Fade fade;
    public new Transform camera;
    //固定位置、ステージの初期位置
    public GameObject[] subcamera;
    Vector3 fix;
    Quaternion fixRot;
    
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
        FixPointInit(0);
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
        
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(0.0f, 0.01f, 0.0f);//上昇
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(-0.01f, 0.0f, 0.0f);//左
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(0.0f, -0.01f, 0.0f);//下降
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(0.01f, 0.0f, 0.0f);//右
        }
       
    }

    void CameraRotation()
    {


        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.Rotate(0.0f, 0.1f, 0.0f);
        }


        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.Rotate(0.0f, -0.1f, 0.0f);
        }

        if (Input.GetKey(KeyCode.E))
        {
            this.transform.Rotate(-0.1f, 0.0f, 0.0f);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            this.transform.Rotate(0.1f, 0.0f, 0.0f);
        }






        if (Input.GetKey(KeyCode.Alpha0))//0を押すと角度をリセットする
        {

            transform.rotation = Quaternion.identity;
        }
    }

    void FollowMove()
    {
       
        transform.position = Sphere.transform.position + offset;
    }

    public void ChangeMode()
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
                    FollowInit();                    
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

    public void FixPointInit(int stageNo)
    {
        fix=subcamera[stageNo].transform.position;
        fixRot= subcamera[stageNo].transform.rotation;
    }
    public void FollowInit()
    {
        Vector3 locpos = Sphere.transform.position- transform.position;
        Vector3 repos=locpos.normalized;
        offset = transform.position - Sphere.transform.position;
        transform.position = Sphere.transform.position;

        if(VecComp (locpos, Vector3.Scale(repos,vec10)))
        {
            
        }

    }

    public bool VecComp(Vector3 ve1, Vector3 ve2)
    {
        //一つ目の引数のほうが大きければtrue
        //そうでなければfalse
        return (ve1.x>=ve2.x&& ve1.y >= ve2.y&& ve1.z >= ve2.z);
    }
}
