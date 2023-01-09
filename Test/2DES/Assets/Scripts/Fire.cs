using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject projectile;
    public float fireRate;
    public float speed;
    public float range;
    public float reloadTime;
    public int clipSize;
    public int currentClip;
    private float fireTimer;
    private bool isReloading;


    // Start is called before the first frame update
    void Start()
    {
        currentClip = clipSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && currentClip > 0 && !isReloading && fireTimer > fireRate)
        {
            Schuss();
            fireTimer = 0;
            currentClip--;
        }

        if (Input.GetButtonDown("Reload") && currentClip < clipSize && !isReloading)
        {
            StartCoroutine(Reload());
        }

        if (fireTimer < fireRate)
        {
            fireTimer += Time.deltaTime;
        }
    }

    void Schuss()
    {
        GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
        newProjectile.GetComponent<Rigidbody2D>().velocity = transform.right * speed;
        Destroy(newProjectile, range);
    }

    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentClip = clipSize;
        isReloading = false;
    }
}
