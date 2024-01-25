using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableEnemy : MonoBehaviour
{
	public string condition; // boss requirement
	public int lowerSpawnChance = 1, upperSpawnChance = 0; // this enemy will have a {lowerSpawnChance} in {upperSpawnChance} chance to spawn 
	public string eventName; // the event this enemy belongs to (if any)
	public float eventValue = 0.0f; // every time you kill an enemy during an event, this is how much it counts towards completion
}
