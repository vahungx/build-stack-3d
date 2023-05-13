using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform followTarget;
    private Vector3 offset;

    [SerializeField] private float smoothSpeed = 0.04f;

    private void Start()
    {
        followTarget = MovingCube.CurrentCube.transform;
        offset = transform.position - followTarget.position;
    }
    private void Update()
    {   
        followTarget = MovingCube.CurrentCube.transform;
    }

    private void LateUpdate()
    {
        Vector3 newPosition = Vector3.Lerp(transform.position, new Vector3(transform.position.x, followTarget.position.y + offset.y, transform.position.z), smoothSpeed);
        transform.position = newPosition;
    }
}
