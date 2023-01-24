using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject greenSlime;

    public float regularSlimeSpawnRate = 45.0f;
    void Start()
    {
         StartCoroutine(spawnEnemy(regularSlimeSpawnRate, greenSlime));
    }
    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(-5f, 5), Random.Range(-6f, 6f), 0), Quaternion.identity);
        newEnemy.SetActive(true);
        StartCoroutine(spawnEnemy(interval, enemy));
    }
}
