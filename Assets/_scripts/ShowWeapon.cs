using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject pistol;

    private Animator anim;

    public bool showPistol;
    public AudioSource gunShot;

    public EnemyHealthSystem enemyHealthSystem;
    public Transform playerShootingPoint;
    public float nextFire;
    public float fireRate;

    private void Start()
    {
        showPistol = false;
        anim = GetComponentInChildren<Animator>();
        
    }

    private void Update()
    {
        ShowGun();
        UpdateAnimator();
        Shoot();
    }

    private void UpdateAnimator()
    {
        anim.SetBool("Pistol", showPistol);
        if(showPistol == true)
        {
            anim.SetLayerWeight(0, 0);
            anim.SetLayerWeight(1, 1);
        }
        if(showPistol == false)
        {
            anim.SetLayerWeight(1, 0);
            anim.SetLayerWeight(0, 1);
        }
    }

    private void ShowGun()
    {
        if (showPistol == false)
        {
            pistol.SetActive(false);
        }
        if (showPistol == true)
        {
            pistol.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && showPistol == false)
        {
            showPistol = true;

        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && showPistol == true)
        {
            showPistol = false;
        }
    }

    private void Shoot()
    {
        if(Input.GetButtonDown("Fire1") && showPistol == true)
        {
            RaycastHit enemyHit;
            Physics.Raycast(playerShootingPoint.position, playerShootingPoint.TransformDirection(Vector3.forward), out enemyHit, 2);

            if(enemyHit.transform.tag.Equals("Player") && Time.time > nextFire)
            {
                enemyHealthSystem.currentHP -= 20;
                nextFire = Time.time + fireRate;
                gunShot.Play();
            }
            
        }
    }
}
