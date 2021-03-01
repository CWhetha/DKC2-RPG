using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] string newScene;
    [SerializeField] Vector3 playerPosition;

    public void ChangeScene()
    {
        PlayerParty.Position = playerPosition;
        SceneManager.LoadScene(newScene);
        Time.timeScale = 1f;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            ChangeScene();
        }
    }
}
