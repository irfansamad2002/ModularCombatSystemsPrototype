using UnityEngine;

namespace Project.VFX
{
public class EnemyVisualFeedback : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer meshRenderer;

        private Material _material;
        private Color _baseColor;

        private void Awake()
        {
            if (meshRenderer == null)
            {
                Debug.LogError("MeshRenderer not assigned");
                return;
            }

            _material = meshRenderer.material;
            _baseColor = _material.GetColor("_BaseColor");
        }

        public void SetSlowed(bool isSlowed)
        {
            if (_material == null) return;

            if (isSlowed)
            {
                _material.SetColor("_BaseColor", new Color(.1f, .1f, 1f));
            }
            else
            {
                _material.SetColor("_BaseColor", _baseColor);
            }
        }
    }
}