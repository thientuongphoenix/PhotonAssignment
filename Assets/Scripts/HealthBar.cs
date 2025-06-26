using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private Transform _valueImage;

    private void Start()
    {
        UpdateHealthPointValue();
        _health.OnHealthChanged.AddListener(UpdateHealthPointValue);
    }

    public void UpdateHealthPointValue() => _valueImage.localScale = new Vector3(_health.HealthRate, 1, 1);

}
