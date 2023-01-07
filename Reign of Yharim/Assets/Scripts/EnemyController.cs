using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    public int iFrames = 2;
    [SerializeField] private bool immune;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Item" && immune == false)
        {
            TakeDamage(5); //damage the player for 5 damage
            StartCoroutine(Immunity()); //when the player is damaged, start courotine
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }

    IEnumerator Immunity()
    {
        immune = true; //once courotine is started, set immune to true
        yield return new WaitForSeconds(iFrames); //wait iFrames seconds
        immune = false; //set immune to false, allowing for the player to be damaged
    }
}
