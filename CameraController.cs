using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //  why does it slow down when it gets near the edge?
    //      it lerps to the clamped position, so even if the player goes past the clamped position
    //      it will really just be moving towards the final clamped value it can go to

    [SerializeField] float smoothFactor = 2f;
    [SerializeField] float minY;
    [SerializeField] float maxY;

    [SerializeField] Transform targetTransform;
    [SerializeField] BoxCollider2D boundingCollider;

    [SerializeField] private Vector2 scaleBound = new Vector2(1, 1);
    [SerializeField] private Vector2 offset = new Vector2(0, 0f);

    private float cameraWiggleRoomY;
    private float cameraWiggleRoomX;

    private Vector2 startingPosition;

    private void Start()
    {
        transform.position = boundingCollider.bounds.center;
        startingPosition = transform.position;

        var cameraHeightExtent = Camera.main.orthographicSize;
        var cameraWidthExtent = Camera.main.orthographicSize * Camera.main.pixelWidth / Camera.main.pixelHeight;

        var boundingColliderExtentY = boundingCollider.bounds.extents.y;
        var boundingColliderExtentX = boundingCollider.bounds.extents.x;

        //  size is double the extents
        if (cameraHeightExtent > boundingColliderExtentY)
        {
            Camera.main.orthographicSize = boundingColliderExtentY;

            //boundingCollider.size = new Vector2(boundingCollider.size.x, cameraHeightExtent * 2);
            //boundingColliderExtentY = boundingCollider.bounds.extents.y;
        }

        if (cameraWidthExtent > boundingColliderExtentX)
        {
            Camera.main.orthographicSize = boundingCollider.size.x * Screen.height / Screen.width * 0.5f;
            
            cameraHeightExtent = Camera.main.orthographicSize;
            cameraWidthExtent = Camera.main.orthographicSize * Camera.main.pixelWidth / Camera.main.pixelHeight;

            //boundingCollider.size = new Vector2(cameraWidthExtent * 2, boundingCollider.size.y);
            //boundingColliderExtentX = boundingCollider.bounds.extents.x;
        }

        cameraWiggleRoomY = (boundingColliderExtentY - cameraHeightExtent) * scaleBound.y;
        cameraWiggleRoomX = (boundingColliderExtentX - cameraWidthExtent) * scaleBound.x;
        offset *= scaleBound;

    }

    void FixedUpdate()
    {
        if (targetTransform)
        {
            float clampedY = Mathf.Clamp(targetTransform.position.y, -cameraWiggleRoomY + startingPosition.y + offset.y
                        , cameraWiggleRoomY + startingPosition.y + offset.y);
            float clampedX = Mathf.Clamp(targetTransform.position.x, -cameraWiggleRoomX + startingPosition.x + offset.x
                , cameraWiggleRoomX + startingPosition.x + offset.x);
            var targetPos = new Vector3(clampedX, clampedY, transform.position.z);
            var lerpedPosition = Vector3.Lerp(transform.position, targetPos, smoothFactor * Time.fixedDeltaTime);
            transform.position = lerpedPosition;
        }
    }

    /* Doesnt work correctly on the x axis for some reason
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(startingPosition, new Vector3(startingPosition.x, -cameraWiggleRoomY));
        Gizmos.DrawLine(startingPosition, new Vector3(startingPosition.x, cameraWiggleRoomY));
        Gizmos.DrawLine(startingPosition, new Vector3(-cameraWiggleRoomX, startingPosition.y));
        Gizmos.DrawLine(startingPosition, new Vector3(cameraWiggleRoomX, startingPosition.y));
    }
    */
}
