using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollision : MonoBehaviour
{

    public PhysicsObject lastHitObj = null;

    public void Collided(PhysicsObject ObjA, PhysicsObject ObjB)
    {

        if (IsLastColider(ObjA) || IsLastColider(ObjB))
            return;

        ObjA.GetComponent<BallCollision>().lastHitObj = ObjB;
        ObjB.GetComponent<BallCollision>().lastHitObj = ObjA;

        float dot = Vector3.Dot(ObjA.Veloctiy, ObjB.Veloctiy);
        float dist = Vector3.Distance(ObjA.transform.position, ObjB.transform.position);
        float radiusSum = ObjA.GetComponent<Ball>().Radius + ObjB.GetComponent<Ball>().Radius;

        if (dist > radiusSum)
            return;


        Vector3 aToB = (ObjB.transform.position - ObjA.transform.position).normalized;

        Vector3 pushFromA = radiusSum * aToB;

        ObjB.transform.position = ObjA.transform.position + 1f * pushFromA;
        TwoDReflection(ObjA, ObjB);
    }

    private bool IsLastColider(PhysicsObject hitObj)
    {
        if (lastHitObj == null)
            return false;

        return hitObj == lastHitObj;
    }

    private void TwoDReflection(PhysicsObject ObjA, PhysicsObject ObjB)
    {
        float totalMass = ObjA.mass + ObjB.mass;

        float doubleMassA = ObjA.mass * 2f;
        float doubleMassB = ObjB.mass * 2f;

        float doubleAOverSum = doubleMassA / totalMass;
        float doubleBOverSum = doubleMassB / totalMass;


        Vector3 vAMinusVB = ObjA.Veloctiy - ObjB.Veloctiy;
        Vector3 vBMinusVA = -vAMinusVB;

        Vector3 postAminusPosB = ObjA.transform.position - ObjB.transform.position;
        Vector3 posBMinusPosA = -postAminusPosB;

        float posesSquare = Vector3.Dot(postAminusPosB, postAminusPosB);

        float dotA = Vector3.Dot(vAMinusVB, postAminusPosB);
        float dotB = Vector3.Dot(vBMinusVA, posBMinusPosA);

        ObjA.Veloctiy -= doubleBOverSum * (dotA / posesSquare) * postAminusPosB;
        ObjB.Veloctiy -= doubleAOverSum * (dotB / posesSquare) * posBMinusPosA;
    }

}
