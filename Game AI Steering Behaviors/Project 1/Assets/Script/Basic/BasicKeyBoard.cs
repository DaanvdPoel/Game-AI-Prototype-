using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleSteering
{

    public class BasicKeyBoard : MonoBehaviour
    {
        [SerializeField] private float Speed;
        private Vector3 velocity;

        void FixedUpdate()
        {
            Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

            velocity = direction.normalized * Speed;

            transform.position += velocity * Time.fixedDeltaTime;

            transform.LookAt(transform.position + velocity);
        }

        private void OnDrawGizmos()
        {
            Support.DrawRay(transform.position, velocity, Color.red);
            Support.DrawLabel(transform.position, name, Color.white);
        }

    }
}
