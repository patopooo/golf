using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title_Level1 : MonoBehaviour
{
    public void Change_button()
    {
        SceneManager.LoadScene("golf2");
    }
}