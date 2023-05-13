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
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (MovingCube.CurrentCube != null)
            {
                MovingCube.CurrentCube.Stop();
            }
            spawnerIndex = spawnerIndex == 0 ? 1 : 0;
            currentSpawner = spawners[spawnerIndex];
            currentSpawner.SpawnCube();
            OnCubeSpawned();
        }
        GameStateManagement(gameState);
    }

    private void GameStateManagement(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Start:
                Time.timeScale = 1f;
                break;

            case GameState.End:
                Time.timeScale = 0;
                break;

            case GameState.Restart:
                Time.timeScale = 1f;
                SceneManager.LoadScene(0);
                break;

            case GameState.Loader:
                break;

            default: break;
        }
    }
}

public enum GameState
{
    Start,
    End,
    Loader,
    Restart
}


