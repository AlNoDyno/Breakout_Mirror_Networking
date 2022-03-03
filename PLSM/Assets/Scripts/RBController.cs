using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBController : NetworkBehaviour
{
    [SerializeField]
    private GameObject racketGO;

    [SerializeField]
    private GameObject ballGO;

    private GameObject Parent;

    private float racketW;
    private float racketH;

    private bool ballFlying;
    private Rigidbody2D ballRB;
    private float ballH;

    private float speed = 200f;

    private KeyCode moveLEFT = KeyCode.LeftArrow;
    private KeyCode moveRIHT = KeyCode.RightArrow;
    private KeyCode launchBall = KeyCode.Space;

    void OnEnable()
    {
        racketW = racketGO.GetComponent<RectTransform>().rect.width;
        racketH = racketGO.GetComponent<RectTransform>().rect.height;

        ballRB = ballGO.GetComponent<Rigidbody2D>();
        ballH = ballGO.GetComponent<RectTransform>().rect.height;

        ballFlying = false;
        SetBallPosition();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        Parent = GameObject.Find("Panel");
        transform.SetParent(Parent.transform, true);
        transform.localPosition = new Vector3(-200, -100, 0);
    }

    
    void Update()
    {
        if (!isLocalPlayer) return;
        
        if (Input.GetKey(moveRIHT))
        {
            MoveAllRight();
        }
        else if (Input.GetKey(moveLEFT))
        {
            MoveAllLeft();
        }
        else if (Input.GetKey(launchBall))
        {
            LaunchBall();
        }

        //ResetBallIfNeeded();
        
    }


    public void LaunchBall()
    {
        if (!ballFlying)
        {
            ballFlying = true;

            float angleMin = Mathf.PI * 1 / 4;
            float angleMax = Mathf.PI * 3 / 4;

            float angle = Random.Range(angleMin, angleMax);

            float Fx = Mathf.Cos(angle);
            float Fy = Mathf.Sin(angle);

            ballRB.AddForce(new Vector2(Fx, Fy) * 2000);
        }
    }

    public void SetBallPosition()
    {
        ballGO.transform.position = racketGO.transform.position + new Vector3(0, ballH, 0);
    }

    public void MoveLeft()
    {
        if (!RacketOutLeft())
        {
            racketGO.transform.Translate(-speed * Time.deltaTime, 0, 0);

            if (!ballFlying)
            {
                SetBallPosition();
            }
        }
    }


    public void MoveAllLeft()
    {
        if (!RacketOutLeft())
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);

            if (!ballFlying)
            {
                //SetBallPosition();
            }
        }
    }

    public void MoveAllRight()
    {
        if (!RacketOutRight())
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);

            if (!ballFlying)
            {
                //SetBallPosition();
            }
        }
    }

    public void MoveRight()
    {
        if (!RacketOutRight())
        {
            racketGO.transform.Translate(speed * Time.deltaTime, 0, 0);

            if (!ballFlying)
            {
                SetBallPosition();
            }
        }
    }

    public void ResetBallIfNeeded()
    {
        if (BallOutofBounds())
        {
            ballFlying = false;
            SetBallPosition();
            ballRB.velocity = Vector2.zero;
        }
    }

    private bool BallOutofBounds()
    {
        GameObject Parent = this.transform.parent.gameObject;
        RectTransform ParentRectTransform = Parent.GetComponentInParent<RectTransform>();

        float parentH = ParentRectTransform.rect.height / 2;

        Vector3 ballPos = ballGO.transform.position;
        Vector2 newPosition = new Vector2(ballPos.x, ballPos.y);

        return (newPosition.y < 0);
    }

    private bool RacketOutLeft()
    {
        GameObject Parent = this.transform.parent.gameObject;
        RectTransform ParentRectTransform = Parent.GetComponentInParent<RectTransform>();

        float parentW = ParentRectTransform.rect.width;

        Vector2 newPosition = new Vector2(racketGO.transform.position.x, racketGO.transform.position.y);
        Vector2 pLeft = new Vector2(-racketW / 2, racketH / 2) + newPosition;

        return (pLeft.x < 0);
    }

    private bool RacketOutRight()
    {
        GameObject Parent = this.transform.parent.gameObject;
        RectTransform ParentRectTransform = Parent.GetComponentInParent<RectTransform>();

        float parentW = ParentRectTransform.rect.width;

        Vector2 newPosition = new Vector2(racketGO.transform.position.x, racketGO.transform.position.y);
        Vector2 pRight = new Vector2(racketW / 2, racketH / 2) + newPosition;

        return (pRight.x > parentW);
    }
}
