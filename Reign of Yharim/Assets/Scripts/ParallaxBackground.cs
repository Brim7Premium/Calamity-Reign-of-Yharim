using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Vector2 parallaxEffectMultiplier;
    [SerializeField] private bool infiniteLooping;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    private float textureUnitSizeX;
    private float textureOffsetX;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;

        if (infiniteLooping)
        {
            textureOffsetX = transform.position.x;
        }
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y);
        lastCameraPosition = cameraTransform.position;

        if (infiniteLooping)
        {
            if (Mathf.Abs(cameraTransform.position.x - textureOffsetX) >= textureUnitSizeX)
            {
                float offsetPositionX = (cameraTransform.position.x - textureOffsetX) % textureUnitSizeX;
                textureOffsetX = cameraTransform.position.x - offsetPositionX;
                transform.position = new Vector3(textureOffsetX, transform.position.y, transform.position.z);
            }
        }
    }
}
