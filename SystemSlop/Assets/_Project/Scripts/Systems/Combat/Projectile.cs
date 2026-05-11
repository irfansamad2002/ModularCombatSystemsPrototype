using Project.Core.Health;
using Project.Systems.Effects;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Systems.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Material transparentMaterial;
        private List<EffectData> _effects;
        private float _speed;
        private float _explosionRadius;
        private LayerMask _damageLayers;
        private GameObject _impactVFX;
        private float _minDistanceThreshold;
        private float _minFalloff;

        private bool _hasHit;



        public void Init(List<EffectData> effects, float speed, float radius, LayerMask damageLayers, GameObject impactVFX, float minDistanceThreshold, float minFalloff)
        {
            _effects = effects;
            _speed = speed;
            _explosionRadius = radius;
            _damageLayers = damageLayers;
            _minDistanceThreshold = minDistanceThreshold;
            _minFalloff = minFalloff;
            _impactVFX = impactVFX;
        }

        private void Update()
        {
            transform.position += transform.forward * _speed * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_hasHit) return; // prevent multiple hits
            _hasHit = true;

            Explode();


            SpawnDebugSphere(transform.position, _explosionRadius);
            Destroy(gameObject);

        }
        private void Explode()
        {
            Vector3 explosionCenter = transform.position;

            SpawnImpactVFX(explosionCenter);

            Collider[] hitColliders = Physics.OverlapSphere(explosionCenter, _explosionRadius, _damageLayers);

            foreach (var hitCollider in hitColliders)
            {

                ApplyAOE(hitCollider, explosionCenter);
            }
        }

        private void ApplyAOE(Collider hitCollider, Vector3 explosionCenter)
        {
          
            float distance = Vector3.Distance(explosionCenter, hitCollider.ClosestPoint(explosionCenter));

            if (distance <= _minDistanceThreshold)
            {
                distance = 0f; // treat as direct hit
            }

            float normalized = distance / _explosionRadius;
            normalized = Mathf.Clamp01(normalized);

            float falloff = Mathf.Pow(1f - normalized, .5f); // quadratic falloff

            falloff = Mathf.Max(falloff, _minFalloff); // ensure minimum effect

            foreach (var effect in _effects)
            {
                effect.Apply(hitCollider.gameObject,default, falloff);
            }
                
        }


        private void SpawnDebugSphere(Vector3 position, float radius)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            sphere.transform.position = position;
            sphere.transform.localScale = Vector3.one * radius * 2f; // scale to match explosion radius

            // remove collider so it doesn't interfere
            Destroy(sphere.GetComponent<Collider>());

            // optional: make it semi-transparent
            var renderer = sphere.GetComponent<Renderer>();
            renderer.material = transparentMaterial;

            Destroy(sphere, 1f); // auto cleanup
        }

        private void SpawnImpactVFX(Vector3 position)
        {
            if (_impactVFX == null) return;

            var vfx = Instantiate(_impactVFX, position, Quaternion.identity);

            //scale to match explosion raidus
            float diameter = _explosionRadius * 2f;
            vfx.transform.localScale = Vector3.one * diameter;

            Destroy(vfx, 2f);// Cleanup
        }
    }
}