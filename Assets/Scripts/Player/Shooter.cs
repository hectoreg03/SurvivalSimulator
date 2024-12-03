using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Shooter : MonoBehaviour
{
    //============== Unity Inspector ==============
    [Header("Requirements")] 
    [SerializeField] private GameObject bulletGameObject;
    [SerializeField] private Transform directionTransform;
    [SerializeField] private TMP_Text bulletIndicator;
    [SerializeField] private GameObject body;

    [Header("Parameters")] 
    [SerializeField] private int magazineCapacity;
    [SerializeField] private float reloadingTime;
    [SerializeField] private float cooldownTime;
    //==============================================

    public int bullets;
    public bool reloading;
    public bool coolingDown;

    void Start()
    {
        bullets = magazineCapacity;
        UpdateBullets();
    }
    void FixedUpdate()
    {
        //space or click
        if (Input.GetKey(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (reloading || coolingDown || bullets < 1) return;
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletGameObject);
        bullet.transform.rotation = directionTransform.rotation;
        bullet.transform.position = directionTransform.position;

        try
        {
            bullet.GetComponent<Bullet>().SetShooter(body);
        }
        catch (Exception)
        {
            Destroy(bullet);
        }

        bullets--;
        UpdateBullets();
        if (bullets > 0) StartCoroutine(Cooldown());
        else StartCoroutine(Reload());
    }

    void UpdateBullets()
    {
        bulletIndicator.SetText(bullets.ToString());
    }

    IEnumerator Reload()
    {
        reloading = true;
        bulletIndicator.SetText("...");
        yield return new WaitForSeconds(reloadingTime);
        bullets = magazineCapacity;
        UpdateBullets();
        reloading = false;
    }

    IEnumerator Cooldown()
    {
        coolingDown = true;
        yield return new WaitForSeconds(cooldownTime);
        coolingDown = false;
    }   
}
