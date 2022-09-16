using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SimpleSteering
{
    [CreateAssetMenu(fileName = "Simple Steering Settings", menuName = "Steering/Simple Steering Settings", order = 2)]
    public class SimpleSteeringSettings : ScriptableObject
    {
        [Header("Steering Settings")]
        public float mass = 70;
        public float maxDesiredVelocity = 3;
        public float maxSteeringForce = 4;
        public float maxSpeed = 3;
    }
}
