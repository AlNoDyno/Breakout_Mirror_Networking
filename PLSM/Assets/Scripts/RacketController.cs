using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacketController : NetworkBehaviour
{
    private GameObject Parent;

    private float racketW;
    private float racketH;

    [Header("Ball")]
    public GameObject ballPrefab;

    public GameObject ballGO;
    BallController ballController = null;

    [SyncVar]
    public bool ballFlying;

    [SyncVar]
    public Vector2 ballVelocity;

    [SyncVar]
    public Vector3 ballPosition;

    private float speed = 200f;

    private KeyCode moveLEFT = KeyCode.LeftArrow;
    private KeyCode moveRIHT = KeyCode.RightArrow;
    private KeyCode launchBall = KeyCode.Space;

    void OnEnable()
    {
        Parent =  CanvasUI.instance.playersPanel.gameObject;
        transform.SetParent(CanvasUI.instance.playersPanel);

        racketW = GetComponent<RectTransform>().rect.width;
        racketH = GetComponent<RectTransform>().rect.height;

        ballFlying = false;
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.GetKey(moveRIHT))
            {
                MoveRight();
            }
            else if (Input.GetKey(moveLEFT))
            {
                MoveLeft();
            }
        }

        if (GetComponent<NetworkIdentity>().hasAuthority)//make sure this is an object that we ae controlling
        {
            if (Input.GetKey(launchBall) && !ballFlying)
            {
                CmdApplyForce();
            }
        }

        if (GetComponent<NetworkIdentity>().isClient)//if we are a client update our rigidbody with the servers rigidbody info
        {
            if (!ballFlying)
            {
                ballGO.transform.position = transform.position + new Vector3(0, ballController.ballH, 0);
            }
            else
            {
                if (BallOutofBounds())
                {
                    ballFlying = false;
                    ballGO.transform.position = transform.position + new Vector3(0, ballController.ballH, 0);
                    ballController.ballRB.velocity = Vector2.zero;
                }
            }
        }

        if (GetComponent<NetworkIdentity>().isServer)//if we are the server update the varibles with our cubes rigidbody info
        {
            if (!ballFlying)
            {
                ballPosition = transform.position + new Vector3(0, ballController.ballH, 0);
                ballGO.transform.position = ballPosition;
            }
            else
            {
                if (BallOutofBounds())
                {
                    ballFlying = false;
                    ballGO.transform.position = transform.position + new Vector3(0, ballController.ballH, 0);
                    ballController.ballRB.velocity = Vector2.zero;
                }
            }
        }
    }

    [ClientRpc]
    void RpcOnFire(Vector2 vel, Vector2 pos)
    {
        ballController.ballRB.velocity = vel;
        ballController.ballRB.position = pos;
    }

    [Command]
    public void CmdApplyForce()
    {
        ballFlying = true;

        ballVelocity = GetVelocity();
        ballController.ballRB.velocity = ballVelocity;

        ballPosition = ballGO.transform.position;
        //ballController.ballRB.position = ballPosition;

        RpcOnFire(ballVelocity, ballPosition);
    }

    private bool BallOutofBounds()
    {
        return (ballGO.transform.position.y < 0);
    }

    public Vector2 GetVelocity()
    {       
        float angleMin = Mathf.PI * 1 / 4;
        float angleMax = Mathf.PI * 3 / 4;

        float angle = Random.Range(angleMin, angleMax);

        float Vx = Mathf.Cos(angle);
        float Vy = Mathf.Sin(angle);

        return new Vector2(Vx, Vy) * 500;     
    }

    public void MoveLeft()
    {
        if (!RacketOutLeft())
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
    }

    public void MoveRight()
    {
        if (!RacketOutRight())
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
    }

    private bool RacketOutLeft()
    {
        RectTransform ParentRectTransform = Parent.GetComponentInParent<RectTransform>();

        Vector2 newPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 pLeft = new Vector2(-racketW / 2, racketH / 2) + newPosition;

        return (pLeft.x < 0);
    }

    private bool RacketOutRight()
    {
        RectTransform ParentRectTransform = Parent.GetComponentInParent<RectTransform>();

        float parentW = ParentRectTransform.rect.width;

        Vector2 newPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 pRight = new Vector2(racketW / 2, racketH / 2) + newPosition;

        return (pRight.x > parentW);
    }

    public override void OnStartServer()
    {
        Debug.Log("OnStartServer");

        if (ballGO == null)
        {
            // Instantiate the player UI as child of the Players Panel
            ballGO = Instantiate(ballPrefab, CanvasUI.instance.playersPanel);
            ballGO.transform.SetParent(CanvasUI.instance.playersPanel);

            ballController = ballGO.GetComponent<BallController>();
            ballGO.transform.position = transform.position + new Vector3(0, ballController.ballH, 0);
        }
    }

    public override void OnStopServer()
    {
        if (ballGO != null)
        {
            Destroy(ballGO);
        }
    }

    public override void OnStartClient()
    {
        Debug.Log("OnStartClient");

        if (ballGO == null)
        {
            // Instantiate the player UI as child of the Players Panel
            ballGO = Instantiate(ballPrefab, CanvasUI.instance.playersPanel);
            ballGO.transform.SetParent(CanvasUI.instance.playersPanel);

            ballController = ballGO.GetComponent<BallController>();

            if (!isLocalPlayer)
            {
                ballGO.transform.position = ballPosition;
                ballController.ballRB.velocity = ballVelocity;
            }
        }
    }

    public override void OnStopClient()
    {
        if (ballGO != null)
        {
            Destroy(ballGO);
        }
    }
}
