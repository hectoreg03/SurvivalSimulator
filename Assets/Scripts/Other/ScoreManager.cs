using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    //========= Unity Inspector ===========
    [Header("Requirements")] [SerializeField]
    private TMP_Text scoreText;
    //=====================================
    
    private int score = 0;
    void Start()
    {
        UpdateScoreText();
    }

    public void AddToScore(int n)
    {
        this.score += n;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = this.score.ToString();
    }
}
