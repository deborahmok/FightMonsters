using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraFollowZoom2D : MonoBehaviour
{
    [Header("Follow")]
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 8f;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);

    [Header("Zoom (Orthographic)")]
    [SerializeField] private PlayerLight playerLightScript; // your script, not the Light2D component
    [SerializeField] private float padding = 2f;            // your "radius + 2"
    [SerializeField] private float minOrthoSize = 3f;       // prevents zooming in too far
    [SerializeField] private float maxOrthoSize = 12f;      // prevents zooming out to whole map
    [SerializeField] private float zoomLerpSpeed = 8f;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        if (cam == null) cam = Camera.main;
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desired = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desired, smoothSpeed * Time.deltaTime);
        }

        if (cam != null && playerLightScript != null)
        {
            // PlayerLight should expose CurrentRadius based on the Light2D radius
            float desiredSize = playerLightScript.CurrentRadius + padding;
            desiredSize = Mathf.Clamp(desiredSize, minOrthoSize, maxOrthoSize);

            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, desiredSize, zoomLerpSpeed * Time.deltaTime);
        }
    }
}