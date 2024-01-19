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

    static Vector3 vec0 = new Vector3(0, 0, 0);
    static Vector3 vec01 = new Vector3(0.1f, 0.1f, 0.1f);
    static Vector3 vec5 = new Vector3(5, 5, 5);
    static Vector3 vecfol = new Vector3(2, 8, -10);

    GameObject fades;
    Fade fade;

    public new Transform camera;
    //�Œ�ʒu�A�X�e�[�W�̏����ʒu
    public GameObject[] subcamera;
    Vector3 fix;
    Quaternion fixRot;

    //�}�E�X�œ������p
    bool mouseflg=false;
    private Vector3 angle;
    private Vector3 primary_angle;

    public Text text; //���[�h�m�F�p
    MODE mode;//���[�h�Ǘ�
    bool changeflag = false;//true:�t�F�[�h�C���i�`�F���W�s�jfalse:�`�F���W�\

    //�t�H���[
    private Vector3 followpos;//�t�H���[����ꍇ�̍Œ���̋���
    public GameObject Sphere;//�L�����N�^�[
    SphereBooster sphereBooster;//�������~�߂邽�߂̃v���O�����擾�p
    private Vector3 offset;//�J�����Ƃ̋���
    float speed = 1f;//���`��Ԃ̑��x
    bool folflag=false;
    //debug
    public Text postext;

    

    // Start is called before the first frame update
    void Start()
    {
        angle = this.gameObject.transform.localEulerAngles;
        primary_angle = this.gameObject.transform.localEulerAngles;
        fades = GameObject.Find("FadePanel");
        fade = fades.GetComponent<Fade>();
        sphereBooster = Sphere.GetComponent<SphereBooster>();
        camera = GetComponent<Transform>();
        offset = transform.position - Sphere.transform.position;
        followpos = vec0;
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
                CameraMoveMouse();
                break;
            case MODE.FOLLOW:
                FollowReserve();
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

    void CameraMoveMouse()
    {

        if(Input.GetMouseButtonDown(1))
        {
            if(mouseflg)
            {
                mouseflg = false;
                return;
            }
            mouseflg=true;
        }

        if(mouseflg)
        {
            angle.y += Input.GetAxis("Mouse X");
            if (angle.y <= primary_angle.y - 30f)
            {
                angle.y = primary_angle.y - 30f;
            }
            if (angle.y >= primary_angle.y + 30f)
            {
                angle.y = primary_angle.y + 30f;
            }

            angle.x -= Input.GetAxis("Mouse Y");
            if (angle.x <= primary_angle.x - 30f)
            {
                angle.x = primary_angle.x - 30f;
            }
            if (angle.x >= primary_angle.x + 30f)
            {
                angle.x = primary_angle.x + 30f;
            }

            this.gameObject.transform.localEulerAngles = angle;
        }
       
    }
    void FollowReserve()
    {
        if(!(followpos==vec0))
        {
            transform.position = Vector3.Lerp(transform.position, followpos, speed * Time.deltaTime);
            transform.rotation= Quaternion.Lerp(transform.rotation,Quaternion.Euler(30.0f, 0f,0f), speed * Time.deltaTime);
        }
        
        if(VecComp(vec01, followpos - transform.position))
        {
            sphereBooster.Isupdate=true;
            folflag=true;
            offset = transform.position - Sphere.transform.position;
            transform.rotation = Quaternion.Euler(30.0f, 0f, 0f);
            followpos = vec0;
        }
    }
    void FollowMove()
    {
       if(folflag)
        {
            transform.position = Sphere.transform.position + offset;
        }
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
                    folflag=false;
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
        
        if (Mathf.Abs(locpos.x) > vecfol.x )
        {
            followpos = vecfol;
        }
        sphereBooster.Isupdate = false;
        postext.text = "pos" + followpos;

    }

    public bool VecComp(Vector3 ve1, Vector3 ve2)
    {

        //��ڂ̈����̂ق����傫�����true
        //�����łȂ����false
        return (ve1.x > ve2.x && ve1.y >ve2.y && ve1.z > ve2.z);
    }
}
