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

    private Vector3 startPosition;
    private AudioSource audioSource;
    private Transform player;

    void Start()
    {
        startPosition = transform.position;
        audioSource = GetComponent<AudioSource>();

        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        
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
            {
                weaponSwitch = other.GetComponentInChildren<WeaponSwitch>();
            }

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
                            {
                                audioSource.PlayOneShot(pickupSound);
                            }

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