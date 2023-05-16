using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static event Action OnCubeSpawned = delegate { };
    public static  GameState gameState;

    private CubeSpawner[] spawners;
    private int spawnerIndex;
    private CubeSpawner currentSpawner;


    private void Start()
    {
        spawners = FindObjectsOfType<CubeSpawner>();
        gameState = GameState.Start;
    }

    private void Update()
    {
        if(gameState == GameState.Start)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (MovingCube.CurrentCube != null)
                {
                    MovingCube.CurrentCube.Stop();
                    FindAnyObjectByType<AudioManager>().Play("drop");
                }
                spawnerIndex = spawnerIndex == 0 ? 1 : 0;
                currentSpawner = spawners[spawnerIndex];
                currentSpawner.SpawnCube();
                OnCubeSpawned();
            }
        }

        GameStateManagement(gameState);
    }

    private void GameStateManagement(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Start:
                Time.timeScale = 1f;
                GameManager.gameState = GameState.Play;
                break;

            case GameState.End:
                Time.timeScale = 0;
                break;

            case GameState.Restart:
                Time.timeScale = 1f;
                SceneManager.LoadScene(0);
                break;

            case GameState.Pause:
                Time.timeScale = 0; 
                break;

            case GameState.Settings:
                Time.timeScale = 0;
                break;

            case GameState.Play:
                    Time.timeScale = 1f;
                break;
            default: break;
        }
    }
}

public enum GameState
{
    Start,
    Pause,
    End,
    Loader,
    Restart,
    Settings,
    Play
}


