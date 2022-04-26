using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{

    public float damage = 20f;
    public float range = 1000f;
    public float ImpactForce = 0f;
    public float fireRate = 15f;

    public Camera fpsCam;

    public ParticleSystem muzzleFlah;

    public GameObject impactEffect;
    public GameObject impactImage;

    private float nextTimeToFire = 0f;

    public int maxAmmo = 25;
    private int currentAmmo;
    public float reloadTime = 0.7f;
    //public float shootTime = 0.04f;

    public AudioSource shoot;
    public AudioSource reload;
    public AudioSource no_ammo;


    public Animator animator;
    public Animator shooting;
    public Animator camera;

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        camera.SetBool("CameraIdle", true);
        //if (Input.GetKeyDown(r))
        //{
            //StartCoroutine(Reload());
            //currentAmmo = maxAmmo;
            //shooting.SetBool("Shooting", false);
        //}
        if (currentAmmo <= 0)
        {
            no_ammo.Play();
            shooting.SetBool("Shooting", false);
            reload.Play();
            StartCoroutine(Reload());
            return;
        }
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
            shooting.SetBool("Shooting", true);
            camera.SetBool("Razbros", true);
        }
        if (!Input.GetButton("Fire1"))
        {
            shooting.SetBool("Shooting", false);
            camera.SetBool("Razbros", false);
        }
        animator.SetBool("Idle", true);
    }

    IEnumerator Reload()
    {
        camera.SetBool("Razbros", false);

        animator.SetBool("Reloading", true);
        
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime - .20f);

        animator.SetBool("Reloading", false);

        yield return new WaitForSeconds(.20f);

        currentAmmo = maxAmmo;
    }

    //IEnumerator Shooting()
    //{
        //shooting.SetBool("Shooting", true);

        //Debug.Log("Shooting");

        //yield return new WaitForSeconds(shootTime - .1f);

        //shooting.SetBool("Shooting", false);

        //yield return new WaitForSeconds(.1f);
    //}

    void Shoot()
    {
        RaycastHit hit;
        shoot.Play();
        muzzleFlah.Play();

        currentAmmo--;
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
            Destroy(impactGO, 3f);
        }
    }
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), $"Ammos: {currentAmmo}/{maxAmmo}.");
    }
}
