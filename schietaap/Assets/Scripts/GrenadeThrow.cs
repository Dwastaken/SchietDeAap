using UnityEngine;
using UnityEngine.UI;

public class GrenadeThrow : MonoBehaviour
{
    public float maxThrowForce = 40f;
    public float chargeRate = 3f;
    public GameObject grenadePrefab;
    public Slider chargeSlider;


    private float currentThrowForce;
    private bool isCharging;

    private void Start()
    {
        chargeSlider.gameObject.SetActive(false);
        chargeSlider.maxValue = maxThrowForce;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
            isCharging = true;
            currentThrowForce = 0f;
            chargeSlider.gameObject.SetActive(true);
        }


        if (isCharging && Input.GetKey(KeyCode.C))
        {
            currentThrowForce += chargeRate * Time.deltaTime;
            currentThrowForce = Mathf.Clamp(currentThrowForce, 0f, maxThrowForce);
            chargeSlider.value = currentThrowForce;
        }


        if (Input.GetKeyUp(KeyCode.C))
        {
            isCharging = false;
            ThrowGrenade(currentThrowForce);
            chargeSlider.gameObject.SetActive(false);
        }
    }

    void ThrowGrenade(float force)
    {
        GameObject grenade = Instantiate(grenadePrefab, transform.position, transform.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * force, ForceMode.VelocityChange);
    }
}
