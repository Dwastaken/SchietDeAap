using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("Health")]
    public float health = 50f;

    [Header("Ammo Drop")]
    public GameObject ammoPickupPrefab;
    [Range(0f, 1f)]
    public float dropChance = 0.25f;
    public Vector3 dropOffset = Vector3.up;

    public void takedDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {

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

        Debug.Log("Ammo drop gespawnd!");

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