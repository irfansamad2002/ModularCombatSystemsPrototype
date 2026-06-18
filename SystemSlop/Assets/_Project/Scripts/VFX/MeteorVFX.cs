using UnityEngine;

public class MeteorVFX : MonoBehaviour
{
    private Vector3 _startPos;
    private Vector3 _endPos;
    private float _duration;
    private float _timer;

    private bool _initialized;

    public void Init(Vector3 start, Vector3 end, float duration)
    {
        _startPos = start;
        _endPos = end;
        _duration = duration;
        _timer = 0;
        _initialized = true;
    }

    private void Update()
    {
        if (!_initialized) return;

        _timer += Time.deltaTime;

        float t = Mathf.Clamp01(_timer / _duration);

        transform.position = Vector3.Lerp(_startPos, _endPos, t);

        if (t >= 1)
        {
            ImpactVFX();
        }
    }

    private void ImpactVFX()
    {
        Destroy(gameObject);
    }

}
