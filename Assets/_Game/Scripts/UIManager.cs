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
        GameManager.OnCubeSpawned += GameManager_OnCubeSpawned;
        Common.GameStatus.Playing += ReloadTimeScale;
    }

    private void ReloadTimeScale()
    {
        Time.timeScale = 1;
    }

    private void Update()
    {
        switch (Common.GameStatus.Current)
        {
            case Common.GameStatus.GameState.Play:
                StartEnable();
                break;

            case Common.GameStatus.GameState.End:
                EndEnable();
                break;

            case Common.GameStatus.GameState.Restart:

                break;

            case Common.GameStatus.GameState.Pause:
                PauseEnable();
                break;

            default: break;
        }
    }

    private void GameManager_OnCubeSpawned()
    {   if (Common.GameStatus.Current != Common.GameStatus.GameState.End)
        {
            score++;
            scoreTMP.text = "Score: " + (score - 1).ToString();
        }
    }

    private void StartEnable()
    {
        Common.GameStatus.Current = Common.GameStatus.GameState.Play;
        startPanel.SetActive(true);
        endPanel.SetActive(false);
    }

    private void EndEnable()
    {
       Common.GameStatus.Current = Common.GameStatus.GameState.End;
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
        AudioManager.instance.Play(Common.Audio.CLICK_BUTTON);
    }

    public void Restart()
    {
        if (endPanel.activeSelf)
        {
            Common.GameStatus.Current = Common.GameStatus.GameState.Restart;
        }
        AudioManager.instance.Play(Common.Audio.CLICK_BUTTON);
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
        AudioManager.instance.Play(Common.Audio.CLICK_BUTTON);
    }

    public void Settings()
    {
        Common.GameStatus.Current = Common.GameStatus.GameState.Settings;
        settingButton.SetActive(false);
        closeSettingButton.SetActive(true);
        pauseButton.SetActive(false);
        playButton.SetActive(false);
        muteButton.SetActive(true);
        AudioManager.instance.Play(Common.Audio.CLICK_BUTTON);
    }

    public void QuitSettings()
    {
        Common.GameStatus.Current = Common.GameStatus.GameState.Play;
        settingButton.SetActive(true);
        closeSettingButton.SetActive(false);
        pauseButton.SetActive(true);
        playButton.SetActive(false);
        muteButton.SetActive(false);
        unMuteButton.SetActive(false);
        AudioManager.instance.Play(Common.Audio.CLICK_BUTTON);
    }

    public void Mute()
    {
        muteButton.SetActive(false);
        unMuteButton.SetActive(true);
        AudioManager.instance.Play(Common.Audio.CLICK_BUTTON);
        AudioManager.instance.Mute();   
    }

    public void UnMute()
    {
        muteButton.SetActive(true);
        unMuteButton.SetActive(false);
        AudioManager.instance.Play(Common.Audio.CLICK_BUTTON);
        AudioManager.instance.UnMute();
    }

    public void Pause()
    {
        AudioManager.instance.Play(Common.Audio.CLICK_BUTTON);
        Common.GameStatus.Current = Common.GameStatus.GameState.Pause;
        playButton.SetActive(true);
        pauseButton.SetActive(false);   
    }

    public void Play()
    {
        Common.GameStatus.Current = Common.GameStatus.GameState.Play;
        playButton.SetActive(false);
        pauseButton.SetActive(true);
        AudioManager.instance.Play(Common.Audio.CLICK_BUTTON);
    }
}
