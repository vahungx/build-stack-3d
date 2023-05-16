using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingCube : MonoBehaviour
{
    public static MovingCube CurrentCube { get; private set; }
    internal GameState GameState { get; private set; }
    public static MovingCube LastCube { get; private set; }
    public MoveDirection MoveDirection { get; internal set; }

    [SerializeField] private float moveSpeed = 1f;

    private void Awake()
    {
        
    }

    private void OnEnable()
    {   
        if(LastCube == null)
        {
            LastCube = GameObject.Find("Start").GetComponent<MovingCube>();
        }
        CurrentCube = this;

        GetComponent<Renderer>().material.color = GetRandomColor();

        transform.localScale = new Vector3 (LastCube.transform.localScale.x, transform.localScale.y, LastCube.transform.localScale.z);
    }

    private Color GetRandomColor()
    {
        return new Color(UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f));
    }

    internal void Stop()
    {        
        moveSpeed = 0;
        float hangover = GetHangover();
        
        float max =  MoveDirection == MoveDirection.Z ? LastCube.transform.localScale.z : LastCube.transform.localScale.x;

        if (Mathf.Abs(hangover) >= max )
        {
            FindAnyObjectByType<AudioManager>().Play("gameover");
            GameManager.gameState = GameState.End;
        }

        float direction = hangover > 0 ? 1f : -1f;

        if (MoveDirection == MoveDirection.Z)
        {
            SplitCubeOnZ(hangover, direction);
        }
        else
        {
            SplitCubeOnX(hangover, direction);
        }

        LastCube = this;
    }

    private float GetHangover()
    {
        if (MoveDirection == MoveDirection.Z)
        {
            return transform.position.z - LastCube.transform.position.z;

        }
        else
        {
            return transform.position.x - LastCube.transform.position.x;
        }
    }

    private void SplitCubeOnX(float hangover, float direction)
    {
        float newXSize = LastCube.transform.localScale.x - Mathf.Abs(hangover);
        float fallingBlockSize = transform.localScale.x - newXSize;

        float newXPosition = LastCube.transform.position.x + (hangover / 2f);

        transform.localScale = new Vector3(newXSize, transform.localScale.y, transform.localScale.z);
        transform.localPosition = new Vector3(newXPosition, transform.localPosition.y, transform.localPosition.z);

        float cubeEdge = transform.position.x + (newXSize / 2f * direction);
        float fallingBlockXPosition = cubeEdge + fallingBlockSize / 2f * direction;

        SpawnDropCube(fallingBlockXPosition, fallingBlockSize);
    }

    private void SplitCubeOnZ(float hangover, float direction)
    {
        float newZSize = LastCube.transform.localScale.z - Mathf.Abs(hangover); 
        float fallingBlockSize = transform.localScale.z - newZSize;

        float newZPosition = LastCube.transform.position.z + (hangover / 2f);

        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZSize);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, newZPosition);

        float cubeEdge = transform.position.z + (newZSize / 2f * direction);
        float fallingBlockZPosition = cubeEdge + fallingBlockSize / 2f * direction;

        SpawnDropCube(fallingBlockZPosition, fallingBlockSize);
    }

    private void SpawnDropCube(float fallingBlockZPosition, float fallingBlockSize)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        if (MoveDirection == MoveDirection.Z)
        {
            cube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, fallingBlockSize);
            cube.transform.position = new Vector3(transform.position.x, transform.position.y, fallingBlockZPosition);
        }
        else
        {
            cube.transform.localScale = new Vector3(fallingBlockSize, transform.localScale.y, transform.localScale.z);
            cube.transform.position = new Vector3(fallingBlockZPosition, transform.position.y, transform.position.z);
        }
            cube.AddComponent<Rigidbody>();
            cube.GetComponent<Renderer>().material.color = GetRandomColor();
        


        Destroy(cube.gameObject, 1f);
    }

    private void Update()
    {
        if (MoveDirection == MoveDirection.Z)
        {
            transform.position += transform.forward * Time.deltaTime * moveSpeed;
        }
        else
        {
            transform.position += transform.right * Time.deltaTime * moveSpeed;
        }

    }

    internal void SpeedSetting(string speed)
    {
        if(speed == "easy")
        {
            moveSpeed = PlayerPrefs.GetFloat("easySpeed", 0.7f);
        }
        if(speed == "medium")
        {
            moveSpeed = PlayerPrefs.GetFloat("mediumSpeed", 1.5f);
        }
        if(speed == "hard")
        {
            moveSpeed = PlayerPrefs.GetFloat("hardSpeed", 2f);
        }
    }
}
