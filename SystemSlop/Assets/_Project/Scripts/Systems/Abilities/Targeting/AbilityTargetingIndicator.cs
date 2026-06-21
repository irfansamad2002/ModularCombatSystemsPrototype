using UnityEngine;

namespace Project.Systems.VFX
{
    public class AbilityTargetingIndicator : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;

        private Color _validColor = new Color(0, 1, 0, .3f);
        private Color _invalidColor = new Color(1, 0, 0, .3f);

        private Vector3 _targetPosition;
        private bool _isValid;

        public void Initialize(float radius)
        {
            transform.localScale = Vector3.one * radius * 2f;
            //Debug.Log("AOE indicator" + transform.localScale);
        }

        public void SetPosition(Vector3 position)
        {
            _targetPosition = position;
        }

        public void SetValid(bool isValid)
        {
            _isValid = isValid;
            _renderer.material.color = isValid ? _validColor : _invalidColor;
        }

        private void Update()
        {
            transform.position = _targetPosition;
        }
        public void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}