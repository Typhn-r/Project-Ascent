using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI altitudeText;
    public Button launchButton;
    public Transform rocket;
    public UpgradeManager upgradeManager;

    private float groundLevel;
    private bool hasLaunched = false;
    private float maxAltitude = 0f;
    private bool rocketLanded = false;
    private Rigidbody2D rocketRb;
    private Vector3 startPosition;

    void Start()
    {
        groundLevel = rocket.position.y;
        startPosition = rocket.position;
        rocketRb = rocket.GetComponent<Rigidbody2D>();
        launchButton.onClick.AddListener(OnLaunchPressed);
    }

    void Update()
    {
        if (hasLaunched && !rocketLanded)
        {
            float altitude = Mathf.Max(0, rocket.position.y - groundLevel);
            float altitudeMeters = altitude * 1f;

            if (altitudeMeters > maxAltitude)
                maxAltitude = altitudeMeters;

            altitudeText.text = Mathf.RoundToInt(altitudeMeters) + " m";

            if (rocketRb.linearVelocity.y < 0 && altitude < 0.05f && hasLaunched)
            {
                rocketLanded = true;
                altitudeText.text = "MAX: " + Mathf.RoundToInt(maxAltitude) + " m";
                launchButton.gameObject.SetActive(true);

                if (EconomyManager.Instance != null)
                    EconomyManager.Instance.OnFlightComplete(maxAltitude);

                if (upgradeManager != null)
                    upgradeManager.RefreshButtons();

                Invoke("ResetRocket", 2f);
            }
        }
    }

    void OnLaunchPressed()
    {
        if (!hasLaunched || rocketLanded)
        {
            hasLaunched = true;
            rocketLanded = false;
            maxAltitude = 0f;
            rocket.GetComponent<RocketController>().LaunchFromButton();
            launchButton.gameObject.SetActive(false);
        }
    }

    void ResetRocket()
    {
        rocket.position = startPosition;
        rocketRb.linearVelocity = Vector2.zero;
        rocketRb.angularVelocity = 0f;
        rocket.rotation = Quaternion.identity;
        rocket.GetComponent<RocketController>().ResetLaunch();
    }
}