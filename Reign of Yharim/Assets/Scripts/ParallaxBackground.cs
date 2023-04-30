using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Transform background;
    [SerializeField] private float parallaxEffect;
    [SerializeField] private float smoothing = 1f;

    private Transform cameraTransform;
    private Vector3 previousCameraPosition;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        previousCameraPosition = cameraTransform.position;
    }

    private void LateUpdate() //on lateupdate
    {
            float parallax = (previousCameraPosition.x - cameraTransform.position.x) * parallaxEffect; //parallax variable is the x of previousCameraPosition subtracted by the transform of the main camera. this is then multiplied by the paralaxEffect variable.
            float backgroundTargetPosX = background.position.x - parallax; //backgroundtargetPosX is the x position of the background object, subtracted by the parallax variable.
            Vector2 backgroundTargetPos = new Vector3(backgroundTargetPosX, background.position.y); //the backgroundTargetPos is a vector2 with backgroundTargetPosX for x and the y position of the background object for y)
            background.position = Vector3.Lerp(background.position, backgroundTargetPos, smoothing * Time.deltaTime); //The position of the background object equals the point smoothing * time.deltatime of points a (background.position) and b (backgroundTargetPos)

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
}
