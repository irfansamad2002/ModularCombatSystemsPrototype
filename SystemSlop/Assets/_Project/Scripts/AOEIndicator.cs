using UnityEngine;

public class AOEIndicator : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;

    private Color _validColor = new Color(0, 1, 0, .3f);
    private Color _invalidColor = new Color(1, 0, 0, .3f);
    private float _radius;
    private Vector3 _targetPosition;

    public void SetValid(bool isValid)
    {
        _renderer.material.color = isValid ? _validColor : _invalidColor;
    }

    public void Init(float radius)
    {
        _radius = radius;

        //scale to match explosion EXACTLY
        transform.localScale = new Vector3(radius * 2f, 0.1f, radius * 2f);
    }

    public Vector3 GetCurrentPosition()
    {
        return transform.position;
    }

    public void SetPosition(Vector3 position)
    {
        _targetPosition = position;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(
        transform.position,
        _targetPosition,
        Time.deltaTime * 15f);
    }

}
