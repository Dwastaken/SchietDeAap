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

        if (gunData.isReloading)
            return;


        if (Input.GetButtonDown("Fire1"))
        {
            if (currentAmmo > 0)
            {
                Shoot();
            }
            else if (totalAmmo > 0)
            {

                StartCoroutine(Reload());
            }
            else
            {

                Debug.Log("Geen ammo meer!");
            }
        }


        if (Input.GetKeyDown(KeyCode.R) && CanReload())
        {
            StartCoroutine(Reload());
        }
    }

    bool CanReload()
    {

        return totalAmmo > 0 && currentAmmo < gunData.maxAmmo && !gunData.isReloading;
    }

    IEnumerator Reload()
    {

        if (totalAmmo <= 0 || currentAmmo >= gunData.maxAmmo)
        {
            yield break;
        }

        gunData.isReloading = true;
        Debug.Log("Reloading...");
        animator.SetBool("isReloading", true);


        yield return new WaitForSeconds(gunData.reloadTime - 0.25f);

        animator.SetBool("isReloading", false);
        yield return new WaitForSeconds(0.25f);


        int ammoNeeded = gunData.maxAmmo - currentAmmo;
        int ammoToReload = Mathf.Min(ammoNeeded, totalAmmo);


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
            Enemy.Enemy target = hit.transform.GetComponent<Enemy.Enemy>();

            if (target != null)
            {
                target.takedDamage(gunData.damage);
            }
        }
    }



    public bool AddAmmo(int amount)
    {

        int ammoToAdd = Mathf.Min(amount, gunData.totalAmmo - totalAmmo);

        if (ammoToAdd <= 0)
        {
            Debug.Log("Ammo is al vol!");
            return false;
        }

        totalAmmo += ammoToAdd;
        Debug.Log($"Ammo toegevoegd: +{ammoToAdd}. Totaal: {totalAmmo}/{gunData.totalAmmo}");
        return true;
    }


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