using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Common
{
    public static GameState gameState;
    public enum GameState{
        Start,
        Pause,
        End,
        Loader,
        Restart,
        Settings,
        Playing
    }
}
