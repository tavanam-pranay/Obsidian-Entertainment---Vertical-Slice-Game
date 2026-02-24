using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] private float dampening;
    [SerializeField] private Vector3 offset; //Vector3 because we need z offset.

    private Vector3 velocity = Vector3.zero; //SmoothDamp requires a velocity variable to be passed by reference, so we need to store it here
    public Transform target;


    private void FixedUpdate()
    {
        Vector3 targetPosition = target.position;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition + offset, ref velocity, dampening);
    }


}
