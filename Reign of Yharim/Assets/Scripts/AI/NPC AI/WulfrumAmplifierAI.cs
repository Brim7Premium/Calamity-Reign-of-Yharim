using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WulfrumAmplifierAI : NPC
{
    private bool spotted;
    private Color color;
    private GameObject[] AllGameObj;
    private bool WokeUp = false;
    [SerializeField] private Transform EnergyField;
    [SerializeField] private GameObject GuardDrone;

    public override void SetDefaults()
    {
        base.SetDefaults();

        NPCName = "WulfrumAmplifier";
        Damage = 0;
        LifeMax = 92;
        Life = LifeMax;
        target = GameObject.Find("Player");

        spotted = false;

    }
    public override void AI()
    {
        if (target != null)
        {
            AllGameObj = SceneManager.GetActiveScene().GetRootGameObjects();

            if(EnergyField.localScale.x < 1)
            {
                if(Vector2.Distance(target.transform.position, transform.position) <= 7 && !WokeUp)
                {
                    WokeUp = true;
                    var enemyspawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
                    for(int i = 0; i < 2; i++)
                    {
                        enemyspawner.SpawnEnemy(GuardDrone, transform.position + new Vector3(-20 + (i * 40), -17, 0));
                    }
                }
                if (WokeUp)
                {
                    EnergyField.localScale += new Vector3(Time.deltaTime, Time.deltaTime, 0);
                    EnergyField.transform.GetChild(0).localScale += new Vector3(Time.deltaTime, Time.deltaTime, 0);
                }
                if (EnergyField.localScale.x > 1)
                {
                    EnergyField.localScale = new Vector3(1, 1, 0);
                    EnergyField.transform.GetChild(0).localScale = new Vector3(1, 1, 0);
                }
            }

            foreach (var Obj in AllGameObj)
            {
                if (Obj != null)
                {
                    if (Obj.GetComponent<NPC>() != null)
                    {
                        if (Vector2.Distance(transform.position, Obj.transform.position) <= 15.2f && Obj.GetComponent<NPC>().ai[1] < 1 && Obj.GetComponent<NPC>().IsWulfrumGuy)
                        {
                            Obj.GetComponent<NPC>().ai[1] = 1; // ai 1 is amount of seconds the Wulfrum enemy will be charged
                        }
                    }
                }
            }

            if (!IsVisibleFromCamera())
            {
                spotted = false;
                ai[0] = 0.0f;
            }

            DrawDistance(transform.position, target.transform.position, color);

            if (DistanceBetween(transform.position, target.transform.position) > 60f && spotted == false)
            {
                Destroy(gameObject); //despawn the object
            }
        }
    }
}
