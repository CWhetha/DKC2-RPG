using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WorldMapTraversal : MonoBehaviour
{
    public Transform camera;
    public float minCamera;
    public float maxCamera;

    public Text worldText;

    public MapPoints activeMapPoint;
    public Animator animator;
    public int moveSpeed;
    private Direction activeDir;
    private Vector3 movement;

    private void Start()
    {
        activeMapPoint.setArrows(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && activeMapPoint.checkDirection(Direction.Right) && transform.position == activeMapPoint.transform.position)
        {
            activeMapPoint.setArrows(false);
            activeMapPoint = activeMapPoint.getNewPoint(Direction.Right);
            animator.SetBool("MoveRight", true);
            activeDir = Direction.Right;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && activeMapPoint.checkDirection(Direction.Left) && transform.position == activeMapPoint.transform.position)
        {
            activeMapPoint.setArrows(false);
            activeMapPoint = activeMapPoint.getNewPoint(Direction.Left);
            animator.SetBool("MoveRight", false);
            activeDir = Direction.Left;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && activeMapPoint.checkDirection(Direction.Up) && transform.position == activeMapPoint.transform.position)
        {
            activeMapPoint.setArrows(false);
            activeMapPoint = activeMapPoint.getNewPoint(Direction.Up);
            activeDir = Direction.Up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && activeMapPoint.checkDirection(Direction.Down) && transform.position == activeMapPoint.transform.position)
        {
            activeMapPoint.setArrows(false);
            activeMapPoint = activeMapPoint.getNewPoint(Direction.Down);
            activeDir = Direction.Down;
        }


        if (transform.position == activeMapPoint.transform.position)
        {
            activeMapPoint.setArrows(true);
            if (activeMapPoint.isMajor)
            {
                worldText.text = activeMapPoint.name;

                if (Input.GetKeyDown(KeyCode.E) && activeMapPoint.transition != "")
                {
                    SceneManager.LoadScene(activeMapPoint.transition);
                }
            }
            else
            {
                worldText.text = "";
                activeMapPoint = activeMapPoint.getNewPoint(activeDir);
            }
        }

        if (transform.position != activeMapPoint.transform.position)
        {
            movement = Vector3.MoveTowards(transform.position, activeMapPoint.transform.position, moveSpeed * Time.deltaTime);
            transform.position = movement;

            if (transform.position.y > maxCamera)
            {
                camera.position = new Vector3(camera.position.x, maxCamera, camera.position.z);
            }
            else if (transform.position.y < minCamera)
            {
                camera.position = new Vector3(camera.position.x, minCamera, camera.position.z);
            }
            else
            {
                camera.position = new Vector3(camera.position.x, transform.position.y, camera.position.z);
            }
        }
    }
}
