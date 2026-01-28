using UnityEngine;
using UnityEngine.UI;

public class ExperienceManager : MonoBehaviour
{
    public static ExperienceManager Instance { get; private set; }

    [Header("Experience")]
    [SerializeField] private int currentExperience;
    [SerializeField] private int experienceForLevel = 10;
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private GameObject experiencePrefab;
    [SerializeField] private Slider levelProgressBar;
    [SerializeField] private AbilityController abilityController;
    [SerializeField] private UpgradeWindow upgradeWindow;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (levelProgressBar != null)
        {
            levelProgressBar.minValue = 0f;
            levelProgressBar.maxValue = 1f;
            levelProgressBar.value = CurrentLevelProgress;
        }
    }

    public int CurrentExperience => currentExperience;
    public int ExperienceForLevel => experienceForLevel;
    public int CurrentLevel => currentLevel;
    public float CurrentLevelProgress => experienceForLevel > 0 ? (float)currentExperience / experienceForLevel : 0f;

    public void AddExperience(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        currentExperience += amount;

        while (experienceForLevel > 0 && currentExperience >= experienceForLevel)
        {
            currentExperience -= experienceForLevel;
            LevelUp();
        }

        UpdateProgressUI();
    }

    public GameObject SpawnExperience(Vector3 position)
    {
        if (experiencePrefab == null)
        {
            return null;
        }

        return Instantiate(experiencePrefab, position, Quaternion.identity);
    }

    private void LevelUp()
    {
        currentLevel++;
        Time.timeScale = 0f;

        if (abilityController != null)
        {
            abilityController.GenerateUpgrades();
        }

        if (upgradeWindow != null)
        {
            upgradeWindow.gameObject.SetActive(true);
        }
    }

    private void UpdateProgressUI()
    {
        if (levelProgressBar != null)
        {
            levelProgressBar.value = CurrentLevelProgress;
        }
    }
}
