using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject racketGO;

    [SerializeField]
    private GameObject ballGO;

    private GameObject Parent;
    private RectTransform ParentRectTransform;
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
        Parent = this.transform.parent.gameObject;
        ParentRectTransform = Parent.GetComponentInParent<RectTransform>();

        racketW = racketGO.GetComponent<RectTransform>().rect.width;
        racketH = racketGO.GetComponent<RectTransform>().rect.height;

        ballRB = ballGO.GetComponent<Rigidbody2D>();
        ballH = ballGO.GetComponent<RectTransform>().rect.height;

        ballFlying = false;
        SetBallPosition();
    }

    void Update()
    {
        if (Input.GetKey(moveRIHT))
        {
            MoveRight();
        }
        else if (Input.GetKey(moveLEFT))
        {
            MoveLeft();
        }
        else if (Input.GetKey(launchBall))
        {
            LaunchBall();
        }

        if (ballFlying)
        {
            ResetBallIfNeeded();
        }
    }

    public void LaunchBall()
    {
        if (!ballFlying)
        {
            ballFlying = true;
            ballRB.velocity = GetVelocity();
        }
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
        float parentH = ParentRectTransform.rect.height / 2;
        Vector3 ballPos = ballGO.transform.localPosition;
        Vector2 newPosition = new Vector2(ballPos.x, ballPos.y);

        return (newPosition.y < -parentH);
    }

    private bool RacketOutLeft()
    {
        float parentW = ParentRectTransform.rect.width / 2;
        Vector2 newPosition = new Vector2(racketGO.transform.localPosition.x, racketGO.transform.localPosition.y);

        Vector2 pLeft = new Vector2(-racketW / 2, racketH / 2) + newPosition;

        return (pLeft.x < -parentW);
    }

    private bool RacketOutRight()
    {    
        float parentW = ParentRectTransform.rect.width / 2;
        Vector2 newPosition = new Vector2(racketGO.transform.localPosition.x, racketGO.transform.localPosition.y);

        Vector2 pRight = new Vector2(racketW / 2, racketH / 2) + newPosition;

        return (pRight.x > parentW);
    }
}
