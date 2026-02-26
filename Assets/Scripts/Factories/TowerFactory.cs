using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace TowerKingdomWars
{
    public class TowerFactory : MonoBehaviour
    {
        [SerializeField] private ArrowTower _arrowTowerPrefab;
        [SerializeField] private FlameTower _flameTowerPrefab;
        [SerializeField] private FrostTower _frostTowerPrefab;

        private static TowerFactory _instance;

        private void Awake()
        {
            _instance = this;
        }

        public static ArrowTower getArrowTowerGhost(Vector3 position)
        {
            if (_instance == null)
            {
                return null;
            }

            ArrowTower arrowTower = Instantiate(_instance._arrowTowerPrefab, position, Quaternion.identity);
            arrowTower.IsGhost = true;
            arrowTower.gameObject.transform.SetParent(GameSceneController.Instance.LocalPlayer.transform);
            return arrowTower;
        }

        public static ArrowTower SpawnArrowTower(uint playerSlot, Vector3 position, List<Tile> tiles)
        {
            if (_instance == null)
            {
                return null;
            }

            ArrowTower arrowTower = Instantiate(_instance._arrowTowerPrefab, position, Quaternion.identity);
            arrowTower.Initialize(playerSlot, tiles);
            arrowTower.gameObject.transform.SetParent(GameSceneController.Instance.Map.Towers.transform);
            return arrowTower;
        }

        public static FlameTower getFlameTowerGhost(Vector3 position)
        {
            if (_instance == null)
            {
                return null;
            }

            FlameTower flameTower = Instantiate(_instance._flameTowerPrefab, position, Quaternion.identity);
            flameTower.IsGhost = true;
            flameTower.gameObject.transform.SetParent(GameSceneController.Instance.LocalPlayer.transform);
            return flameTower;
        }

        public static FlameTower SpawnFlameTower(uint playerSlot, Vector3 position, List<Tile> tiles)
        {
            if (_instance == null)
            {
                return null;
            }

            FlameTower flameTower = Instantiate(_instance._flameTowerPrefab, position, Quaternion.identity);
            flameTower.Initialize(playerSlot, tiles);
            flameTower.gameObject.transform.SetParent(GameSceneController.Instance.Map.Towers.transform);
            return flameTower;
        }

        public static FrostTower getFrostTowerGhost(Vector3 position)
        {
            if (_instance == null)
            {
                return null;
            }

            FrostTower frostTower = Instantiate(_instance._frostTowerPrefab, position, Quaternion.identity);
            frostTower.IsGhost = true;
            frostTower.gameObject.transform.SetParent(GameSceneController.Instance.LocalPlayer.transform);
            return frostTower;
        }

        public static FrostTower SpawnFrostTower(uint playerSlot, Vector3 position, List<Tile> tiles)
        {
            if (_instance == null)
            {
                return null;
            }

            FrostTower frostTower = Instantiate(_instance._frostTowerPrefab, position, Quaternion.identity);
            frostTower.Initialize(playerSlot, tiles);
            frostTower.gameObject.transform.SetParent(GameSceneController.Instance.Map.Towers.transform);
            return frostTower;
        }
    }
}