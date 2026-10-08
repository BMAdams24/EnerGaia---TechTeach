using System;
using Unity.VisualScripting;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [Header("Activation Settings")]
    public float requiredImpactForce = 8f;


    [Header("Visual Feedback")]
    public Color inactiveColor = Color.red;
    public Color activeColor = Color.green;

    private SpriteRenderer sr;
    private bool isActivated = false;


    [Header("Timing")]
    public float activeDuration = 10f; //10 second countdown timer for Pressure Plate when pressed

    private float activeTimer;


    [Header("Connected Objects")]
    public Door connectedDoor; //Connects to Door.cs

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = inactiveColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActivated)
        {
            return; //If already activated, do nothing lol
        }

        Rigidbody2D rb = collision.attachedRigidbody;

        if (rb != null)
        {
            float impactForce = rb.linearVelocity.magnitude; //Gives total Kinetic Energy magnitude in any direction.

            if (impactForce >= requiredImpactForce) //If more than threshold(8f), it will activate
            {
                ActivatePlate();
            }
        }
    }

    void ActivatePlate() //Sets color to green and Dublug Log tells us its activated
    {
        isActivated = true;
        activeTimer = activeDuration;
        sr.color = activeColor;

        Debug.Log("Pressure Plate Activated");

        if (connectedDoor != null)
        {
            connectedDoor.OpenDoor();
        }
    }

    void Update()
    {
        if (isActivated) //If pressure plate is actively pressed (green)
        {
            activeTimer -= Time.deltaTime; //Start countdown timer

            if (activeTimer <= 0)
            {
                DeactivatePlate(); //Once timer reaches zero, deactivate the pressure plate
            }
        }
    }

    void DeactivatePlate()
    {
        isActivated = false;
        sr.color = inactiveColor;

        Debug.Log("Pressure Plate is Deactivated");

        if (connectedDoor != null)
        {
            connectedDoor.CloseDoor();
        }
    }
}
