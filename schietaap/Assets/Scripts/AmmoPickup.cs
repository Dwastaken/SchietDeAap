using UnityEngine;

public class AmmoPickup : MonoBehaviour
{

    [Header("Ammo Settings")]
    public int ammoAmount = 30;
    public AudioClip pickupSound;

    [Header("Visual Effects")]
    public float rotationSpeed = 50f;
    public float bobSpeed = 2f;
    public float bobHeight = 0.5f;

    [Header("Despawn Timer")]
    public float lifespan = 40f;
    public float blinkStartTime = 5f;
    public float blinkInterval = 0.2f;

    private Vector3 startPosition;
    private AudioSource audioSource;
    private Transform player;

    private float lifespanTimer;
    private float blinkTimer;
    private bool isBlinking = false;
    private Renderer[] renderers;

    void Start()
    {
        startPosition = transform.position;
        audioSource = GetComponent<AudioSource>();
        renderers = GetComponentsInChildren<Renderer>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        lifespanTimer = lifespan;
        FacePlayer();
    }

    void Update()
    {

        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        if (player != null)
        {
            FacePlayer();
        }


        lifespanTimer -= Time.deltaTime;


        if (lifespanTimer <= blinkStartTime)
        {
            isBlinking = true;
        }

        if (isBlinking)
        {
            blinkTimer -= Time.deltaTime;
            if (blinkTimer <= 0f)
            {
                ToggleVisibility();
                blinkTimer = blinkInterval;
            }
        }


        if (lifespanTimer <= 0f)
        {
            Destroy(gameObject);
        }
    }

    void ToggleVisibility()
    {
        foreach (Renderer r in renderers)
        {
            r.enabled = !r.enabled;
        }
    }

    void FacePlayer()
    {
        if (player == null) return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WeaponSwitch weaponSwitch = other.GetComponent<WeaponSwitch>();
            if (weaponSwitch == null)
                weaponSwitch = other.GetComponentInChildren<WeaponSwitch>();

            if (weaponSwitch != null)
            {
                Transform activeWeapon = GetActiveWeapon(weaponSwitch);
                if (activeWeapon != null)
                {
                    Shooting shootingScript = activeWeapon.GetComponent<Shooting>();
                    if (shootingScript != null)
                    {
                        bool ammoAdded = shootingScript.AddAmmo(ammoAmount);

                        if (ammoAdded)
                        {
                            if (pickupSound != null && audioSource != null)
                                audioSource.PlayOneShot(pickupSound);

                            Debug.Log($"Ammo opgepakt! +{ammoAmount} kogels");
                            Destroy(gameObject);
                        }
                    }
                }
            }
        }
    }

    Transform GetActiveWeapon(WeaponSwitch weaponSwitch)
    {
        Shooting[] weapons = weaponSwitch.GetComponentsInChildren<Shooting>(true);
        foreach (Shooting weapon in weapons)
        {
            if (weapon.gameObject.activeInHierarchy)
            {
                return weapon.transform;
            }
        }
        return null;
    }
}
