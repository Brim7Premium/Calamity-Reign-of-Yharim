using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
	[SerializeField] private Transform background; //the transform of the attached background object
	[SerializeField] private float parallaxEffectX, parallaxEffectY, smoothing = 1f;

	private GameObject player;
	private Transform cameraTransform; //the transform of the main camera
	private Vector3 previousCameraPosition; //the previous camera position
	private string scenename;

	private void FixedUpdate()
	{
		if (player == null)
		{
			player = GameObject.Find("Player");
		}

		else
		{
			cameraTransform = Camera.main.transform;
			previousCameraPosition = cameraTransform.position;
			scenename = this.gameObject.scene.name;
			int index = scenename.IndexOf("_");
			if (index >= 0)
			{
				scenename = scenename.Substring(0, index);
			}
			if (BiomeManager.instance.biomeName == scenename)
			{
				if (this.transform.GetChild(0).GetChild(0).gameObject.GetComponent<SpriteRenderer>().color.a < 1f)
				{
					this.transform.GetChild(0).GetChild(0).gameObject.GetComponent<SpriteRenderer>().color += new Color(0f, 0f, 0f, .01f);
				}
				float parallaxX = (previousCameraPosition.x - cameraTransform.position.x) * parallaxEffectX; //parallax variable is the x of previousCameraPosition subtracted by the transform of the main camera. this is then multiplied by the paralaxEffect variable.
				float backgroundTargetPosX = background.position.x - parallaxX; //backgroundtargetPosX is the x position of the background object, subtracted by the parallax variable.
        		float parallaxY = (previousCameraPosition.y - cameraTransform.position.y) * parallaxEffectY;
        		float backgroundTargetPosY = background.position.y - parallaxY;
				Vector2 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgroundTargetPosY); //the backgroundTargetPos is a vector2 with backgroundTargetPosX for x and the y position of the background object for y)
				background.position = Vector3.Lerp(background.position, backgroundTargetPos, smoothing * Time.deltaTime); //The position of the background object equals the point (smoothing * time.deltatime) of points a (background.position) and b (backgroundTargetPos)

				if (background.position.x < cameraTransform.position.x - (background.GetComponent<SpriteRenderer>().bounds.size.x / 2))
				{
					float offsetPositionX = (background.GetComponent<SpriteRenderer>().bounds.size.x - 0.01f);
					background.position = new Vector3(background.position.x + offsetPositionX, background.position.y, background.position.z);
				}
				else if (background.position.x > cameraTransform.position.x + (background.GetComponent<SpriteRenderer>().bounds.size.x / 2))
				{
					float offsetPositionX = (background.GetComponent<SpriteRenderer>().bounds.size.x - 0.01f);
					background.position = new Vector3(background.position.x - offsetPositionX, background.position.y, background.position.z);
				}

				previousCameraPosition = cameraTransform.position;
			}

			else
			{
				if (this.transform.GetChild(0).GetChild(0).gameObject.GetComponent<SpriteRenderer>().color.a > 0f)
				{
					this.transform.GetChild(0).GetChild(0).gameObject.GetComponent<SpriteRenderer>().color -= new Color(0f, 0f, 0f, .01f);
				}
			}
		}
	}
}
