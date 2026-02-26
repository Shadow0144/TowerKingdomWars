using System.Collections.Generic;
using UnityEngine;

namespace TowerKingdomWars
{
    public class MonsterFactory : MonoBehaviour
    {
        [SerializeField] private Goblin _goblinPrefab;

        private static MonsterFactory _instance;

        private void Awake()
        {
            _instance = this;
        }

        public static Goblin SpawnGoblin(Vector3 startingPos, PlayerController.PlayerInfo playerInfo, Path path)
        {
            if (_instance == null)
            {
                return null;
            }

            Goblin goblin = Instantiate(_instance._goblinPrefab, startingPos, Quaternion.identity);
            goblin.Initialize(playerInfo, path);
            goblin.transform.SetParent(GameSceneController.Instance.Map.Monsters.transform);
            return goblin;
        }
    }
}