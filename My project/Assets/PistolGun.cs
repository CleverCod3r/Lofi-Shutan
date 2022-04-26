using UnityEngine;

public class PistolGun : MonoBehaviour
{

    public float damage = 10f;
    public float range = 100f;
    public float ImpactForce = 30f;
    public float fireRate = 15f;

    public Camera fpsCam;

    public ParticleSystem muzzleFlah;

    public GameObject impactEffect;

    public AudioSource shoot;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
            shoot.Play();
            muzzleFlah.Play();
        }
    }
    void Shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * ImpactForce);
            }

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);
        }
    }
}
