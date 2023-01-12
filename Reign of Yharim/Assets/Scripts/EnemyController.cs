using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    public float enemyIFrames = 1f;

    [SerializeField] private bool enemyImmune;

    [SerializeField] private Animator playerAnimator;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Item" && enemyImmune == false)
        {
            if (playerAnimator.GetCurrentAnimatorStateInfo(1).IsName("Swing") == true)
            {
                TakeDamage(5); //damage the player for 5 damage
                StartCoroutine(EnemyImmunity()); //when the player is damaged, start courotine
            }
        }
    }
    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }

    IEnumerator EnemyImmunity()
    {
        enemyImmune = true; //once courotine is started, set immune to true
        yield return new WaitForSeconds(enemyIFrames); //wait iFrames seconds
        enemyImmune = false; //set immune to false, allowing for the player to be damaged
    }
}
