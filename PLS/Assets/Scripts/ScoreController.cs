using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    [SerializeField]
    private Text scoreLabel;

    private int score;

    private void OnEnable()
    {
        score = 0;
        scoreLabel.text = $"Score: {score}";
    }
    public void UpdateScore(int value)
    {
        score += value;
        scoreLabel.text = $"Score: {score}";
    }
}
