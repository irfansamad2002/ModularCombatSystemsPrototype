using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private List<EffectData> _effects;
    private float _speed;

    public void Init(List<EffectData> effects, float speed)
    {
        _effects = effects;
        _speed = speed;
    }

    private void Update()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        var health = other.GetComponent<Health>();
        if (health== null) return;
        
        foreach (var effect in _effects)
        {
            effect.Apply(other.gameObject);
        }

        Destroy(gameObject);

    }

}
