using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HUD : MonoBehaviour
{
    [SerializeField] private Player _player;

    private UIDocument _document;

    private RadioButtonGroup _towerButtonGroup;
    private RadioButton _arrowTowerButton;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();

        _towerButtonGroup = _document.rootVisualElement.Q("TowerRadioButtonGroup") as RadioButtonGroup;

        _arrowTowerButton = _document.rootVisualElement.Q("ArrowTowerButton") as RadioButton;
        _arrowTowerButton.RegisterCallback<ClickEvent>(OnArrowTowerButtonClicked);
    }

    private void OnDisable()
    {
        _arrowTowerButton.UnregisterCallback<ClickEvent>(OnArrowTowerButtonClicked);
    }

    private void OnArrowTowerButtonClicked(ClickEvent clickEvent)
    {
        _player.SelectArrowTower();
    }

    public void DeselectTower()
    {
        _towerButtonGroup.value = -1;
    }
}
