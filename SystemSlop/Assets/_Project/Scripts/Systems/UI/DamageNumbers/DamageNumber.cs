using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private float _lifetime = 1.5f;
    private float _timer;

    private Vector3 _moveDir = Vector3.up;
    private float _moveSpeed = 0.5f;

    public void Setup(float damage)
    {
        text.text = Mathf.RoundToInt(damage).ToString();
        Vector3 randomDir = new Vector3(
            Random.Range(-0.3f, 0.3f),
            1f,
            Random.Range(-0.3f, 0.3f)
        ).normalized;

        _moveDir = randomDir;

    }

    private void Update()
    {
        _timer += Time.deltaTime;

        transform.position += _moveDir * _moveSpeed * Time.deltaTime;

        float t = _timer / _lifetime;

        Color c = text.color;
        c.a = Mathf.Lerp(1f, 0f, t);
        text.color = c;

        if(_timer >= _lifetime)
        {
            Destroy(gameObject);
        }

    }

    private void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
