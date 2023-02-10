using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance : Seek
{
    //the minimum distance that the character should be avoiding
    float avoidDistance = 50f; 

    // The distance to look ahead for a collision, i lengthened this a bit
    float lookahead = 15f; 

    protected override Vector3 getTargetPosition()
    {
        // cast a ray to see if there is something in the distance
        RaycastHit hit; 
        if (Physics.Raycast(character.transform.position, character.linearVelocity, out hit, lookahead))
        {
            Debug.DrawRay(character.transform.position, character.linearVelocity.normalized * hit.distance, Color.red, 1f);
            Debug.Log("Hit " + hit.collider);
            return hit.point - (hit.normal * avoidDistance); //red if it's hitting an obstacle
        }
        else
        {
            Debug.DrawRay(character.transform.position, character.linearVelocity.normalized * lookahead, Color.blue, 1f); //blue if it's not hitting an obstacle
            return base.getTargetPosition();
        }
    }

}
