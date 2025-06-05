using TMPro;
using UnityEngine;

public class WeaponAmmoUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI weaponNameText;

    [Header("WeaponSwitch Reference")]
    public WeaponSwitch weaponSwitch;

    [Header("Display Settings")]
    public Color normalColor = Color.white;
    public Color lowAmmoColor = Color.yellow;
    public Color emptyAmmoColor = Color.red;
    public Color reloadingColor = Color.cyan;
    [Range(0.1f, 0.5f)]
    public float lowAmmoPercentage = 0.30f;

    private Shooting currentShootingScript;

    void Update()
    {
        UpdateCurrentWeapon();
        UpdateAmmoDisplay();
    }

    void UpdateCurrentWeapon()
    {
        if (weaponSwitch == null) return;


        Transform activeWeapon = null;
        int weaponIndex = 0;

        foreach (Transform weapon in weaponSwitch.transform)
        {
            if (weapon.gameObject.activeInHierarchy)
            {
                activeWeapon = weapon;
                break;
            }
            weaponIndex++;
        }


        if (activeWeapon != null)
        {
            Shooting shootingScript = activeWeapon.GetComponent<Shooting>();
            if (shootingScript != currentShootingScript)
            {
                currentShootingScript = shootingScript;
            }
        }
        else
        {
            currentShootingScript = null;
        }
    }

    void UpdateAmmoDisplay()
    {
        if (currentShootingScript == null || ammoText == null)
        {
            if (ammoText != null)
                ammoText.text = "No Weapon";
            return;
        }


        int currentAmmo = currentShootingScript.GetCurrentAmmo();
        int totalAmmo = currentShootingScript.GetTotalAmmo();
        int maxAmmo = currentShootingScript.GetMaxAmmo();


        int dynamicLowAmmoThreshold = Mathf.RoundToInt(maxAmmo * lowAmmoPercentage);


        ammoText.text = $"{currentAmmo}/{totalAmmo}";


        if (weaponNameText != null && currentShootingScript.gunData != null)
        {
            weaponNameText.text = currentShootingScript.gunData.gunName;
        }


        Color textColor = normalColor;

        if (currentShootingScript.gunData.isReloading)
        {
            textColor = reloadingColor;
            ammoText.text += " (Reloading...)";
        }
        else if (currentAmmo == 0)
        {
            textColor = emptyAmmoColor;
        }
        else if (currentAmmo <= dynamicLowAmmoThreshold)
        {
            textColor = lowAmmoColor;
        }

        ammoText.color = textColor;


        if (weaponNameText != null)
        {
            weaponNameText.color = textColor;
        }
    }
}