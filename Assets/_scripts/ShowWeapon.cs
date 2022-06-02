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

    private void Start()
    {
        showPistol = false;
        anim = GetComponentInChildren<Animator>();
        
    }

    private void Update()
    {
        ShowGun();
        UpdateAnimator();
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
        if(Input.GetButton("Fire1"))
        {
            gunShot.Play();
        }
    }
}
