using System.Collections;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GunData gunData;
    public Camera fpsCam;
    public Animator animator;

    private int currentAmmo;
    private int totalAmmo;

    void Start()
    {
        currentAmmo = gunData.maxAmmo;
        totalAmmo = gunData.totalAmmo;
    }

    private void OnEnable()
    {
        gunData.isReloading = false;
        animator.SetBool("isReloading", false);
    }

    void Update()
    {
        // Als we aan het reloaden zijn, doe niets
        if (gunData.isReloading)
            return;

        // Schieten met linkermuisknop
        if (Input.GetButtonDown("Fire1"))
        {
            if (currentAmmo > 0)
            {
                Shoot();
            }
            else if (totalAmmo > 0)
            {
                // Reload alleen als je probeert te schieten met lege magazijn
                StartCoroutine(Reload());
            }
            else
            {
                // Geen ammo meer
                Debug.Log("Geen ammo meer!");
            }
        }

        // Handmatig reloaden met R toets
        if (Input.GetKeyDown(KeyCode.R) && CanReload())
        {
            StartCoroutine(Reload());
        }
    }

    bool CanReload()
    {
        // Je kunt reloaden als:
        // - Je hebt nog totale ammo
        // - Je magazijn is niet vol
        // - Je bent niet al aan het reloaden
        return totalAmmo > 0 && currentAmmo < gunData.maxAmmo && !gunData.isReloading;
    }

    IEnumerator Reload()
    {
        // Controleer of we kunnen reloaden
        if (totalAmmo <= 0 || currentAmmo >= gunData.maxAmmo)
        {
            yield break; // Stop de coroutine als we niet kunnen reloaden
        }

        gunData.isReloading = true;
        Debug.Log("Reloading...");
        animator.SetBool("isReloading", true);

        // Wacht voor reload tijd
        yield return new WaitForSeconds(gunData.reloadTime - 0.25f);

        animator.SetBool("isReloading", false);
        yield return new WaitForSeconds(0.25f);

        // Bereken hoeveel ammo we nodig hebben
        int ammoNeeded = gunData.maxAmmo - currentAmmo;
        int ammoToReload = Mathf.Min(ammoNeeded, totalAmmo);

        // Update ammo counts
        currentAmmo += ammoToReload;
        totalAmmo -= ammoToReload;

        gunData.isReloading = false;

        Debug.Log($"Reload voltooid! Huidige ammo: {currentAmmo}/{gunData.maxAmmo}, Totale ammo: {totalAmmo}");
    }

    void Shoot()
    {
        currentAmmo--;
        Debug.Log($"Geschoten! Ammo over: {currentAmmo}/{gunData.maxAmmo}, Totaal: {totalAmmo}");

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit))
        {
            Debug.Log($"Geraakt: {hit.transform.name}");
            Target target = hit.transform.GetComponent<Target>();

            if (target != null)
            {
                target.takedDamage(gunData.damage);
            }
        }
    }

    // Handige getter methods voor UI of andere scripts
    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    public int GetTotalAmmo()
    {
        return totalAmmo;
    }

    public int GetMaxAmmo()
    {
        return gunData.maxAmmo;
    }
}