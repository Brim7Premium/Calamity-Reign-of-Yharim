using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitReset : MonoBehaviour
{
    [SerializeField] private Transform orbitPoint;

    private void Update()
    {
        if (orbitPoint.transform.rotation == Quaternion.Euler(0f, 0f, -220f))
        {
            orbitPoint.transform.rotation = Quaternion.Euler(0f, 0f, -40f);
        }
    }
}
