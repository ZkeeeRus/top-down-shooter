using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class EnterNextRoom : MonoBehaviour
{
    private Camera camera;

    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private Vector3 playerOffset;

    private void Start()
    {
        camera = Camera.main.GetComponent<Camera>();
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("PlayerBody"))
        {
            camera.transform.position += cameraOffset;
            collider.transform.parent.position += playerOffset;
        }
    }
}