using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldRotate : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.Rotate(0, 0, 1f);
    }
}