using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : PhysicsObject
{
    public Sea hitSea = null;

    BallCollision CollCode => GetComponent<BallCollision>();
    public float Radius => transform.localScale.x * 0.5f;

    public float VelocityOnSea(Sea sea)
    {
        return Mathf.Sqrt(2f * Physics.gravity.magnitude * (transform.position.y - sea.transform.position.y));
    }

    public float VelocityOnGround(Vector3 hitGround)
    {
        return Mathf.Sqrt(2f * Physics.gravity.magnitude * (transform.position.y - hitGround.y));
    }

    Vector3 ReflectionFunction(Vector3 vel, Vector3 normal, float energyDiss = 0)
    {
        Vector3 reflectVel = (vel - 1f * Vector3.Dot(Veloctiy, normal) * normal * (1f - energyDiss));
        return reflectVel;
    }

    private void BallGroundCheck()
    {
        if(Physics.Raycast(transform.position,Vector3.down, out RaycastHit hit, Radius))
        {
            Sea seaCode = hit.collider.GetComponent<Sea>();
            if(seaCode)
            {
                seaCode.Chock(this);
            }

        }
    }
    public void CarCollision(PhysicsObject boat, Vector3 contactPoint, Vector3 normal)
    {
        /*
        //Correct Position
        Vector3 sphereToSea = transform.position - boat.transform.position;
        float distance = Vector3.Dot(sphereToSea, normal);

        Vector3 sphereToProjection = distance * normal;
        Vector3 curTransform = transform.position + sphereToProjection;
        transform.position = curTransform;
        */


        Vector3 pointToBall = (transform.position - contactPoint).normalized;
        float dotProd = Vector3.Dot(pointToBall, RelativeVelocity(boat));
        Veloctiy = pointToBall * dotProd * (boat.GetComponent<Boat>().BoatSpeed * 12f);
    }


   void CorrectCollPosition(Vector3 ObjectPos, Vector3 normalPoint, Vector3 ClosePointOfColl)
    {
        //Distanace
        Vector3 distanceCheck = transform.position - ObjectPos;
        float distanceSum = Vector3.Dot(distanceCheck, normalPoint);

        //Projection
        Vector3 projectionVector = distanceSum * normalPoint;
        Vector3 projectionSum = transform.position + projectionVector;

        Veloctiy = projectionSum + normalPoint * 0.1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        hitSea = other.GetComponent<Sea>();
    }


    private void OnTriggerStay(Collider other)
    {
        hitSea = other.GetComponent<Sea>();
    }

    private void OnTriggerExit(Collider other)
    {
        Sea seaTest = other.GetComponent<Sea>();
        if (seaTest != null)
            hitSea = null;
    }

    private void Update()
    {
        BallGroundCheck();
        LimitVelocity();
        ApplyDynamicDrag();
    }

}
