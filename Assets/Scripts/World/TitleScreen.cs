using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public string main;

    private void Update()
    {
        if (Input.anyKey)
        {
            SceneManager.LoadScene(main);
        }
    }
}
