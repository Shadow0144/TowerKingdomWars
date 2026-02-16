using UnityEngine;

public class Tile : MonoBehaviour
{
    protected Material _material;
    protected const float _HighlightDecay = 0.1f;
    [Range(0f, 1f)] private float _currentHighlight;

    protected Tower _tower;

    public void Initialize(string name)
    {
        this.name = name;

        _currentHighlight = 0.0f;
        _material = GetComponent<Renderer>().material;
        _material.SetFloat("_Blend", _currentHighlight);
    }

    private void Update()
    {
        _currentHighlight = Mathf.Clamp01(_currentHighlight - _HighlightDecay);
        SetBlend(_currentHighlight);
    }

    public virtual void Highlight()
    {
        _currentHighlight = 1.0f;
        SetBlend(_currentHighlight);
    }

    public void SetBlend(float blend)
    {
        _material.SetFloat("_Blend", blend);
    }

    public virtual void Click()
    {
        if (_tower == null)
        {
            switch (GameSceneController.Instance.LocalPlayer.CurrentTowerSelected)
            {
                case PlayerController.TowerSelected.None:
                    {
                        // Do nothing
                    }
                    break;
                case PlayerController.TowerSelected.ArrowTower:
                    {
                        _tower = TowerFactory.SpawnArrowTower(GameSceneController.Instance.LocalPlayer.CurrentPlayerNumber, this);
                    }
                    break;
                case PlayerController.TowerSelected.FlameTower:
                    {
                        _tower = TowerFactory.SpawnFlameTower(GameSceneController.Instance.LocalPlayer.CurrentPlayerNumber, this);
                    }
                    break;
                case PlayerController.TowerSelected.FrostTower:
                    {
                        _tower = TowerFactory.SpawnFrostTower(GameSceneController.Instance.LocalPlayer.CurrentPlayerNumber, this);
                    }
                    break;
            }
            GameSceneController.Instance.DeselectTower();
        }
        else // _tower != null
        {
            if (GameSceneController.Instance.LocalPlayer.CurrentTowerSelected == PlayerController.TowerSelected.RemoveTower)
            {
                Destroy(_tower.gameObject);
                _tower = null;
                GameSceneController.Instance.DeselectTower();
            }
        }
    }
}
