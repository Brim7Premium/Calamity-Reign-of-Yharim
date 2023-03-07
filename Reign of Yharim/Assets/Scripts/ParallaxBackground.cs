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

    private void LateUpdate()
    {
            float parallax = (previousCameraPosition.x - cameraTransform.position.x) * parallaxEffect;
            float backgroundTargetPosX = background.position.x - parallax;
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, background.position.y, background.position.z);
            background.position = Vector3.Lerp(background.position, backgroundTargetPos, smoothing * Time.deltaTime);

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
