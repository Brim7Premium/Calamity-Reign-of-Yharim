using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image barImage;
    [SerializeField] private Sprite[] barSprites = new Sprite[52];
    private float[] healthValues = new float[52];
    public int maxHealth;
    public int curHealth;

    private void Start() //on start
    {
        barImage = GetComponent<Image>(); //the image variable is equal to the attached image component
        CalculateHealthValues(); //calculate the health values
    }
    private void Update() //every frame
    {
        for (int i = 0; i < 51; i++) //loop through 51 times, increasing i every loop (i starts at 1)
        {
            if (curHealth >= healthValues[i] && curHealth < healthValues[i + 1]) //if the current player health is greater than value i of healthvalues and the current player health is less than the value i of healthvalues + 1 (if the player's current health is between two numbers of the array) 
                barImage.sprite = barSprites[i]; //set the sprite of the healthbar to whatever value i is on
            if (curHealth >= healthValues[51] && curHealth < maxHealth + 1)
                barImage.sprite = barSprites[51];
        }

    }
    public void SetMaxHealth(int health)
    {
        maxHealth = health;
        curHealth = health;
        CalculateHealthValues();
    }

    public void SetHealth(int health)
    {
        curHealth = health;
    }
    private void CalculateHealthValues()
    {
        float divHealth = maxHealth / 52f;

        for (int i = 0; i < 52; i++)
        {
            healthValues[i] = divHealth * i;
        }
    }
}
