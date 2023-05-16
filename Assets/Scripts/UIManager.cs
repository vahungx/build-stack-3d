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
    [SerializeField] private GameObject settingButton;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject muteButton;
    [SerializeField] private GameObject unMuteButton;
    [SerializeField] private GameObject closeSettingButton;

    private void Awake()
    {
        highScore = PlayerPrefs.GetInt("highScore", 0);    
    }

    private void Start()
    {
        GameManager.OnCubeSpawned += GameManager_OnCubeSpawned;// cai nay moi
    }

    private void Update()
    {
        switch (GameManager.gameState)
        {
            case GameState.Play:
                StartEnable();
                break;

            case GameState.End:
                EndEnable();
                break;

            case GameState.Restart:

                break;

            case GameState.Pause:
                PauseEnable();
                break;

            case GameState.Loader:
                break;

            default: break;
        }
    }

    private void GameManager_OnCubeSpawned()
    {   if (GameManager.gameState != GameState.End)
        {
            score++;
            scoreTMP.text = "Score: " + (score - 1).ToString();
        }
    }

    private void StartEnable()
    {
        GameManager.gameState = GameState.Start;
        startPanel.SetActive(true);
        endPanel.SetActive(false);
    }

    private void EndEnable()
    {
        GameManager.gameState = GameState.End;
        endPanel.SetActive(true);
        startPanel.SetActive(false);
        overScoreTMP.text = "Score: " + (score - 1).ToString();

        if (highScore <= score)
        {
            PlayerPrefs.SetInt("highScore", score);
        }

        highScoreTMP.text = "High Score: " + (highScore - 1).ToString();
    }

    private void PauseEnable()
    {
/*        startPanel.SetActive(false); 
        settingPanel.SetActive(true);*/
        FindAnyObjectByType<AudioManager>().Play("button");
    }

    public void Restart()
    {
        if (endPanel.activeSelf)
        {
            GameManager.gameState = GameState.Restart;
        }
        FindAnyObjectByType<AudioManager>().Play("button");
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
        FindAnyObjectByType<AudioManager>().Play("button");
    }

    public void Settings()
    {
        GameManager.gameState = GameState.Settings;
        settingButton.SetActive(false);
        closeSettingButton.SetActive(true);
        pauseButton.SetActive(false);
        playButton.SetActive(false);
        muteButton.SetActive(true);
        FindAnyObjectByType<AudioManager>().Play("button");
    }

    public void QuitSettings()
    {
        GameManager.gameState = GameState.Play;
        settingButton.SetActive(true);
        closeSettingButton.SetActive(false);
        pauseButton.SetActive(true);
        playButton.SetActive(false);
        muteButton.SetActive(false);
        FindAnyObjectByType<AudioManager>().Play("button");
    }

    public void Mute()
    {
        muteButton.SetActive(true);
        unMuteButton.SetActive(false);
        FindAnyObjectByType<AudioManager>().Play("button");
        FindAnyObjectByType<AudioManager>().Mute();
    }

    public void UnMute()
    {
        muteButton.SetActive(false);
        unMuteButton.SetActive(true);
        FindAnyObjectByType<AudioManager>().Play("button");
        FindAnyObjectByType<AudioManager>().UnMute();
    }

    public void Pause()
    {
        FindAnyObjectByType<AudioManager>().Play("button");
        GameManager.gameState = GameState.Pause;
        playButton.SetActive(true);
        pauseButton.SetActive(false);   
    }

    public void Play()
    {
        GameManager.gameState = GameState.Play;
        playButton.SetActive(false);
        pauseButton.SetActive(true);
        FindAnyObjectByType<AudioManager>().Play("button");
    }
}


