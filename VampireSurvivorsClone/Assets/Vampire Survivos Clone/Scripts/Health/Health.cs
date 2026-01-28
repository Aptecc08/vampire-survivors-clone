using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    public float CurrentValue;
    [SerializeField] private float _maxValue;
    void Start()
    {
        CurrentValue = _maxValue;
    }

    public void TakeDamage(float amount)
    {
        CurrentValue -= amount;

        if(CurrentValue <= 0)
            Die();
    }

    private void Die()
    {
        if (ExperienceManager.Instance != null)
        {
            ExperienceManager.Instance.SpawnExperience(transform.position);
        }

        Destroy(gameObject);
    }
}
