using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image barImage;
    [SerializeField] private RectTransform rectTransform;
    public int maxHealth;
    public int curHealth;
    [SerializeField][Range(0, 100)]private int barStep = 69;

    private void Start()
    {
        barImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }
    private void Update()
    {
        //HelthProcent * BarSpriteSize.x * ReferencePixelsPerUnit
        //(int)(x*barStep)/(float)(barStep) will divide healthBar in discrete sprites like it was before
        //expected that the limiter and the bar are the same width and height
        float healthPercent = Mathf.Clamp01(curHealth / (float)maxHealth);
        float healthFill = (int)(healthPercent * barStep) / (float)barStep;
        barImage.fillAmount = healthFill;
        //barImage.fillAmount = 1f * curHealth / maxHealth;
    }
    public void SetMaxHealth(int health)
    {
        maxHealth = health;
        curHealth = health;
    }

    public void SetHealth(int health)
    {
        curHealth = health;
    }
}
