using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBlinking : MonoBehaviour
{
   [SerializeField] private GameObject aroundLight, insideLight;

    private void Start()
    {
        StartCoroutine(AroundBlink());
        StartCoroutine(InsideBlink());
    }
    private IEnumerator AroundBlink()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5f, 10f));

            aroundLight.SetActive(false);
            yield return new WaitForSeconds(Random.Range(.2f, 2f));
            aroundLight.SetActive(true);
        }

    }
    private IEnumerator InsideBlink()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 5f));

            insideLight.SetActive(false);
            yield return new WaitForSeconds(Random.Range(.2f, 2f));
            insideLight.SetActive(true);
        }

    }
}