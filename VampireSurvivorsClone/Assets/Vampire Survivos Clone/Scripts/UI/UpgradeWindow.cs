using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UpgradeWindow : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text[] abilityTypeTexts;
    [SerializeField] private TMP_Text[] statTexts;
    [SerializeField] private AbilityController abilityController;

    private readonly List<AbilityController.UpgradeOption> upgrades = new List<AbilityController.UpgradeOption>(3);

    public IReadOnlyList<AbilityController.UpgradeOption> Upgrades => upgrades;

    public void SetUpgrades(IEnumerable<AbilityController.UpgradeOption> options)
    {
        upgrades.Clear();
        if (options == null)
        {
            UpdateUI();
            return;
        }

        foreach (var option in options)
        {
            upgrades.Add(option);
        }

        UpdateUI();
    }

    public void OnUpgradeButtonClicked(int buttonId)
    {
        if (abilityController == null)
        {
            return;
        }

        if (buttonId < 0 || buttonId >= upgrades.Count)
        {
            return;
        }

        AbilityController.UpgradeOption option = upgrades[buttonId];
        abilityController.ApplyUpgrade(option.abilityType, option.stat);
        Time.timeScale = 1f;
    }

    private void UpdateUI()
    {
        int slots = Mathf.Max(abilityTypeTexts != null ? abilityTypeTexts.Length : 0,
                              statTexts != null ? statTexts.Length : 0);

        for (int i = 0; i < slots; i++)
        {
            string abilityText = string.Empty;
            string statText = string.Empty;

            if (i < upgrades.Count)
            {
                abilityText = upgrades[i].abilityType.ToString();
                statText = upgrades[i].stat.ToString();

                if (upgrades[i].abilityType == AbilitiesType.aura && upgrades[i].stat == Stats.amount)
                {
                    statText = "radius";
                }
            }

            if (abilityTypeTexts != null && i < abilityTypeTexts.Length && abilityTypeTexts[i] != null)
            {
                abilityTypeTexts[i].text = abilityText;
            }

            if (statTexts != null && i < statTexts.Length && statTexts[i] != null)
            {
                statTexts[i].text = statText;
            }
        }
    }
}
