using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Brick : MonoBehaviour
{
    public ScoreController scoreController;
    public Image img;

    private void OnEnable()
    {
        img = GetComponent<Image>();
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        GetComponent<Image>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;

        scoreController.UpdateScore(100);
    }
}
