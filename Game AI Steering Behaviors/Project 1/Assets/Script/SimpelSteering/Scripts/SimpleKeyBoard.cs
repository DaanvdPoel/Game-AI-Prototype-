using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleSteering
{
    public class SimpleKeyBoard : MonoBehaviour
    {
        [Header("Steering Settings")]
        [SerializeField] private SimpleSteeringSettings settings;

        [Header("Steering Runtime")]
        [SerializeField] private Vector3 position = Vector3.zero;
        [SerializeField] private Vector3 positionTarget = Vector3.zero;
        [SerializeField] private Vector3 velocity = Vector3.zero;
        [SerializeField] private Vector3 velocityDesired = Vector3.zero;
        [SerializeField] private Vector3 steering = Vector3.zero;

        private void Start()
        {
            position = transform.position;
        }

        private void Update()
        {
            Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

            if (direction != Vector3.zero)
            {
                positionTarget = position + direction.normalized * settings.maxDesiredVelocity;
            }
            else
            {
                positionTarget = position;
            }
        }

        private void FixedUpdate()
        {
            velocityDesired = (positionTarget - position).normalized * settings.maxDesiredVelocity;
            Vector3 steeringForce = velocityDesired - velocity;

            steering = Vector3.zero;
            steering += steeringForce;

            steering = Vector3.ClampMagnitude(steering, settings.maxSteeringForce);
            steering /= settings.mass;

            velocity = Vector3.ClampMagnitude(velocity + steering, settings.maxSpeed);
            position += velocity * Time.fixedDeltaTime;

            transform.position = position;
            transform.LookAt(position + Time.fixedDeltaTime * velocity);
        }

        private void OnDrawGizmos()
        {
            Support.DrawRay(transform.position, velocity, Color.red);
            Support.DrawRay(transform.position, velocityDesired, Color.blue);
            Support.DrawLabel(transform.position, name, Color.white);
        }

    }
}

