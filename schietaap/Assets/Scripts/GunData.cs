using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "Scriptable Objects/GunData")]
public class GunData : ScriptableObject
{
    [Header("Basisinformatie")]
    public string gunName = "New Gun";
    public float damage = 10f;
    public float range = 100f;

    [Header("Munitie")]
    public int maxAmmo = 30;
    public float reloadTime = 2f;
    public bool isReloading = false;

    [Header("Schietgedrag")]
    public float fireRate = 0.1f;
    public bool isAutomatic = false;

    [Header("Recoil")]
    public Vector2 recoilAmount = new Vector2(1f, 1f);
    public float recoilRecoverySpeed = 5f;

    [Header("geluid en visueel")]
    public AudioClip shootSound;
    public GameObject muzzleFlashPrefab;
}
