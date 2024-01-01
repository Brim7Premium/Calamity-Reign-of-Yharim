using UnityEngine;
public class DummyAI : NPC
{
    public DialogueStuff dialogueStuff;
    public override void SetDefaults()
    {
        base.SetDefaults();

        Damage = 10;
        NPCName = "Dummy";
        LifeMax = 100;
        Life = LifeMax;
    }
    public override void AI()
    {
        int randomNumber = Random.Range(1, 100);
        Debug.Log(randomNumber);
        UpdateVelocity();
        if (target != null)
        {
            velocity = new Vector2(DirectionTo(transform.position, target.transform.position).x * 0.05f, velocity.y);

            if (1 == TargetDirection) //for looking at player
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            else
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        if (randomNumber == 50)
        {
            dialogueStuff.SetDialogue("Keep yourself safe tonight Jacob Bryson", gameObject.GetComponent<SpriteRenderer>(), 2f);
        }
    }
}
