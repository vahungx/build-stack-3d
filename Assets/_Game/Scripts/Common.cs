using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Common
{
    public class GameStatus
    {
        public static GameState Current;
        
        public static event Action Playing = delegate { };

        public enum GameState
        {
            Start,
            Pause,
            End,
            Load,
            Restart,
            Settings,
            Play
        }

        public static void GameStateManagement(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Start:
                    Time.timeScale = 1f;
                    Current = GameState.Play;
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
                    Playing();
                    break;
                default: break;
            }
        }
    }

    public class Audio
    {
        public static string GAMEOVER = "gameover";
        public static string CLICK_BUTTON = "button";
        public static string DROPPING_SOUND = "drop";
    }
}
