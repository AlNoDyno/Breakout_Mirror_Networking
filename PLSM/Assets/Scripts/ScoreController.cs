using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : NetworkBehaviour
{
    public Text scoreText;

    [SyncVar(hook = nameof(OnScoreChanged))]
    public int score;

    private void OnEnable()
    {
        transform.SetParent(CanvasUI.instance.mainPanel);
        scoreText = GetComponent<Text>();
        score = 0;
    }

    void OnScoreChanged(int _Old, int _New)
    {
        scoreText.text = "Score: " + score.ToString();
    }
}
