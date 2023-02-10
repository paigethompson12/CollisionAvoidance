using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoidance : SteeringBehavior
{
    public Kinematic character;
    public float maxAccel = 1f;
    public Kinematic[] targetArray;

    float radius = .1f; 

    public override SteeringOutput getSteering()
    {
        float shortestTime = float.PositiveInfinity; //this is so the shortest time could be updated looking at the time to collision

        Kinematic firstTarget = null;
        float firstMinSeparation = float.PositiveInfinity; //initializing all these to be infinity so they can be updated
        float firstDistance = float.PositiveInfinity;
        Vector3 firstRelativePos = Vector3.positiveInfinity;
        Vector3 firstRelativeVel = Vector3.zero;

        // loop through each target
        Vector3 relativePos = Vector3.positiveInfinity; //in my case, it's all the other red blocks
        foreach (Kinematic target in targetArray)
        {
            relativePos = target.transform.position - character.transform.position; //get the relative position in relation to the target
            Vector3 relativeVel = character.linearVelocity - target.linearVelocity; //same as above but for velocity
            float relativeSpeed = relativeVel.magnitude;
            float timeToCollision = (Vector3.Dot(relativePos, relativeVel) / (relativeSpeed * relativeSpeed)); //getting the estimated collision time based on the relatives above

            // check if it is going to be a collision at all
            float distance = relativePos.magnitude;
            float minSeparation = distance - relativeSpeed * timeToCollision;
            if (minSeparation > 2*radius)
                continue; 

            if (timeToCollision > 0 && timeToCollision < shortestTime)
            {
                shortestTime = timeToCollision;  //update all these variables if the time to collision is near
                firstTarget = target;
                firstMinSeparation = minSeparation;
                firstDistance = distance;
                firstRelativePos = relativePos;
                firstRelativeVel = relativeVel;
            }
        }

        if (firstTarget == null)
            return null;

        //generate a new steering output to update the linear velocity in respect to the first target's transsform
        SteeringOutput result = new SteeringOutput();

        float dotResult = Vector3.Dot(character.linearVelocity.normalized, firstTarget.linearVelocity.normalized);
        if (dotResult < -0.9) 
            result.linear = -firstTarget.transform.right;
        else
            result.linear = -firstTarget.linearVelocity;
        result.linear.Normalize();
        result.linear *= maxAccel;
        result.angular = 0;
        return result;
    }
}
