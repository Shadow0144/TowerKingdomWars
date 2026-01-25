using UnityEngine;

public class TileScript : MonoBehaviour
{
    private Material material;
    private const float HighlightDecay = 0.1f;
    [Range(0f, 1f)] private float currentHighlight;

    void Start()
    {
        currentHighlight = 0.0f;
        material = GetComponent<Renderer>().material;
        material.SetFloat("_Blend", currentHighlight);
    }

    void Update()
    {
        currentHighlight = Mathf.Clamp01(currentHighlight - HighlightDecay);
        SetBlend(currentHighlight);
    }

    public void Highlight()
    {
        currentHighlight = 1.0f;
        SetBlend(currentHighlight);
    }
    public void SetBlend(float blend)
    {
        material.SetFloat("_Blend", blend);
    }
}
