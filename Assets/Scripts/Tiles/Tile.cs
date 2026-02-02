using UnityEngine;

public class Tile : MonoBehaviour
{
    public Map map { get; set; }

    protected Material material;
    protected const float HighlightDecay = 0.1f;
    [Range(0f, 1f)] private float currentHighlight;

    protected Tower tower;

    public void Initialize(string name, Map map)
    {
        this.name = name;
        this.map = map;
    }

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

    public virtual void Click()
    {
        if (tower == null)
        {
            tower = TowerFactory.SpawnArrowTower(Player.PlayerNumber.One, this);
        }
        else
        {
            Destroy(tower.gameObject);
            tower = null;
        }
    }
}
