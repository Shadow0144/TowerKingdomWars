using System;
using System.Collections.Generic;
using Unity.Multiplayer.PlayMode;
using UnityEngine;

namespace TowerKingdomWars
{
    public abstract class Structure : MonoBehaviour
    {
        private bool _isGhost = false;
        public bool IsGhost
        {
            get
            {
                return _isGhost;
            }

            set
            {
                _isGhost = value;
                SetUpGhost();
            }
        }

        public bool CanBeDeconstructed = false;

        public PlayerController.PlayerInfo OwningPlayerInfo { get; set; }

        public uint PlayerSlot { get; private set; }

        public List<Tile> CurrentTiles { get; private set; }

        private static List<Vector2Int> _footprint = new List<Vector2Int>()
    {
        new Vector2Int(+0, +0)
    };
        public static List<Vector2Int> Footprint => _footprint; // row, col
        public abstract List<Vector2Int> GetFootprint();

        public virtual void Initialize(uint playerSlot, List<Tile> tiles)
        {
            PlayerSlot = playerSlot;
            CurrentTiles = tiles;
        }

        protected virtual void SetUpGhost()
        {
            if (_isGhost)
            {
                Renderer renderer = GetComponent<Renderer>();
                Color color = renderer.material.color;
                color.a = _isGhost ? 0.5f : 1.0f;
                renderer.material.color = color;
            }
        }
    }
}