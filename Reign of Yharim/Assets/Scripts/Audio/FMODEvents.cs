using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{ 
    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference hitSound { get; private set; }

    [field: Header ("Boss Music")]
    [field: SerializeField] public EventReference DoGMusic { get; private set; }
    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMODEvents script in the scene");
        }
        instance = this;
    }
}
