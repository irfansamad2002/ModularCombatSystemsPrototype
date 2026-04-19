using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Image fillImage;

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        float ratio = health.maxHealth > 0 ? (float)health.currentHealth / health.maxHealth : 0f;
        fillImage.fillAmount = ratio;
    }

    private void LateUpdate()
    {
        transform.forward = cam.transform.forward; 
    }
}
