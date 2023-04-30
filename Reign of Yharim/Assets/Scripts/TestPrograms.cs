using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPrograms : MonoBehaviour
{
    void Start()
    {
        Debug.Log(3 * (1 - 5) + 4 * 5); //3 is a, 4 is b, 5 is t.
        Debug.Log(Mathf.Lerp(3, 4, 5) + " This is using lerp. The two values should be the same");
    }
}
