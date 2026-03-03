using UnityEngine;

public class PulseGlow : MonoBehaviour
{
    public float speed = 2f;
    public float minScale = 1.4f;
    public float maxScale = 1.7f;

    private Vector3 baseScale;

    void Start()
    {
        baseScale = transform.localScale;
    }

    void Update()
    {
        float scale = Mathf.Lerp(minScale, maxScale,
            (Mathf.Sin(Time.time * speed) + 1f) / 2f);

        transform.localScale = baseScale.normalized * scale;
    }
}