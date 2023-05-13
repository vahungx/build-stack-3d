using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class UIManager : MonoBehaviour
{
    private int score;
    private int highScore;

    [SerializeField] private TextMeshProUGUI scoreTMP;
    [SerializeField] private TextMeshProUGUI overScoreTMP;
    [SerializeField] private TextMeshProUGUI highScoreTMP;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject endPanel;

    private void Awake()
    {
        highScore = PlayerPrefs.GetInt("highScore", 0);    
    }

    private void Start()
    {
        GameManager.OnCubeSpawned += GameManager_OnCubeSpawned;// cai nay moi
        UIStartEnable();
    }

    private void Update()
    {
        if (GameManager.gameState == GameState.End)
        {
            UIEndEnable();
        }

    }

    private void GameManager_OnCubeSpawned()
    {   if (GameManager.gameState != GameState.End)
        {
            score++;
            scoreTMP.text = "Score: " + (score - 1).ToString();
        }
    }

    private void UIStartEnable()
    {
        GameManager.gameState = GameState.Start;
        startPanel.SetActive(true);
        endPanel.SetActive(false);
    }

    private void UIEndEnable()
    {
        GameManager.gameState = GameState.End;
        endPanel.SetActive(true);
        startPanel.SetActive(false);
        overScoreTMP.text = "Score: " + (score - 1).ToString();

        if (highScore <= score)
        {
            PlayerPrefs.SetInt("highScore", score);
        }

        highScoreTMP.text = "High Score:" + (highScore - 1).ToString();
    }
    public void Restart()
    {
        if (endPanel.activeSelf)
        {
            GameManager.gameState = GameState.Restart;
        }
    }

    public void Quit()
    {
        if (endPanel.activeSelf)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
               Application.Quit(); 
#endif
        }
    }
}


