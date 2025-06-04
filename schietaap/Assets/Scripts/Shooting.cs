using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GunData gunData;
    public Camera fpsCam;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }
    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit))
        {
            Debug.Log(hit.transform.name);
           Target target = hit.transform.GetComponent<Target>();
            
            if(target !=null)
            {
                target.takedDamage(gunData.damage);
            }    

        }
    }
}
