using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static event Action OnCubeSpawned = delegate { };

    private CubeSpawner[] spawners;

    public static GameState gameState; 

    private int spawnerIndex;
    private CubeSpawner currentSpawner;


    private void Start()
    {
        spawners = FindObjectsOfType<CubeSpawner>();
        gameState = GameState.Start;
    }

    private void Update()
    {
        GameStateManagement(gameState);

        if (gameState == GameState.Play)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;

                if (MovingCube.CurrentCube != null )
                {
                    MovingCube.CurrentCube.Stop();
                    AudioManager.instance.Play("drop");
                }
                spawnerIndex = spawnerIndex == 0 ? 1 : 0;
                currentSpawner = spawners[spawnerIndex];
                currentSpawner.SpawnCube();
                OnCubeSpawned();
            }
        }
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
    End,
    Restart,
    Pause,
    Settings,
    Play,
}


