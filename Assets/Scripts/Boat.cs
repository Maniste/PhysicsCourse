using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : PhysicsObject
{

    [SerializeField] private float boatSpeed = 1f;
    public float BoatSpeed
    { get { return boatSpeed; } }

    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private float turnSpeed = 1f;
    [SerializeField] private float turningForceCoef = 1f;
    [SerializeField] private float groundCheckLenght = 1f;

    public GameObject childObj => transform.GetChild(0).gameObject;

    private bool grounded = false;

    private BoxCollider boxColl = null;
    public LayerMask GroundLayer;

    // Start is called before the first frame update
    void Awake()
    {
        boxColl = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Ball Layer
        if (other.gameObject.layer == 8)
        {
            Vector3 normal = (other.transform.position - transform.position).normalized;
            other.GetComponent<Ball>().CarCollision(this.GetComponent<PhysicsObject>(), ContactPoint(boxColl, other.transform.position), normal);
            Debug.Log("hit");
        }
        else if (other.gameObject.layer == 9)
        {
            //transform.position = CorrectPosition(other);
            Vector3 normal = (transform.position - other.transform.position);
            normal.y = Veloctiy.y;

            InverseRelativeVelocity(other, Reflect(RelativeVelocity(other), other.ClosestPoint(transform.position).normalized, 0.0f));

            /*
            Vector3 dir = (transform.position - other.transform.position).normalized;
            dir.y = Veloctiy.y;
            Veloctiy = dir * Veloctiy.magnitude;
            Debug.Log("hit wall");
        
            */
        }
    }

    private Vector3 CorrectPosition(Collider hitWall)
    {

        return Projection(hitWall) + transform.up * 2f;

    }
    Vector3 Projection(Collider hitWall)
    {
        Vector3 sphereToProjection = Distance(hitWall) * transform.up;
        return hitWall.transform.position + sphereToProjection;
    }

    float Distance(Collider hitWall)
    {
        Vector3 sphereToSea = transform.position - hitWall.transform.position;
        return Vector3.Dot(sphereToSea, transform.up);
    }

    private Vector3 ContactPoint(Collider coll, Vector3 point)
    {
        return coll.ClosestPoint(point);
    }

    public void InverseRelativeVelocity(Collider wall, Vector3 vel)
    {
        this.Veloctiy = vel + 1f * Vector3.zero;
    }

    Vector3 Reflect(Vector3 v, Vector3 normal,float energyDissipation = 0f)
    {
        Vector3 r = (v - 2f * Vector3.Dot(v, normal) * normal) * (1f - energyDissipation);
        return r;
    }

    Vector3 RelativeVelocity(Collider wall)
    {
        //Debug.Log("Sphere Vel: " + other.Velocity + " Car Vel: " + ParentVelocity);
        return this.Veloctiy;
    }

    private bool IsGrounded()
    {
        if (Physics.Raycast(childObj.transform.position, -childObj.transform.up, out RaycastHit hit, groundCheckLenght))
        {
            if (hit.collider.gameObject.layer == 10)
                return true;
        }

        return false;
    }

    private void CorrectBoatGroundPosition()
    {
        if (Physics.Raycast(childObj.transform.position, -childObj.transform.up, out RaycastHit hit, 2f))
        {
            if (hit.collider.gameObject.layer == 10)
            {
                /*
                Debug.Log(hit.collider);
                Vector3 trans = transform.position;
                trans.y = hit.point.y;
                transform.position = trans;
                */
                Vector3 newVel = Veloctiy;
                newVel.y = 0f;
                Veloctiy = newVel;
            }    
        }

        Debug.DrawRay(childObj.transform.position, -childObj.transform.up * groundCheckLenght, Color.red);
    }

    private void Update()
    {
        CorrectBoatGroundPosition();
        grounded = IsGrounded();

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Debug.Log("Jumped");
            ApplyForce(transform.up * jumpForce);
        }

        transform.Rotate(transform.up * turnSpeed * Input.GetAxis("Horizontal") * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if(!grounded)
            ApplyForce(-transform.up + Physics.gravity * mass);

        LimitVelocity();
        ApplyForce(transform.right * Input.GetAxis("Horizontal") * turningForceCoef * Veloctiy.magnitude);
        ApplyForce(transform.forward * boatSpeed * Input.GetAxisRaw("Vertical"));
    }
}
