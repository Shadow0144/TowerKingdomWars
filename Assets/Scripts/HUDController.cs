using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HUDController : MonoBehaviour
{
    private UIDocument _document;

    private RadioButtonGroup _towerButtonGroup;
    private RadioButton _arrowTowerButton;
    private RadioButton _flameTowerButton;
    private RadioButton _frostTowerButton;
    private RadioButton _removeTowerButton;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();

        _towerButtonGroup = _document.rootVisualElement.Q("TowerRadioButtonGroup") as RadioButtonGroup;

        _arrowTowerButton = _document.rootVisualElement.Q("ArrowTowerButton") as RadioButton;
        _arrowTowerButton.RegisterCallback<ClickEvent>(OnArrowTowerButtonClicked);

        _flameTowerButton = _document.rootVisualElement.Q("FlameTowerButton") as RadioButton;
        _flameTowerButton.RegisterCallback<ClickEvent>(OnFlameTowerButtonClicked);

        _frostTowerButton = _document.rootVisualElement.Q("FrostTowerButton") as RadioButton;
        _frostTowerButton.RegisterCallback<ClickEvent>(OnFrostTowerButtonClicked);

        _removeTowerButton = _document.rootVisualElement.Q("RemoveTowerButton") as RadioButton;
        _removeTowerButton.RegisterCallback<ClickEvent>(OnRemoveTowerButtonClicked);
    }

    private void OnDisable()
    {
        _arrowTowerButton.UnregisterCallback<ClickEvent>(OnArrowTowerButtonClicked);
        _flameTowerButton.UnregisterCallback<ClickEvent>(OnFlameTowerButtonClicked);
        _frostTowerButton.UnregisterCallback<ClickEvent>(OnFrostTowerButtonClicked);
        _removeTowerButton.UnregisterCallback<ClickEvent>(OnRemoveTowerButtonClicked);
    }

    private void OnArrowTowerButtonClicked(ClickEvent clickEvent)
    {
        GameSceneController.Instance.LocalPlayer.SelectArrowTower();
    }

    private void OnFlameTowerButtonClicked(ClickEvent clickEvent)
    {
        GameSceneController.Instance.LocalPlayer.SelectFlameTower();
    }

    private void OnFrostTowerButtonClicked(ClickEvent clickEvent)
    {
        GameSceneController.Instance.LocalPlayer.SelectFrostTower();
    }

    private void OnRemoveTowerButtonClicked(ClickEvent clickEvent)
    {
        GameSceneController.Instance.LocalPlayer.SelectRemoveTower();
    }

    public void DeselectTower()
    {
        _towerButtonGroup.value = -1;
        GameSceneController.Instance.LocalPlayer.SelectNoTower();
    }
}
