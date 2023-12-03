using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{ 
	[field: Header("Player SFX")]
	[field: SerializeField] public EventReference Playerhit { get; private set; }
	[field: SerializeField] public EventReference DemonshadeEnrage { get; private set; }
	[field: SerializeField] public EventReference AstralBeaconOrbs { get; private set; }

	[field: Header("Boss SFX")]
	[field: SerializeField] public EventReference AureusStomp { get; private set; }
	[field: SerializeField] public EventReference _055Roar { get; private set; }

	[field: Header("Menu SFX")]
	[field: SerializeField] public EventReference ExoTwinsHoverIcon { get; private set; }

	[field: Header ("Boss Music")]
	[field: SerializeField] public EventReference DoG1 { get; private set; }

	[field: Header("Biome Music")]
	[field: SerializeField] public EventReference FullDay { get; private set; }
	[field: SerializeField] public EventReference Day1 { get; private set; }
	[field: SerializeField] public EventReference Day2 { get; private set; }
	[field: SerializeField] public EventReference Day3 { get; private set; }
	[field: SerializeField] public EventReference Day4 { get; private set; }
	[field: SerializeField] public EventReference Night { get; private set; }

	[field: Header("Other Music")]
	[field: SerializeField] public EventReference Title { get; private set; }
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
