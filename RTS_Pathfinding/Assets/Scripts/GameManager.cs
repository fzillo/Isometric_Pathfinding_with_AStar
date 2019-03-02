using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Pathfinder pathfinder;

    public int startX;
    public int startY;
    public int goalX;
    public int goalY;

    void Start()
    {
        pathfinder.Init(startX, startY, goalX, goalY);
        StartCoroutine(pathfinder.GeneratePath());
    }
}
