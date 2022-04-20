using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRendererSorting : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sortingLayerName = "Canvas";
        meshRenderer.sortingOrder = 0;
    }
}