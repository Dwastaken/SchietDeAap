using System.Collections;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GunData gunData;
    public Camera fpsCam;
    public Animator animator;
    private int currentAmmo;
    void Start()
    {
        currentAmmo = gunData.maxAmmo;
    }

    private void OnEnable()
    {
        gunData.isReloading = false;
        animator.SetBool("Reloading", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (gunData.isReloading)
            return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    IEnumerator Reload()
    {
        gunData.isReloading = true;
        Debug.Log("reloading...");
        animator.SetBool("isReloading", true);
        yield return new WaitForSeconds(gunData.reloadTime - .25f);

        animator.SetBool("isReloading", false);
        yield return new WaitForSeconds(.25f);
        currentAmmo = gunData.maxAmmo;
        gunData.isReloading = false;
    }


    void Shoot()
    {
        currentAmmo--;
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit))
        {
            Debug.Log(hit.transform.name);
            Target target = hit.transform.GetComponent<Target>();

            if (target != null)
            {
                target.takedDamage(gunData.damage);
            }

        }
    }
}
