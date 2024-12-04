using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    //=========== Unity Inspector ============
    [Header("Requirements")]
    [SerializeField] private ZombieSpawner spawner;
    [SerializeField] private TMP_Text roundIndicator;
    [SerializeField] private TMP_Text winMessage; // Reference to display "You Win"
    [SerializeField] private bool continuePlaying; // Reference to display "You Win"

    [Header("Parameters")]
    [Range(1, 50)]
    [SerializeField] private int zombieAmount;
    //========================================

    private int round;

    public void Start()
    { 
        continuePlaying = true;
        round = 0;
        NewRound();
        winMessage.gameObject.SetActive(false); // Ensure the win message is hidden initially
    }

    public void FixedUpdate()
    {
        if (GameObject.FindGameObjectsWithTag("Zombie").Length < 1&&continuePlaying==true) NewRound();
    }

    public void NewRound()
    {
        round++;
        roundIndicator.SetText(round.ToString());

        if (round == 6)
        {
            StartCoroutine(HandleWin());
            continuePlaying = false;
        }
        else
        {
            spawner.SpawnZombies(zombieAmount);
            zombieAmount = Mathf.CeilToInt(zombieAmount * 1.25f);
            roundIndicator.SetText("You Won");
        }
    }

    private IEnumerator HandleWin()
    {
        yield return new WaitForSeconds(3f); // Wait for 3 seconds
        SceneManager.LoadScene("Menu"); // Go to main menu
    }

    public void EndGame()
    {
        SceneManager.LoadScene("Menu");
    }
}
