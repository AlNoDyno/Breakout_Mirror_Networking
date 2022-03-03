using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : NetworkBehaviour
{
    public Rigidbody2D ballRB;
    public float ballH;

    [SyncVar]
    public Vector2 Position;

    [SyncVar]
    public Vector2 Velocity;

    private float speed = 500f;

    void OnEnable()
    {
        transform.SetParent(CanvasUI.instance.playersPanel);

        ballRB = GetComponent<Rigidbody2D>();
        ballH = GetComponent<RectTransform>().rect.height;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        bool ballHit = col.transform.GetComponent<BallController>();

        if (!ballHit)
        {
            Velocity = GetVelocity(col);
            ballRB.velocity = Velocity;
        }
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
