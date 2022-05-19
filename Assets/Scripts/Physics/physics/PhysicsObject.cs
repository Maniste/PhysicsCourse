using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    [SerializeField]
    private Vector3 Vel = Vector3.zero;

    [SerializeField]
    private float maxVelocity = 0f;

    [SerializeField]
    private float DynamicDragCoef = 1.2f;

    [SerializeField]
    private bool bIsVerlet = false;

    [SerializeField]
    private bool useGravity = false;

    public Vector3 Veloctiy
    { get => Vel;
    set => Vel = new Vector3 (value.x, value.y, value.z); 
    }

    public float mass = 1f;

    protected void LimitVelocity()
    {
        Veloctiy = Veloctiy.normalized * Mathf.Min(Veloctiy.magnitude, maxVelocity);
    }

    private float DynamicDragAmount()
    {
        return Vector3.Dot(Veloctiy, Veloctiy) * DynamicDragCoef / 1000f;
    }


    protected void ApplyDynamicDrag()
    {
        Vector2 dragForce = -DynamicDragAmount() * Veloctiy;
        ApplyForce(dragForce);
    }

    protected virtual Vector3 RelativeVelocity(PhysicsObject other)
    {
        return other.Veloctiy - Veloctiy;
    }


    public void ApplyForce(Vector3 force)
    {
        Vector3 totalForce = useGravity ? force + mass * Physics.gravity : force;

        //force = mass * acc
        //acc = force / mass

        Vector3 accel = totalForce / mass;
        Intergrate(accel);
    }

    void Intergrate(Vector3 accel)
    {
        if(bIsVerlet)
        {
            transform.position += Veloctiy * Time.fixedDeltaTime + accel * Time.fixedDeltaTime * Time.fixedDeltaTime * 0.5f;
            Veloctiy += accel * Time.fixedDeltaTime * 0.5f;
        }else
        {
            //Euler
            Veloctiy = Vel + accel * Time.deltaTime;
            transform.position = transform.position + Veloctiy * Time.deltaTime;
        }

    }

}
