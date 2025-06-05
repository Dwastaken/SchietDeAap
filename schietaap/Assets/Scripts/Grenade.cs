using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float Delay = 3f;
    public float radius = 5f;
    public float force = 700f;
    public float damage = 75f;
    public GameObject explosionEffect;
    float countdown;
    bool hasExploded = false;
    public Collider[] colliders;

    void Start()
    {
        countdown = Delay;
    }

    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }

    void Explode()
    {
        // Spawn explosie-effect
        GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);

        // Verzamel alle particle systems in de explosie
        ParticleSystem[] particleSystems = explosion.GetComponentsInChildren<ParticleSystem>();
        float maxDuration = 0f;

        // Zorg ervoor dat alle particle systems niet loopen
        foreach (ParticleSystem ps in particleSystems)
        {
            var main = ps.main;
            main.loop = false;

            // Bereken de totale levensduur (duur + deeltjes levensduur)
            float duration = main.duration + main.startLifetime.constantMax;
            if (duration > maxDuration) maxDuration = duration;
        }

        // Vernietig explosie-effect na langste levensduur
        Destroy(explosion, maxDuration);

        // Voer explosie-effect uit op nabije objecten
        colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius, 3.0f);
            }

            Enemy.Enemy target = nearbyObject.GetComponent<Enemy.Enemy>();
            if (target != null)
            {
                float distance = Vector3.Distance(transform.position, nearbyObject.transform.position);
                float damageMultiplier = 1f - (distance / radius);
                float finalDamage = damage * damageMultiplier;

                target.takedDamage(finalDamage);
                Debug.Log($"Granaat deed {finalDamage} schade aan {nearbyObject.name}");
            }
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}