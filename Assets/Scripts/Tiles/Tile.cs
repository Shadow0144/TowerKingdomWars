using UnityEditor.Rendering;
using UnityEngine;

namespace TowerKingdomWars
{
    public class Tile : MonoBehaviour
    {
        public uint Row { get; private set; }
        public uint Column { get; private set; }

        protected Material _material;
        protected const float _HIGHLIGHT_DECAY = 0.1f;
        [Range(0f, 1f)] private float _currentHighlight;

        public bool ContainsStructure = false; // Paths should count as a structure
        Structure _containedStructure = null;
        public Structure ContainedStructure
        {
            get { return _containedStructure; }
            set
            {
                _containedStructure = value;
                ContainsStructure = (_containedStructure != null);
            }
        }

        public virtual void Initialize(uint row, uint column)
        {
            Row = row;
            Column = column;

            _currentHighlight = 0.0f;
            _material = GetComponent<Renderer>().material;
            _material.SetFloat("_Blend", _currentHighlight);
        }

        private void Update()
        {
            _currentHighlight = Mathf.Clamp01(_currentHighlight - _HIGHLIGHT_DECAY);
            SetBlend(_currentHighlight);
        }

        public virtual void Highlight(bool valid)
        {
            _material.SetFloat("_Valid", valid ? 1.0f : 0.0f);
            _currentHighlight = 1.0f;
            SetBlend(_currentHighlight);
        }

        public void SetBlend(float blend)
        {
            _material.SetFloat("_Blend", blend);
        }
    }
}