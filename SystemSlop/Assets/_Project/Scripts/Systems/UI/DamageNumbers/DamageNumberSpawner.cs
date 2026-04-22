using UnityEngine;

public class DamageNumberSpawner : MonoBehaviour
{
    public static DamageNumberSpawner Instance;

    [SerializeField] private DamageNumber prefab;
    private Transform _canvas;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Multiple DamageNumberSpawners in scene!");
            Destroy(gameObject);
            return;
        }
        _canvas = transform;
        Instance = this;
    }

    public void Spawn(float damage, Vector3 worldPos)
    {
        var obj = Instantiate(prefab, _canvas);
        obj.transform.position = worldPos;
        obj.Setup(damage);
    }
}
