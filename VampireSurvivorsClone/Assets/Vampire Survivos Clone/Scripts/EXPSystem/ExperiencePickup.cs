using UnityEngine;

public class ExperiencePickup : MonoBehaviour
{
    [SerializeField] private int experienceAmount = 1;
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryCollect(other);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryCollect(collision.collider);
    }

    private void TryCollect(Collider2D other)
    {
        if (other == null || !other.CompareTag(playerTag))
        {
            return;
        }

        if (ExperienceManager.Instance != null)
        {
            ExperienceManager.Instance.AddExperience(experienceAmount);
        }

        Destroy(gameObject);
    }
}
