using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Shooter : MonoBehaviour
{
    //============== Unity Inspector ==============
    [Header("Requirements")] 
    [SerializeField] private GameObject bulletGameObject;
    [SerializeField] private Transform directionTransform;

    [SerializeField] private GameObject body;
    //==============================================
    void FixedUpdate()
    {
        //space or click
        if (Input.GetKey(KeyCode.Space) || Input.GetMouseButtonDown(0))
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
        }
    }
}
