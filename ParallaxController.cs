using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    [SerializeField] bool infiniteHorizontal;
    [SerializeField] bool infiniteVertical;
    [SerializeField] Vector2 parallaxEffectMultiplier;


    [SerializeField] Transform cameraTransform;
    Vector3 cameraLastPosition;

    float textureUnitySizeX;
    float textureUnitySizeY;

    void Start()
    {
        cameraLastPosition = cameraTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitySizeX = texture.width / sprite.pixelsPerUnit;
        textureUnitySizeY = texture.height / sprite.pixelsPerUnit;
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - cameraLastPosition;
        transform.position += new Vector3
            (deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y);
        cameraLastPosition = cameraTransform.position;

        //  !! EDITED !!
        if (infiniteHorizontal)
        {
            if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitySizeX)
            {
                //  % is to see remainder after 15 
                //Debug.Log(cameraTransform.position.x - transform.position.x);
                float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitySizeX;
                transform.position = new Vector3(cameraTransform.position.x - offsetPositionX, transform.position.y);
                //Debug.Log(cameraTransform.position.x - transform.position.x);
            }
        }
        if (infiniteVertical)
        {
            if (Mathf.Abs(cameraTransform.position.y - transform.position.y) >= textureUnitySizeY)
            {
                float offsetPositionY = (cameraTransform.position.y - transform.position.y) % textureUnitySizeY;
                transform.position = new Vector3(transform.position.x, cameraTransform.position.y - offsetPositionY);
            }
        }
        
    }
}
