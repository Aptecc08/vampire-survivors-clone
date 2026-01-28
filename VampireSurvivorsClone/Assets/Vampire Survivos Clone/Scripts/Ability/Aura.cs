using UnityEngine;
using System.Collections;

public class Aura : Ability
{
    [Header("Aura Ability")]
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private string enemyTag = "Enemy";
    [Header("Aura VFX")]
    [SerializeField] private LineRenderer pulseRenderer;
    [SerializeField] private int pulseSegments = 64;
    [SerializeField] private float pulseDuration = 0.2f;
    [SerializeField] private float pulseWidth = 0.05f;
    [SerializeField] private Color pulseColor = new Color(0.2f, 0.9f, 1f, 1f);

    private Coroutine damageRoutine;
    private Coroutine pulseRoutine;

    protected override void Awake()
    {
        base.Awake();
        EnsurePulseRenderer();
    }

    private void OnEnable()
    {
        if (damageRoutine == null)
        {
            damageRoutine = StartCoroutine(DamageLoop());
        }
    }

    private void OnDisable()
    {
        if (damageRoutine != null)
        {
            StopCoroutine(damageRoutine);
            damageRoutine = null;
        }
    }

    private System.Collections.IEnumerator DamageLoop()
    {
        while (true)
        {
            if (level > 0 && attackSpeed > 0f)
            {
                DamageAllEnemies();
                yield return new WaitForSeconds(attackSpeed);
                continue;
            }

            yield return null;
        }
    }

    private void DamageAllEnemies()
    {
        TriggerPulse();

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        if (enemies == null || enemies.Length == 0)
        {
            return;
        }

        Vector3 center = owner != null ? owner.position : transform.position;
        float rangeSqr = range * range;

        for (int i = 0; i < enemies.Length; i++)
        {
            GameObject enemy = enemies[i];
            if (enemy == null)
            {
                continue;
            }

            if ((enemy.transform.position - center).sqrMagnitude > rangeSqr)
            {
                continue;
            }

            IDamageable damageable = enemy.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
        }
    }

    private void TriggerPulse()
    {
        if (pulseRenderer == null)
        {
            return;
        }

        if (pulseRoutine != null)
        {
            StopCoroutine(pulseRoutine);
        }

        pulseRoutine = StartCoroutine(PulseRoutine());
    }

    private IEnumerator PulseRoutine()
    {
        pulseRenderer.enabled = true;
        UpdatePulseCircle();

        float elapsed = 0f;
        while (elapsed < pulseDuration)
        {
            float t = pulseDuration > 0f ? elapsed / pulseDuration : 1f;
            float alpha = Mathf.Lerp(1f, 0f, t);
            Color c = new Color(pulseColor.r, pulseColor.g, pulseColor.b, alpha);
            pulseRenderer.startColor = c;
            pulseRenderer.endColor = c;

            elapsed += Time.deltaTime;
            yield return null;
        }

        pulseRenderer.enabled = false;
        pulseRoutine = null;
    }

    private void UpdatePulseCircle()
    {
        if (pulseRenderer == null)
        {
            return;
        }

        int segments = Mathf.Max(3, pulseSegments);
        pulseRenderer.positionCount = segments + 1;

        float angleStep = 360f / segments;
        for (int i = 0; i <= segments; i++)
        {
            float angle = angleStep * i * Mathf.Deg2Rad;
            Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * range;
            pulseRenderer.SetPosition(i, pos);
        }
    }

    private void EnsurePulseRenderer()
    {
        if (pulseRenderer != null)
        {
            return;
        }

        GameObject go = new GameObject("AuraPulse");
        go.transform.SetParent(transform, false);
        pulseRenderer = go.AddComponent<LineRenderer>();
        pulseRenderer.useWorldSpace = false;
        pulseRenderer.loop = true;
        pulseRenderer.widthMultiplier = pulseWidth;
        pulseRenderer.startColor = pulseColor;
        pulseRenderer.endColor = pulseColor;
        pulseRenderer.material = new Material(Shader.Find("Sprites/Default"));
        pulseRenderer.enabled = false;
    }

    public override void LevelUp(Stats stat)
    {
        base.LevelUp(stat);

        switch (stat)
        {
            case Stats.damage:
                damage += 1f;
                break;
            case Stats.speed:
                attackSpeed = Mathf.Max(0.1f, attackSpeed - 0.1f);
                break;
            case Stats.amount:
                range += 0.25f;
                UpdatePulseCircle();
                break;
        }
    }
}
