using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    //================== Unity Inspector ==============
    [Header("Parameters")]
    [Range(5f, 20f)]
    [SerializeField] private float speed;
    [Range(10f, 50f)]
    [SerializeField] private float maxSpeed;
    //=================================================
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //-------------------  WASD Controls -----------------------------
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            rb.AddRelativeForce(Vector3.forward * speed);
        
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            rb.AddRelativeForce(Vector3.left * speed);
        
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            rb.AddRelativeForce(Vector3.back * speed);

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            rb.AddRelativeForce(Vector3.right * speed);
        //-----------------------------------------------------------------
        
        //Sets the maximum velocity to the maxSpeed
        if (rb.velocity.magnitude > maxSpeed) rb.velocity = rb.velocity.normalized * maxSpeed;
    }

}
