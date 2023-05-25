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
        AudioManager.instance.Play("button");
    }

    public void Restart()
    {
        if (endPanel.activeSelf)
        {
            GameManager.gameState = GameState.Restart;
        }
        AudioManager.instance.Play("button");
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
        AudioManager.instance.Play("button");
    }

    public void Settings()
    {
        GameManager.gameState = GameState.Settings;
        settingButton.SetActive(false);
        closeSettingButton.SetActive(true);
        pauseButton.SetActive(false);
        playButton.SetActive(false);
        muteButton.SetActive(true);
        AudioManager.instance.Play("button");
    }

    public void QuitSettings()
    {
        GameManager.gameState = GameState.Play;
        settingButton.SetActive(true);
        closeSettingButton.SetActive(false);
        pauseButton.SetActive(true);
        playButton.SetActive(false);
        muteButton.SetActive(false);
        unMuteButton.SetActive(false);
        AudioManager.instance.Play("button");
    }

    public void Mute()
    {
        muteButton.SetActive(false);
        unMuteButton.SetActive(true);
        AudioManager.instance.Play("button");
        AudioManager.instance.Mute();
    }

    public void UnMute()
    {
        muteButton.SetActive(true);
        unMuteButton.SetActive(false);
        AudioManager.instance.Play("button");
        AudioManager.instance.UnMute();
    }

    public void Pause()
    {
        AudioManager.instance.Play("button");
        GameManager.gameState = GameState.Pause;
        playButton.SetActive(true);
        pauseButton.SetActive(false);   
    }

    public void Play()
    {
        GameManager.gameState = GameState.Play;
        playButton.SetActive(false);
        pauseButton.SetActive(true);
        AudioManager.instance.Play("button");
    }
}
