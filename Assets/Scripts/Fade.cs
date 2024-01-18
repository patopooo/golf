using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Fade : MonoBehaviour
{
    public bool isFadeout = false;
    public bool isFadeIn = false;
    float speed = 0.005f;
    Image fadepanel;
    float red, green, blue, alpha;

    // Start is called before the first frame update
    void Start()
    {
        fadepanel = GetComponent<Image>();
        red = fadepanel.color.r;
        green = fadepanel.color.g;
        blue = fadepanel.color.b;
        alpha = fadepanel.color.a;
        isFadeIn = true;

    }
    // Update is called once per frame
    void Update()
    {
        if (isFadeout)
        {
            FadeOut();
        }
        if (isFadeIn)
        {
            SetFadeIn();
        }
    }

    void FadeOut()
    {
        alpha += speed;
        fadepanel.color = new Color(red, green, blue, alpha);
        if (alpha >= 1)
        {

            isFadeout = false;
            //this.ball.transform.position = ;
            isFadeIn = true;
        }
    }

    void SetFadeIn()
    {
        alpha -= speed;
        fadepanel.color = new Color(red, green, blue, alpha);
        if (alpha <= 0)
        {
            isFadeIn = false;
        }

    }

}
