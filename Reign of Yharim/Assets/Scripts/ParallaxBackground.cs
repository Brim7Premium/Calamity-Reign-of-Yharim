using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Transform[] backgrounds;
    [SerializeField] private float[] parallaxScales;
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
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float parallax = (previousCameraPosition.x - cameraTransform.position.x) * parallaxScales[i];
            float backgroundTargetPosX = backgrounds[i].position.x - parallax;
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);

            if (backgrounds[i].position.x < cameraTransform.position.x - (backgrounds[i].GetComponent<SpriteRenderer>().bounds.size.x / 2))
            {
                float offsetPositionX = (backgrounds[i].GetComponent<SpriteRenderer>().bounds.size.x - 0.01f);
                backgrounds[i].position = new Vector3(backgrounds[i].position.x + offsetPositionX, backgrounds[i].position.y, backgrounds[i].position.z);
            }
            else if (backgrounds[i].position.x > cameraTransform.position.x + (backgrounds[i].GetComponent<SpriteRenderer>().bounds.size.x / 2))
            {
                float offsetPositionX = (backgrounds[i].GetComponent<SpriteRenderer>().bounds.size.x - 0.01f);
                backgrounds[i].position = new Vector3(backgrounds[i].position.x - offsetPositionX, backgrounds[i].position.y, backgrounds[i].position.z);
            }
        }

        previousCameraPosition = cameraTransform.position;
    }
}
