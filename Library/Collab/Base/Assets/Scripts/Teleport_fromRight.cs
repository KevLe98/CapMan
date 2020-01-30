using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport_fromRight : MonoBehaviour
{

    public Transform rightTarget;
    public GameObject player;
    public PlayerMovement playerMovement;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            //
        }


            if (playerMovement.direction == Vector2.right)
        {
            if (other.gameObject.CompareTag("Player"))

            {
                Debug.Log(player.transform.position);
                playerMovement.previousNode = playerMovement.GetNodeAtPosition(transform.position);
                playerMovement.targetNode = playerMovement.GetNodeAtPosition(new Vector2(rightTarget.position.x + 3.5f, rightTarget.position.x));
                playerMovement.currentNode = playerMovement.GetNodeAtPosition(rightTarget.position);
                playerMovement.ChangePosition(Vector2.right);
                player.transform.position = new Vector2(transform.position.x - 25f, transform.position.y);
                print(player.transform.position);
                //transform.position = new Vector2(-15.0f, 1.0f);
            }
        }

        if (other.gameObject.CompareTag("Enemy"))

        {
            Debug.Log("Entered right portal");
            other.gameObject.transform.position = rightTarget.position;
            //transform.position = new Vector2(15.0f, 1.0f);
        }
    }

}
