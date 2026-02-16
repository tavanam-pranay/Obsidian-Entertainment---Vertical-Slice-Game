using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralWalkFootSolver : MonoBehaviour 
{
    [SerializeField] LayerMask terrainLayer = default;
    [SerializeField] Transform body = default;
    [SerializeField] ProceduralWalkFootSolver otherFoot = default;
    [SerializeField] float speed = 1f;
    [SerializeField] float checkDistance = 4f;
    [SerializeField] float stepLength = 4f;
    [SerializeField] float stepHeight = 1f;
    [SerializeField] Vector3 footOffset = default;

    float footSpacing;
    Vector3 oldPosition;
    Vector3 currentPosition;
    Vector3 newPosition;
    Vector3 oldNormal;
    Vector3 currentNormal;
    Vector3 newNormal;

    float lerp; // Lerp value (0.0 - 1.0) for tracking foot movement

    /*
     * Most of this code is taken from https://learn.unity.com/course/prototyping-a-procedural-animated-boss/tutorial/workshop-video-procedural-walker?version=2020.2
     * I have adapted it where version differences occured, and over-commented it for our understanding. The original code is written by Unity's own tutorial team, and is licensed under the Unity Terms of Service (https://unity3d.com/legal/terms-of-service)
     */



    // Start is called before the first frame update
    void Start()
    {
        footSpacing = transform.localPosition.x; //Position along X relative to body (parent)
        currentPosition = newPosition = oldPosition = transform.position; //Set all positions to start pos
        currentNormal = newNormal = oldNormal = transform.up; //Set all normals to start normal
        lerp = 1f;
    }

    void Update()
    {
        transform.position = currentPosition; //Set foot position to last calculated position
        transform.up = currentNormal; //Set foot normal to last calculated normal

        Ray ray = new Ray(body.position + (body.right * footSpacing), Vector3.down); //Raycast down from body, offset by foot spacing

        if (Physics.Raycast(ray, out RaycastHit info, 10f, terrainLayer.value)) //If ray hits terrain (Check if RayCastHit info contains terrain layermask over 10 units)
        {
            if (Vector3.Distance(newPosition, info.point) > checkDistance && !otherFoot.IsMoving() && lerp >= 1) //If enough distance between raycast hit point and new position, other foot isn't moving, and this foot isn't already moving:
            {
                lerp = 0f; //Reset animation to start of cycle
                int direction = body.InverseTransformPoint(info.point).z > body.InverseTransformPoint(newPosition).z ? 1 : -1; //Determine direction of step based on whether raycast hit point is in front or behind foot (By converting world positions to local space and comparing Z values) (? and : are a shorthand for if-else)

                newPosition = info.point + (body.forward * stepLength * direction) + footOffset; //Set new position to hit point, offset by step length in direction, and foot offset
                newNormal = info.normal; //Set new normal to terrain normal at hit point
            }
        }

        if (lerp < 1) // If foot is in the process of moving
        {
            Vector3 tempPosition = Vector3.Lerp(oldPosition, newPosition, lerp);  // Interpolate between old and new ground position while lerp is less than 1
            tempPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight; // Increase Y position based on completion percentage of step (Using sine wave for up and down motion)

            currentPosition = tempPosition; // Set current position to the calculated position
            currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp); // Interpolate between old and new normals for foot rotation
            lerp += Time.deltaTime * speed; // Increment lerp value based on time and speed
        }

        else // Once foot has finished moving
        {
            oldPosition = newPosition; // Set oldPosition to new position for next step
            oldNormal = newNormal; // Set oldNormal to new normal for next step
        }

    } // End of Update()

    public bool IsMoving()
    {
        return lerp < 1;
    }
}
