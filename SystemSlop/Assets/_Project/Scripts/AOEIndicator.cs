using UnityEngine;

public class AOEIndicator : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;

    private float _radius;

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
        transform.position = position + Vector3.up * 0.05f;
    }


}
