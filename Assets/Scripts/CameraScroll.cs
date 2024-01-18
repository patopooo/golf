using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScroll : MonoBehaviour
{
    enum MODE//���[�h
    {
        FIX,//�X�e�[�W�S�̘̂���
        MOVED,//���R�ɃX�e�[�W�ړ��\
        FOLLOW//�����ŒǏ]
    };

    static Vector3 vec10 = new Vector3(10, 10, 10);

    SphereBooster sphereBooster;
    GameObject fades;
    Fade fade;
    public new Transform camera;
    //�Œ�ʒu�A�X�e�[�W�̏����ʒu
    public GameObject[] subcamera;
    Vector3 fix;
    Quaternion fixRot;
    
    public Text text; //���[�h�m�F�p
    MODE mode;//���[�h�Ǘ�
    bool changeflag = false;//true:�t�F�[�h�C���i�`�F���W�s�jfalse:�`�F���W�\

    //�t�H���[
    public GameObject Sphere;//�L�����N�^�[
    private Vector3 offset;//�J�����Ƃ̋���

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
            this.transform.Translate(0.0f, 0.01f, 0.0f);//�㏸
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(-0.01f, 0.0f, 0.0f);//��
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(0.0f, -0.01f, 0.0f);//���~
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(0.01f, 0.0f, 0.0f);//�E
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






        if (Input.GetKey(KeyCode.Alpha0))//0�������Ɗp�x�����Z�b�g����
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
        //��ڂ̈����̂ق����傫�����true
        //�����łȂ����false
        return (ve1.x>=ve2.x&& ve1.y >= ve2.y&& ve1.z >= ve2.z);
    }
}
