using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraMovement : MonoBehaviour
{
    //=================== UNITY INSPECTOR ===============
    [Header("Requirements")]
    [SerializeField] private GameObject camera;
    
    [Header("Parameters")]
    [Range(5.0f, 20.0f)]
    [SerializeField] private float sensibilityX;
    [Range(5.0f, 20.0f)]
    [SerializeField] private float sensibilityY;

    [SerializeField] private bool invertVerticalAxis;
    //====================================================
    
    
    private float xRotation;
    private float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * sensibilityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensibilityY;

        xRotation -= invertVerticalAxis ? -mouseY : mouseY ;
        yRotation += mouseX;

        xRotation = Mathf.Clamp(xRotation, -80.0f, 45.0f);
        
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
        camera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        
        
    }
}
