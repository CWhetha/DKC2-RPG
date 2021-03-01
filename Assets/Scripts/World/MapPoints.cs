using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPoints : MonoBehaviour
{
    public bool isMajor;
    public string name;
    public string transition;

    public GameObject upArrow;
    public GameObject downArrow;
    public GameObject rightArrow;
    public GameObject leftArrow;

    public List<Direction> direction;
    public List<MapPoints> points;

    public bool checkDirection(Direction direct)
    {
        foreach (var dir in direction)
        {
            if (dir == direct)
            {
                return true;
            }
        }
        return false;
    }
    public MapPoints getNewPoint(Direction direct)
    {
        for (int i = 0; i < direction.Count; i++)
        {
            if (direction[i] == direct)
            {
                return points[i];
            }
        }
        return null;
    }

    public void setArrows(bool visible)
    {
        foreach (var dir in direction)
        {
            if (dir == Direction.Down)
            {
                if (downArrow)
                {
                    downArrow.SetActive(visible);
                }
            }
            else if (dir == Direction.Up)
            {
                if (upArrow)
                {
                    upArrow.SetActive(visible);
                }
            }
            else if (dir == Direction.Left)
            {
                if (leftArrow)
                {
                    leftArrow.SetActive(visible);
                }
            }
            else if (dir == Direction.Right)
            {
                if (rightArrow)
                {
                    rightArrow.SetActive(visible);
                }
            }

        }
    }
}
public enum Direction
{
    Up,
    Down,
    Right,
    Left
}
