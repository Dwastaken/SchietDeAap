using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("Health")]
    public float health = 50f;
    public float maxHealth = 50f; // Om health te kunnen resetten

    [Header("Ammo Drop")]
    public GameObject ammoPickupPrefab;
    [Range(0f, 1f)]
    public float dropChance = 0.25f;
    public Vector3 dropOffset = Vector3.up;

    [Header("Debug")]
    public bool showDebugMessages = true;

    void Start()
    {
        maxHealth = health; // Zet max health op start waarde
    }

    public void takedDamage(float amount)
    {
        health -= amount;

        if (showDebugMessages)
        {
            Debug.Log($"{gameObject.name} kreeg {amount} schade. Health: {health}/{maxHealth}");
        }

        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        if (showDebugMessages)
        {
            Debug.Log($"{gameObject.name} is gestorven!");
        }

        // Spawn ammo drop met kans
        if (ammoPickupPrefab != null && Random.Range(0f, 1f) <= dropChance)
        {
            SpawnAmmoDrop();
        }

        Destroy(gameObject);
    }

    void SpawnAmmoDrop()
    {
        Vector3 dropPosition = transform.position + dropOffset;
        GameObject ammoDropObject = Instantiate(ammoPickupPrefab, dropPosition, Quaternion.identity);

        if (showDebugMessages)
        {
            Debug.Log("Ammo drop gespawnd!");
        }

        // Zorg ervoor dat de ammo drop een trigger collider heeft
        Collider collider = ammoDropObject.GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
        else
        {
            SphereCollider sphereCollider = ammoDropObject.AddComponent<SphereCollider>();
            sphereCollider.isTrigger = true;
            sphereCollider.radius = 1f;
        }
    }
}