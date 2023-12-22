using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using FMOD.Studio;
using FMODUnity;
using TMPro;

public class GreenSlimeAI : NPC
{
	private bool isGrounded = false;
	private int curTargetPos;

	private EventInstance bosstheme;
	public bool playing;

	const string SlimeBounce = "Slime_bounce";
	const string SlimeIdle = "Slime_idle";

	public override void SetDefaults()
	{
		base.SetDefaults();

		NPCName = "GreenSlime";
		Damage = 5;
		LifeMax = 20;
		Life = LifeMax;

		target = GameObject.Find("Player");
		if (!GameObject.Find("WorldManager").GetComponent<BiomeDetection>().bossAlive)
		{
			bosstheme = AudioManager.instance.CreateEventInstance(FMODEvents.instance.SCal2);
			bosstheme.start();
			playing = true;
		}
	}

	public override void AI()
	{
		if (target != null)
		{
			GameObject.Find("WorldManager").GetComponent<BiomeDetection>().bossAlive = true;
			playing = true;
			
			animator.speed = 0.8f;

			ai[1]++;
			if (ai[1] == 90.0f && isGrounded)
			{
				ChangeAnimationState(SlimeIdle);

				//Jump
				rb.velocity = new Vector2(TargetDirection * 5, 5);
			}
			else if (ai[1] > 150.0f && isGrounded) 
			{
				ChangeAnimationState(SlimeBounce);
				rb.velocity = Vector2.zero; 
				ai[1] = 0.0f;
			}
			if (DistanceBetween(transform.position, target.transform.position) > 60f)
			{
				Destroy(gameObject);
			}
		}
		
	}
	void OnDestroy()
	{
		bosstheme.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		GameObject.Find("WorldManager").GetComponent<BiomeDetection>().bossAlive = false;
		playing = false;
	}
	private void FixedUpdate()
	{
		float extraHeight = 0.4f; 
		Color rayColor; 

		RaycastHit2D hit = Physics2D.Raycast(c2d.bounds.center, Vector2.down, c2d.bounds.extents.y + extraHeight, groundLayer); 

		//Debug.Log(hit.collider);
		if (hit.collider != null) 
		{
			isGrounded = true; 
			rayColor = Color.green; 
		}
		else
		{
			isGrounded = false; 
			rayColor = Color.red; 
		}
		Debug.DrawRay(c2d.bounds.center, Vector2.down * (c2d.bounds.extents.y + extraHeight), rayColor); 
	}
}
