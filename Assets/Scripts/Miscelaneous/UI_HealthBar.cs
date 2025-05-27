using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    [SerializeField] Vector3 offSet;
    [SerializeField] Health_Handler target;
    [SerializeField] Slider slider;
    Transform targetTransform;
    RectTransform _transform;

    private void Awake()
    {
        targetTransform = target.transform;
        _transform = GetComponent<RectTransform>();
    }
    private void Start()
    {
        target.OnHealthDecreaseEvent.AddListener(UpdateUI);
        target.OnHealthIncreaseEvent.AddListener(UpdateUI);
        slider.maxValue = target.P_GetMaxHealth();
        slider.value = slider.maxValue;
    }

    private void LateUpdate()
    {
        _transform.position = targetTransform.position + offSet;
    }
    private void UpdateUI(float value)
    {
        slider.value = value;
    }
}
