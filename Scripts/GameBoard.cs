using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    private static int boardWidth = 1920;
    private static int boardHeight = 1080;

    public GameObject [,] board = new GameObject [boardWidth,boardHeight];

    // Start is called before the first frame update
    void Start()
    {
        Object[] objects = GameObject.FindObjectsOfType(typeof(GameObject));

        foreach (GameObject o in objects)
        {
            Vector2 pos = o.transform.position;

            if (o.name != "Player" && o.name != "Enemy")
            {
                board[(int)pos.x, (int)pos.y] = o;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
