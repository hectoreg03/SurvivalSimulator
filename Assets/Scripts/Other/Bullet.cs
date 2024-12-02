using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] 
public class Bullet : MonoBehaviour
{
    //================ Unity Editor ================
    [Header("Parameters")]
    [Range(10.0f, 100f)]
    [SerializeField] private float bulletSpeed;
    //==============================================
    private Rigidbody rb;
    private GameObject shooter;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        rb.AddRelativeForce(Vector3.forward * bulletSpeed, ForceMode.Impulse);

        if (rb.velocity.magnitude > bulletSpeed) rb.velocity = rb.velocity.normalized * bulletSpeed;
        
        if(this.transform.position.y < 0 || this.transform.position.y > 25) Destroy(this.gameObject);
    }

    public void SetShooter(GameObject shooter)
    {
        this.shooter = shooter;
    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Zombie"))
        {
            try
            {
                other.GetComponent<ZombieController>().Damage();
            }
            catch (MissingComponentException)
            {
                Debug.Log("Missing Zombie Controller");
            }
        }
        else if (other.gameObject.CompareTag("Survivor") || other.gameObject.CompareTag("Bullet")) return;
        
        Destroy(this.gameObject);
    }
}
