using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    //============ Unity Inspector ===========
    [Header("Requirements")] 
    [SerializeField] private TMP_Text healthIndicator;
    [SerializeField] private RoundManager roundManager;
    //========================================

    private int health;
    void Start()
    {
        health = 100;
        UpdateHealthIndicator();
    }

    void UpdateHealthIndicator()
    {
        healthIndicator.SetText(health.ToString() + "/100");
    }

    public void Damage(int n)
    {
        health -= n;
        if(health >= 0) UpdateHealthIndicator();
        if (health <= 0)
        {
            roundManager.EndGame();
        }
        
    }
}
