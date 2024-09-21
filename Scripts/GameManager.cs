using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] TMP_Text timer;
    [SerializeField] TMP_Text killsCount;
    [SerializeField] GameObject GameOver;
    private float startTime;
    private Player player;



    private void Start()
    {
        player = GetComponent<Player>();
        startTime = Time.time;
    }

    void Update()
    {
        HpCheck();
        UpdateTimer();
        killsCount.text = PlayerStats.killCount.ToString();
    }

    void HpCheck()
    {
        if (PlayerStats.hp <= 0)
        {
            EndGame();
        }
    }

    void UpdateTimer()
    {
        float timeElapsed = Time.time - startTime;
        int minutes = Mathf.FloorToInt(timeElapsed / 60);
        int seconds = Mathf.FloorToInt(timeElapsed % 60);
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    }


    private void EndGame()
    {
        GameOver.SetActive(true);
        Invoke("PauseGame", 0.3f);

    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void NewGame()
    {
        PlayerStats.killCount = 0;
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
