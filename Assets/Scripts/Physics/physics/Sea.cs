using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sea : MonoBehaviour
{
    Vector3 Normal => transform.up;

    [SerializeField]
    private Ball[] BallsInScene;

    private void Awake()
    {
        Ball[] foundObjs = GameObject.FindObjectsOfType<Ball>();
        foreach (Ball balls in foundObjs)
        {
            BallsInScene = foundObjs;
        }
    }

    private void Update()
    {
        CheckBalls();
    }

    float Distance(Ball ballObj)
    {
        Vector3 sphereToSea = transform.position - ballObj.transform.position;
        return Vector3.Dot(sphereToSea, Normal);
    }

    bool isColliding(PhysicsObject ball)
    {
        if (WillBeCollision(ball) == false)
            return false;


        return Distance(ball.GetComponent<Ball>()) >= 0f || 
            Mathf.Abs(Distance(ball.GetComponent<Ball>())) <= ball.GetComponent<Ball>().Radius;
    }
    bool WillBeCollision(PhysicsObject ball)
    {
        //Debug.Log("WillBeCollision: " + name);
        //return Vector3.Dot(sphere.Velocity, Normal) < 0f;
        return Vector3.Dot(RelativeVelocity(ball.GetComponent<Ball>()), Normal) < 0f;
    }

    bool isBallStationary(Ball ballObj)
    {
        bool lowVel = RelativeVelocity(ballObj).magnitude < 0.2f;
        return lowVel && TouchingSea(ballObj);
    }

    bool TouchingSea(Ball ballObj)
    {
        float deltaMove = Mathf.Max(0.5f * ballObj.Radius, RelativeVelocity(ballObj).magnitude * Time.fixedDeltaTime);
        return (CorrectedPosition(ballObj) - ballObj.transform.position).magnitude <= 5f * deltaMove;
    }


    Vector3 CorrectedPosition(Ball ballObj)
    {
        return Projection(ballObj) + Normal * ballObj.Radius;
    }

    Vector3 Projection(Ball ballObj)
    {
        Vector3 sphereToProjection = Distance(ballObj) * Normal;
        return ballObj.transform.position + sphereToProjection;
    }

    Vector3 Reflect(Vector3 v, float energyDissipation = 0f)
    {
        Vector3 r = (v - 2f * Vector3.Dot(v, Normal) * Normal) * (1f - energyDissipation);
        return r;
    }

    Vector3 RelativeVelocity(Ball other)
    {
        //Debug.Log("Sphere Vel: " + other.Velocity + " Car Vel: " + ParentVelocity);
        return other.Veloctiy - Vector3.zero;
    }

    public void Chock(Ball ballObj, float energyDissipation = 0f)
    {
        if (isColliding(ballObj.GetComponent<PhysicsObject>()) == false)
            return;

        if (isBallStationary(ballObj))
        {
            ballObj.transform.position = CorrectedPosition(ballObj);
            ballObj.ApplyForce(-ballObj.mass * Physics.gravity);
            Debug.Log("A");
        }
        else //Is dynamic
        {
            ballObj.transform.position = CorrectedPosition(ballObj);
            InverseRelativeVelocity(ballObj, Reflect(RelativeVelocity(ballObj), energyDissipation));
        }

    }


    public void InverseRelativeVelocity(PhysicsObject other, Vector3 vel)
    {
        if (other)
            other.Veloctiy = vel + 2f * Vector3.zero;
    }

    private void CheckBalls()
    {
        foreach (Ball balls in BallsInScene)
        {
            if (balls.hitSea == this)
                Chock(balls);
        }
    }
}
