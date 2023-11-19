using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    private int count = 660;
    public static string displayTime;

    [SerializeField] private Transform orbitPoint;
    [SerializeField] private Vector3 rotation;

    IEnumerator Start()
    {
        while (true)
        {
            if(count == 24*60){count = 0;}

            displayTime = $"{count/60:d2} : {count%60:d2}";

            yield return new WaitForSeconds(1);
            count++;
        }
    }

    void Update() => orbitPoint.rotation = Quaternion.Euler(0, 0, (count-900)/8.0f);
}
