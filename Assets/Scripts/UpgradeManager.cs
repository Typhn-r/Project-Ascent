using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public RocketController rocket;

    public Button thrustUpgradeButton;
    public TextMeshProUGUI thrustUpgradeText;
    public Button burnTimeUpgradeButton;
    public TextMeshProUGUI burnTimeUpgradeText;

    public GameObject upgradePanel;
    public Button toggleUpgradeButton;

    private int thrustLevel = 0;
    private int burnTimeLevel = 0;

    public int thrustBaseCost = 10;
    public int burnTimeBaseCost = 15;
    public float costScaling = 1.5f;

    public float thrustPerLevel = 0.5f;
    public float burnTimePerLevel = 0.3f;

    private bool panelOpen = false;

    void Start()
    {
        thrustUpgradeButton.onClick.AddListener(BuyThrustUpgrade);
        burnTimeUpgradeButton.onClick.AddListener(BuyBurnTimeUpgrade);
        toggleUpgradeButton.onClick.AddListener(TogglePanel);

        upgradePanel.SetActive(false);
        UpdateUpgradeDisplay();
    }

    void TogglePanel()
    {
        panelOpen = !panelOpen;
        upgradePanel.SetActive(panelOpen);
    }

    int GetThrustCost()
    {
        return Mathf.RoundToInt(thrustBaseCost * Mathf.Pow(costScaling, thrustLevel));
    }

    int GetBurnTimeCost()
    {
        return Mathf.RoundToInt(burnTimeBaseCost * Mathf.Pow(costScaling, burnTimeLevel));
    }

    void BuyThrustUpgrade()
    {
        int cost = GetThrustCost();
        if (EconomyManager.Instance.SpendCoins(cost))
        {
            thrustLevel++;
            rocket.thrustForce += thrustPerLevel;
            UpdateUpgradeDisplay();
        }
    }

    void BuyBurnTimeUpgrade()
    {
        int cost = GetBurnTimeCost();
        if (EconomyManager.Instance.SpendCoins(cost))
        {
            burnTimeLevel++;
            rocket.burnTime += burnTimePerLevel;
            UpdateUpgradeDisplay();
        }
    }

    void UpdateUpgradeDisplay()
    {
        int thrustCost = GetThrustCost();
        int burnCost = GetBurnTimeCost();
        int balance = EconomyManager.Instance.GetBalance();

        thrustUpgradeText.text = "Thrust Lv." + thrustLevel + "\n"
            + "Cost: " + thrustCost + " coins\n"
            + "(" + rocket.thrustForce.ToString("F1") + " -> " + (rocket.thrustForce + thrustPerLevel).ToString("F1") + ")";

        burnTimeUpgradeText.text = "Burn Time Lv." + burnTimeLevel + "\n"
            + "Cost: " + burnCost + " coins\n"
            + "(" + rocket.burnTime.ToString("F1") + "s -> " + (rocket.burnTime + burnTimePerLevel).ToString("F1") + "s)";

        thrustUpgradeButton.interactable = (balance >= thrustCost);
        burnTimeUpgradeButton.interactable = (balance >= burnCost);
    }

    public void RefreshButtons()
    {
        UpdateUpgradeDisplay();
    }
}