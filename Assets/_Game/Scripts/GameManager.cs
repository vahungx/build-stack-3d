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

    private int spawnerIndex;

    private CubeSpawner currentSpawner;


    private void Start()
    {
        spawners = FindObjectsOfType<CubeSpawner>();
       
        Common.GameStatus.Current = Common.GameStatus.GameState.Start;

    }

    private void Update()
    {
        Common.GameStatus.GameStateManagement(Common.GameStatus.Current);

        if (Common.GameStatus.Current == Common.GameStatus.GameState.Play)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;

                if (MovingCube.CurrentCube != null )
                {
                    MovingCube.CurrentCube.Stop();
                    AudioManager.instance.Play(Common.Audio.DROPPING_SOUND);
                }
                spawnerIndex = spawnerIndex == 0 ? 1 : 0;
                currentSpawner = spawners[spawnerIndex];
                currentSpawner.SpawnCube();
                OnCubeSpawned();
            }
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


