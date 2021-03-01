using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
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
