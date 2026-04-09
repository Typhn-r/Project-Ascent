using UnityEngine;

public class RocketController : MonoBehaviour
{
    public float thrustForce = 2f;
    public float burnTime = 2f;
    public ParticleSystem flameTrail;
    private Rigidbody2D rb;
    private bool hasLaunched = false;
    private bool engineOn = false;
    private float burnTimer = 0f;
    private float currentThrust;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !hasLaunched)
        {
            LaunchFromButton();
        }

        if (engineOn)
        {
            rb.AddForce(Vector2.up * currentThrust);
            burnTimer -= Time.deltaTime;

            if (burnTimer <= 0)
            {
                engineOn = false;
                if (flameTrail != null)
                    flameTrail.Stop();
            }
        }
    }

    public void LaunchFromButton()
    {
        hasLaunched = true;
        engineOn = true;
        burnTimer = burnTime;
        currentThrust = thrustForce * Random.Range(0.85f, 1.15f);
        rb.linearVelocity = Vector2.zero;
        if (flameTrail != null)
            flameTrail.Play();
    }

    public void ResetLaunch()
    {
        hasLaunched = false;
    }
}