using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityPrograms : MonoBehaviour
{
    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0); //locks the rotation of attached objects
    }
}
