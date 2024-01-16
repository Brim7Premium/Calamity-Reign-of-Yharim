using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableEnemy : MonoBehaviour
{
	public string condition; // boss requirement
	public int lowerSpawnChance = 1, upperSpawnChance; // this enemy will have a {lowerSpawnChance} in {upperSpawnChance} chance to spawn 
}
