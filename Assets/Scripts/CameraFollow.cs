using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow")]
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 5f;

    [Header("Zoom (Orthographic)")]
    [SerializeField] private PlayerLight playerLight; // drag Player (the one with PlayerLight)
    [SerializeField] private float padding = 0f;      // torch radius + x
    [SerializeField] private float minSize = 4f;
    [SerializeField] private float maxSize = 25f;
    [SerializeField] private float zoomSmooth = 8f;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // FOLLOW
        Vector3 desiredPos = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);

        // ZOOM
        if (cam != null && playerLight != null)
        {
            float desiredSize = Mathf.Clamp(playerLight.CurrentRadius + padding, minSize, maxSize);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, desiredSize, zoomSmooth * Time.deltaTime);
        }
    }
}