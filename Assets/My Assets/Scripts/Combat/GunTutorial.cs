using UnityEngine;

public class GunTutorial : MonoBehaviour
{
    [Header("Gun Stats")]
    [SerializeField] int damage;
    [SerializeField] float fireRate, spreadX, spreadY, range, reloadTime, timeBetweenShots;
    [SerializeField] int magazineSize, bulletsPerShot;
    [SerializeField] bool automatic;
    int ammoLeft, ammoShot;

    [Header("Refferences")]
    [SerializeField] Camera fpsCamera;
    [SerializeField] Transform firePoint;
    [SerializeField] RaycastHit rayHit;
    [SerializeField] LayerMask enemy;

    bool shootPressed, readyToShoot, reloading;

    void Start()
    {
        if (automatic)
        {
            shootPressed = Input.GetMouseButton(0);
        }
        else
        {
            shootPressed = Input.GetMouseButtonDown(0);
        }
    }

    void Update()
    {
        PlayerInput();
    }

    void FixedUpdate()
    {
        
    }

    void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.R) && ammoLeft < magazineSize && !reloading)
        {
            Reload();
        }

        if (shootPressed && readyToShoot && !reloading && ammoLeft > 0)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        readyToShoot = false;

        float x = Random.Range(-spreadX, spreadX);
        float y = Random.Range(-spreadY, spreadY);

        Vector3 shotDir = fpsCamera.transform.forward + new Vector3(x, y, 0);

        if (Physics.Raycast(fpsCamera.transform.position, shotDir, out rayHit, range, enemy))
        {
            Debug.Log(rayHit.collider.gameObject.name);
            if (rayHit.collider.CompareTag("Enemy"))
            {
                //get health component in target and damage enemy
            }
        }

        ammoLeft -= bulletsPerShot;
        Invoke("ResetShot", fireRate);

        if (bulletsPerShot > 1 && ammoLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
    }

    void ResetShot()
    {
        readyToShoot = true;
    }

    void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    void ReloadFinished()
    {
        ammoLeft = magazineSize;
    }
}
