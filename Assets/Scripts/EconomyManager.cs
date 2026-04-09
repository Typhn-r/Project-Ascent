using UnityEngine;
using TMPro;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance;

    public TextMeshProUGUI coinText;
    public TextMeshProUGUI earningsText;

    private int totalCoins = 0;
    private int lastEarnings = 0;

    public float coinsPerMeter = 1f;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        UpdateCoinDisplay();
        if (earningsText != null)
            earningsText.gameObject.SetActive(false);
    }

    public void OnFlightComplete(float maxAltitude)
    {
        lastEarnings = Mathf.RoundToInt(maxAltitude * coinsPerMeter);

        if (maxAltitude >= 100) lastEarnings += 10;
        if (maxAltitude >= 500) lastEarnings += 50;
        if (maxAltitude >= 1000) lastEarnings += 100;

        lastEarnings = Mathf.Max(lastEarnings, 1);

        totalCoins += lastEarnings;
        UpdateCoinDisplay();
        ShowEarnings();
    }

    public bool SpendCoins(int amount)
    {
        if (totalCoins >= amount)
        {
            totalCoins -= amount;
            UpdateCoinDisplay();
            return true;
        }
        return false;
    }

    public int GetBalance()
    {
        return totalCoins;
    }

    void UpdateCoinDisplay()
    {
        if (coinText != null)
            coinText.text = totalCoins + " coins";
    }

    void ShowEarnings()
    {
        if (earningsText != null)
        {
            earningsText.gameObject.SetActive(true);
            earningsText.text = "+" + lastEarnings + " coins!";
            Invoke("HideEarnings", 2.5f);
        }
    }

    void HideEarnings()
    {
        if (earningsText != null)
            earningsText.gameObject.SetActive(false);
    }
}