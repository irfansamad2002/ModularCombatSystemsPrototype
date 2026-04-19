using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private List<EffectData> _effects;
    private float _speed;
    private float _explosionRadius;
    private LayerMask _targetLayers;
    private Collider _ownerCollider;
    private bool _hasHit;
    private GameObject _impactVFX;

    public void SetOwner(Collider owner)
    {
        _ownerCollider = owner; 
        var myCollider = GetComponent<Collider>();
        if(myCollider != null && _ownerCollider != null)
        {
            Physics.IgnoreCollision(myCollider, _ownerCollider);
        }   
    }

    public void Init(List<EffectData> effects, float speed, float radius, LayerMask layers, GameObject impactVFX)
    {
        _effects = effects;
        _speed = speed;
        _explosionRadius = radius;
        _targetLayers = layers;
        _impactVFX = impactVFX;
    }

    private void Update()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_hasHit) return; // prevent multiple hits
        _hasHit = true;
        if (_effects == null) return;

        if (_explosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            // single target still needs filtering
            if (((1 << other.gameObject.layer) & _targetLayers) != 0)
            {
                ApplyEffects(other.gameObject);
            }
        }

        Destroy(gameObject);
    }
    private void Explode()
    {
        SpawnImpactVFX();

        // draw debug sphere at impact point
        DebugDrawSphere(transform.position, _explosionRadius, Color.yellow, 2f);

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _explosionRadius, _targetLayers);

        foreach (Collider hit in hitColliders)
        {
            ApplyEffects(hit.gameObject);
        }
    }

    private void ApplyEffects(GameObject target)
    {
        var health = target.GetComponent<Health>();
        if (health == null) return;

        foreach (var effect in _effects)
        {
            effect.Apply(target);
        }
        
    }

    void DebugDrawSphere(Vector3 center, float radius, Color color, float duration)
    {
        int segments = 20;
        float angleStep = 360f / segments;

        for (int i = 0; i < segments; i++)
        {
            float angle1 = Mathf.Deg2Rad * (i * angleStep);
            float angle2 = Mathf.Deg2Rad * ((i + 1) * angleStep);

            Vector3 p1 = center + new Vector3(Mathf.Cos(angle1), 0, Mathf.Sin(angle1)) * radius;
            Vector3 p2 = center + new Vector3(Mathf.Cos(angle2), 0, Mathf.Sin(angle2)) * radius;

            Debug.DrawLine(p1, p2, color, duration);
        }
    }

    private void SpawnImpactVFX()
    {
        if(_impactVFX == null) return;

        var vfx = Instantiate(_impactVFX, transform.position, Quaternion.identity);

        //scale to match explosion raidus
        float diameter = _explosionRadius * 2f;
        vfx.transform.localScale = Vector3.one * diameter;

        Destroy(vfx, 2f);// Cleanup
    }
}
