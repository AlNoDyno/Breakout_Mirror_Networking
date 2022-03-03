using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody2D ballRB;
    private float speed = 500f;

    void OnEnable()
    {
        ballRB = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {       
        ballRB.velocity = GetVelocity(col);        
    }

    public Vector2 GetVelocity(Collision2D col)
    {
        WallController wallController = col.transform.GetComponent<WallController>();

        float x = col.relativeVelocity.x;
        float y = col.relativeVelocity.y;

        Vector2 dir;

        if (wallController != null)
        {
            if (wallController.vertical)
            {
                dir = new Vector2(x, -y).normalized;
            }
            else
            {
                dir = new Vector2(-x, y).normalized;
            }
        }
        else
        {
            dir = new Vector2(-x, y).normalized;
        }

        return (dir * speed);
    }
}
