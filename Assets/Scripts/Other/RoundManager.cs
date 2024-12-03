using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    //=========== Unity Inspector ============
    [Header("Requierements")] 
    [SerializeField] private ZombieSpawner spawner;
    [SerializeField] private TMP_Text roundIndicator;
    
    [Header("Parameters")] 
    [Range(1, 50)]
    [SerializeField] private int zombieAmount;
    //========================================

    private int round;

    public void Start()
    {
        round = 0;
        NewRound();
    }

    public void FixedUpdate()
    {
        if(GameObject.FindGameObjectsWithTag("Zombie").Length < 1) NewRound();
    }
    
    public void NewRound()
    {
        round++;
        roundIndicator.SetText(round.ToString());
        spawner.SpawnZombies(zombieAmount);
        zombieAmount *= (int)1.25;
    }

    public void EndGame()
    {
        SceneManager.LoadScene("Menu");
    }
}
