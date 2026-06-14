using System.Collections.Generic;
using UnityEngine;

namespace Project.Systems.VFX
{
    public class ChainLightningVFX : MonoBehaviour
    {
        [SerializeField] private Material lineMaterial;
        [SerializeField] private float lineWidth = 0.1f;
        [SerializeField] private float lifeTime = 0.15f;
        [SerializeField] private float heightOffset = 1f;

        public void Play(
            List<(GameObject from, GameObject to)> links)
        {
            foreach (var link in links)
            {
                SpawnLine(link.from, link.to);
            }

            Destroy(gameObject, 1f);
        }

        private void SpawnLine(GameObject from, GameObject to)
        {
            GameObject lineObj = new GameObject("LightningSegment");

            var lr = lineObj.AddComponent<LineRenderer>();

            lr.material = lineMaterial;
            lr.positionCount = 2;
            lr.startWidth = lineWidth;
            lr.endWidth = lineWidth;
            lr.useWorldSpace = true;

            Vector3 a = from.transform.position + Vector3.up * heightOffset;
            Vector3 b = to.transform.position + Vector3.up * heightOffset;

            lr.SetPosition(0, a);
            lr.SetPosition(1, b);

            Destroy(lineObj, lifeTime);
            //var line = Instantiate(linePrefab);

            //line.positionCount = 2;

            //line.SetPosition(0, from.transform.position + Vector3.up);
            //line.SetPosition(1, to.transform.position + Vector3.up);

            //Destroy(line.gameObject, 0.15f);
        }
    }
}