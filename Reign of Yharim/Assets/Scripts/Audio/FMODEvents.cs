using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{ 
	[field: Header("Player SFX")]
	[field: SerializeField] public string DemonshadeEnrage;
	[field: SerializeField] public string Playerhit;

	[field: Header("Enemy SFX")]
	[field: SerializeField] public string _055Roar;
	[field: SerializeField] public string AureusStomp;
	[field: SerializeField] public string DeusSummonOrb;
	[field: SerializeField] public string TwinsTarget;

	[field: Header("Menu SFX")]
	[field: SerializeField] public string ExoTwinsHoverIcon;

	[field: Header ("Boss Music")]
	[field: SerializeField] public string AncientHorror;
	[field: SerializeField] public string BlightTerror;
	[field: SerializeField] public string DoG1;
	[field: SerializeField] public string ObsScourge;
	[field: SerializeField] public string WulfrumMother;

	[field: Header("Biome Music")]
	[field: SerializeField] public string Blight;
	[field: SerializeField] public string Day1;
	[field: SerializeField] public string Day2;
	[field: SerializeField] public string Day3;
	[field: SerializeField] public string Day4;
	[field: SerializeField] public string FullDay;
	[field: SerializeField] public string Night;

	[field: Header("Other Music")]
	[field: SerializeField] public string Title;
	[field: SerializeField] public string WulfrumArmy;

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
