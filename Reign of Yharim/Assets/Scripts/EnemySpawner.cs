using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject player;
    public GameObject greenSlime;

    public float regularSlimeSpawnRate = 45.0f;
    void Start()
    {
         StartCoroutine(spawnEnemy(regularSlimeSpawnRate, greenSlime));
    }
    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(player.transform.position.x - 24f, player.transform.position.x + 24f), player.transform.position.y, 0), Quaternion.identity);
        newEnemy.SetActive(true);
        StartCoroutine(spawnEnemy(interval, enemy));
    }
}
