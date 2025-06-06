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
            // See if this collider belongs to an Enemy
            Enemy.Enemy target = nearbyObject.GetComponent<Enemy.Enemy>();
            if (target != null)
            {
                // Calculate distance‐based damage
                float distance = Vector3.Distance(transform.position, nearbyObject.transform.position);
                float damageMultiplier = 1f - (distance / radius);
                float finalDamage = damage * damageMultiplier;

                // 2) Deal damage (this calls target.TakeDamage → target.Die() → activates ragdoll)
                target.takedDamage(finalDamage);
                Debug.Log($"Granaat deed {finalDamage} schade aan {nearbyObject.name}");

                // 3) If the enemy just died (health <= 0), ragdoll limbs are now non-kinematic.
                //    Grab the Ragdoll component and push on all bone Rigidbodies.
                if (target.health <= 0f)
                {
                    Ragdoll ragdoll = target.GetComponent<Ragdoll>();
                    if (ragdoll != null)
                    {
                        // Make sure the ragdoll has had a chance to turn on all child rigidbodies in IsRagdoll(true).
                        // Usually IsRagdoll(true) sets rb[i].isKinematic = false immediately, so we can push right away.
                        foreach (Rigidbody boneRb in ragdoll.rb)
                        {
                            boneRb.AddExplosionForce(force, transform.position, radius, 3.0f);
                        }
                    }

                    continue; // move on to next collider
                }

                // If we reach here, it was an enemy but they survived. 
                // You can still push on the root Rigidbody (if any) or skip.
                Rigidbody rootRB = nearbyObject.GetComponent<Rigidbody>();
                if (rootRB != null)
                {
                    rootRB.AddExplosionForce(force, transform.position, radius, 3.0f);
                }
            }
            else
            {
                // 4) Not an enemy at all → do standard physics push on any Rigidbody this collider has
                Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(force, transform.position, radius, 3.0f);
                }
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