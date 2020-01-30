using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class BlinkingText : MonoBehaviour

{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Kevin1_GhostAI 1");
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            SceneManager.LoadScene("Info");
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }
}
