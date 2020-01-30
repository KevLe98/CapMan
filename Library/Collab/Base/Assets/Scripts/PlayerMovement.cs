using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    
    public float moveSpeed = 7f;
    public GameObject gameBoard;
    public Text scoreText;
    public Text winText;
    public Text livesText;

    private Rigidbody2D rb2d;
    private int count;
    private static int score = 0;
    private static int lives = 3;
    private static int pellets;

    public NodeScript currentNode, previousNode, targetNode;
    private Vector2 nextDirection;
    public Vector2 direction = Vector2.zero;
    private Rigidbody2D playerRb;
    private Transform playerRotation;

    public AudioClip pelletClip;
    AudioSource pelletAudio;
    GameObject obj;

    void Start()
    {
        StartCoroutine(Wait());
        playerRotation = GetComponent<Transform>();
        playerRb = GetComponent<Rigidbody2D>();

        NodeScript node = GetNodeAtPosition(transform.localPosition);
        if (node != null)
        {
            currentNode = node;
        }

        direction = Vector2.left;
        ChangePosition(direction);



        playerRotation = GetComponent<Transform>();
        playerRb = GetComponent<Rigidbody2D>();
        
        count = 0;
        pellets = 286;

        livesText.text = "Lives: " + lives;
        winText.text = "";
        SetScoreText();

        obj = GameObject.Find("AudioObject");
        
        if (obj != null)
        pelletAudio = obj.GetComponent<AudioSource>();
    }

    
    void Update()
    {
        CheckInput();
        Move();
        UpdateRotation();
        UpdateAnimationState();
        CheckReturnToNode();
    }


    public void ChangePosition(Vector2 d)
    {
        if (d != direction)
        { nextDirection = d; }
        if (currentNode != null) {
            NodeScript moveToNode = CanMove(d);
            if (moveToNode != null)
            {
                direction = d;
                targetNode = moveToNode;
                previousNode = currentNode;
                currentNode = null;
            }
        }
    }

    void UpdateRotation()
    {
        if(direction == Vector2.left)
            playerRotation.rotation = Quaternion.Euler(0f, 180f, 0f);
        if (direction == Vector2.right)
            playerRotation.rotation = Quaternion.Euler(0f, 0f, 0f);
        if (direction == Vector2.up)
            playerRotation.rotation = Quaternion.Euler(0f, 0f, 90f);
        if (direction == Vector2.down)
            playerRotation.rotation = Quaternion.Euler(0f, 0f, -90f);
    }

    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
            {
            ChangePosition(Vector2.down);

        }
       
        if (Input.GetKeyDown(KeyCode.UpArrow))
            {
            ChangePosition(Vector2.up);

        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
            ChangePosition(Vector2.left);

        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
            {
            ChangePosition(Vector2.right);

        }

    }

    void Move()
    {
        if (targetNode != currentNode && targetNode != null)
        {
            if (OverShotTarget())
            {
                currentNode = targetNode;

                transform.localPosition = currentNode.transform.position;

                NodeScript moveToNode = CanMove(nextDirection);

                if (moveToNode != null)
                    direction = nextDirection;

                if (moveToNode == null)
                    moveToNode = CanMove(direction);

                if (moveToNode != null)
                {
                    targetNode = moveToNode;
                    previousNode = currentNode;
                    currentNode = null;
                }
                else { direction = Vector2.zero; }

            }
            else { transform.localPosition += (Vector3)(direction * moveSpeed) * Time.deltaTime; }
        }
    }

    void CheckReturnToNode()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) && direction == Vector2.up)
        {
            previousNode = null;;
            currentNode = targetNode;
            ChangePosition(Vector2.down);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && direction == Vector2.down)
        {
            previousNode = null; ;
            currentNode = targetNode;
            ChangePosition(Vector2.up);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && direction == Vector2.right)
        {
            previousNode = null; ;
            currentNode = targetNode;
            ChangePosition(Vector2.left);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && direction == Vector2.left)
        {
            previousNode = null; ;
            currentNode = targetNode;
            ChangePosition(Vector2.right);
        }


    }

    void UpdateAnimationState()
    {
        if (direction == Vector2.zero)
        { GetComponent<Animator>().enabled = false; }
        else { GetComponent<Animator>().enabled = true; }
    }

    bool OverShotTarget()
    {
        float nodeToTarget = LengthFromNode(targetNode.transform.position);
        float nodeToSelf = LengthFromNode(transform.localPosition);
        return nodeToSelf > nodeToTarget;
    }

    float LengthFromNode(Vector2 targetPosition)
    {
        Vector2 vec = targetPosition - (Vector2)previousNode.transform.position;
        return vec.sqrMagnitude;
    }
    
    
    void MoveToNode(Vector2 d)
    {
        NodeScript moveToNode = CanMove(d);
        if (moveToNode != null)
        {
            transform.localPosition = moveToNode.transform.position;
            currentNode = moveToNode;
        }
    }

    NodeScript CanMove (Vector2 d)
    {
        NodeScript moveToNode = null;
        for(int i = 0; i < currentNode.neighbors.Length; i++)
        {
            if (currentNode.validDirections[i] == d)
            {
                moveToNode = currentNode.neighbors[i];
                break;
            }
            
        }
        return moveToNode;
    }

    public NodeScript GetNodeAtPosition(Vector2 pos)
    {
        GameObject tile = gameBoard.GetComponent<GameBoard>().board[(int)pos.x, (int)pos.y];
        if (tile != null)
        {
            return tile.GetComponent<NodeScript>();
        }
        return null;
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.CompareTag("Pellet"))
        {
            other.gameObject.SetActive(false);
            score = score + 10;
            count = count + 1;
            pellets = pellets - 1;
            SetScoreText();



            pelletAudio.clip = pelletClip;
            pelletAudio.Play();
        }

        if (other.gameObject.CompareTag("PowerPellet"))
        {
            other.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;
            PowerPellet.active = true;
            count = count + 1;
            score = score + 15;
            pellets = pellets - 1;
            SetScoreText();

            pelletAudio.clip = pelletClip;
            pelletAudio.Play();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            lives--;
            UpdateLivesText();
            SceneManager.LoadScene("Kevin1_GhostAI");
        }
    }

    void UpdateLivesText()
    {
        livesText.text = "Lives: " + lives;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
    }

    void SetScoreText()
    {
        scoreText.text = "Score: " + score.ToString();

        if (pellets <= 0)
        {
            winText.text = "You Win";
        }

        //if (pellets <= 0)
        //{
        //   SceneManager.LoadScene("Level2");
        //}
    }
}
