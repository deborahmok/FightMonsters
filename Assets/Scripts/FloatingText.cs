using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float lifeTime = 1f;

    private TMP_Text tmp;

    void Awake()
    {
        // Works for TextMeshPro (3D) and TextMeshProUGUI
        tmp = GetComponent<TMP_Text>();
        if (tmp == null) tmp = GetComponentInChildren<TMP_Text>();
    }

    public void SetText(string text)
    {
        if (tmp == null)
        {
            Debug.LogError("FloatingText: No TMP_Text found on prefab or its children!");
            return;
        }

        tmp.text = text;
    }

    void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
            Destroy(gameObject);
    }
}