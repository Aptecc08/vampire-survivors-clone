using UnityEngine;
using System;
using System.Collections.Generic;

public enum AbilitiesType
{
    gun,
    aura,
    orbit,
}

public class AbilityController : MonoBehaviour
{
    [Serializable]
    public struct UpgradeOption
    {
        public AbilitiesType abilityType;
        public Stats stat;

        public UpgradeOption(AbilitiesType abilityType, Stats stat)
        {
            this.abilityType = abilityType;
            this.stat = stat;
        }
    }

    [SerializeField] private UpgradeWindow upgradeWindow;

    private readonly List<UpgradeOption> currentUpgrades = new List<UpgradeOption>(3);
    private AbilitiesType[] abilityTypes;
    private Stats[] stats;

    public ProjectileGun gun;
    public Orbit orbit;
    public Aura aura;
    private void Awake()
    {
        abilityTypes = (AbilitiesType[])Enum.GetValues(typeof(AbilitiesType));
        stats = (Stats[])Enum.GetValues(typeof(Stats));
    }

    public IReadOnlyList<UpgradeOption> CurrentUpgrades => currentUpgrades;

    public void GenerateUpgrades()
    {
        currentUpgrades.Clear();

        int safety = 0;
        while (currentUpgrades.Count < 3 && safety < 50)
        {
            safety++;
            AbilitiesType abilityType = abilityTypes[UnityEngine.Random.Range(0, abilityTypes.Length)];
            Stats stat = stats[UnityEngine.Random.Range(0, stats.Length)];
            UpgradeOption option = new UpgradeOption(abilityType, stat);

            if (!currentUpgrades.Contains(option))
            {
                currentUpgrades.Add(option);
            }
        }

        if (upgradeWindow != null)
        {
            upgradeWindow.SetUpgrades(currentUpgrades);
        }
    }

    public void ApplyUpgrade(AbilitiesType abilityType, Stats stat)
    {
        Ability ability = null;

        switch (abilityType)
        {
            case AbilitiesType.gun:
                ability = gun;
                break;
            case AbilitiesType.aura:
                ability = aura;
                break;
            case AbilitiesType.orbit:
                ability = orbit;
                break;
        }

        if (ability != null)
        {
            ability.LevelUp(stat);
        }

        upgradeWindow.gameObject.SetActive(false);
    }
}
