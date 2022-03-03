using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Brick : NetworkBehaviour
{
    public ScoreController scoreController;
    public Image img;

    [SyncVar]
    public bool alive;

    [SyncVar]
    public Color color;

    private void OnEnable()
    {
        transform.SetParent(CanvasUI.instance.bricksPanel);

        img = GetComponent<Image>();

        GetComponent<Image>().color = color;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        UpdateStatus();

        if (isServer)
        {
            scoreController.score = scoreController.score + 100;
            scoreController.scoreText.text = "Score: " + scoreController.score.ToString();
        }
    }

    public void UpdateStatus()
    {
        alive = false;

        GetComponent<Image>().enabled = alive;
        GetComponent<BoxCollider2D>().enabled = alive;
    }

    public override void OnStartClient()
    {
        Debug.Log("OnStartClient");
        GetComponent<Image>().enabled = alive;
        GetComponent<BoxCollider2D>().enabled = alive;
    }
}