using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steering
{
    [RequireComponent(typeof(Steering))]
    public class SimulationBrain : MonoBehaviour
    {
        public enum State { Flee, Pursue, Attack, Wander, Approach, Idle}
        public State currentState;


        [Header("Private")]
        private Steering steering;
        private float size;
        private GameObject target;

        float targetDistance = float.MaxValue; //set the distance to the highest possible number;

        [Header("settings")]
        [SerializeField] private SimulatorBrainSettings settings;


        [Header("Steering Settings")]
        [SerializeField] private SteeringSettings pursueSettings;
        [SerializeField] private SteeringSettings wanderSettings;
        [SerializeField] private SteeringSettings fleeSettings;

        private void Awake()
        {
            steering = GetComponent<Steering>();
            size = settings.size;
            currentState = State.Idle;
        }

        private void FixedUpdate()
        {
            FindTarget();

            SetBehavior();

            ScaleSize();
        }

        //uses the Physics.OverlapSphere function to get all object in view. checks which is the closesd and makes it the target
        public void FindTarget()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, settings.FOV, settings.FOVLayer);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject != gameObject)
                {
                    float distance = Vector3.Distance(transform.position, hitCollider.transform.position);

                    if (distance < targetDistance)
                    {
                        targetDistance = distance;
                        target = hitCollider.gameObject;
                    }
                }
            }
        }

        //sets the right behavior
        private void SetBehavior()
        {
            //if a target nearby it gets the distance and checks if it is food or an other cube 
            if (target != null)
            {
                targetDistance = Vector3.Distance(transform.position, target.transform.position);

                // if it is a cube check if it bigger then this cube. if its smaller persue and attack it. if its a bigger cube run away
                if (target.GetComponent<SimulationBrain>() != null)
                {
                    float enemySize = target.GetComponent<SimulationBrain>().size;

                    if (enemySize < size)
                    {
                        ToPersue();
                    }
                    else if (enemySize >= size && targetDistance <= settings.FOV)
                    {
                        ToFlee();
                    }

                    if (targetDistance <= (settings.attackRange + (size / 2)) && enemySize < size)
                    {
                        Attack();
                    }
                }
                //if it is not a cube approach it
                else if (target.GetComponent<SimulationBrain>() == null)
                {
                    ToApproach();
                }
            }
            
            //if there no targets or if the target is out of view go wander and set the traget to null 
            if (target == null || targetDistance > settings.FOV)
            {
                target = null;
                ToWander();
            }
        }

        // changes the game object size to the size variable
        private void ScaleSize()
        {
            gameObject.transform.localScale = new Vector3(size, size, size);
        }

        // add size from the target to the cube. destroys the target object and resets the target distnace and target variables.
        private void Attack()
        {
            size = size + target.GetComponent<SimulationBrain>().size;
            Destroy(target.gameObject);
            target = null;
            targetDistance = float.MaxValue;
        }

        // add size to the cube. destroys the target object and resets the target distnace and target variables.
        private void EatFood()
        {
            size = size + settings.foodQuality;
            Destroy(target.gameObject);
            target = null;
            targetDistance = float.MaxValue;
        }

        // chanches the state to flee
        private void ToFlee()
        {
            if (currentState != State.Flee)
            {
                currentState = State.Flee;
                steering.settings = fleeSettings;

                List<IBehavior> behaviors = new List<IBehavior>();
                behaviors.Add(new Flee(target));
                steering.SetBehaviors(behaviors, "Flee");
            }
        }

        // chanches the state to persue
        private void ToPersue()
        {
            if (currentState != State.Pursue)
            {
                Debug.Log(gameObject.name + " is in persue of " + target.gameObject.name);

                currentState = State.Pursue;
                steering.settings = pursueSettings;

                List<IBehavior> behaviors = new List<IBehavior>();
                behaviors.Add(new Pursue(target));
                steering.SetBehaviors(behaviors, "Pursue");
            }
        }

        // chanches the state to wander
        private void ToWander()
        {
            if (currentState != State.Wander)
            {
                currentState = State.Wander;
                steering.settings = wanderSettings;

                List<IBehavior> behaviors = new List<IBehavior>();
                behaviors.Add(new Wander());
                steering.SetBehaviors(behaviors, "Wander");
            }
        }

        // chanches the state to approach and if the object gets closs to the food it will call the eatfood function
        private void ToApproach()
        {
            if (currentState != State.Approach)
            {
                currentState = State.Approach;
                steering.settings = wanderSettings;

                List<IBehavior> behaviors = new List<IBehavior>();
                behaviors.Add(new Seek(target));
                steering.SetBehaviors(behaviors, "Approach");
            }

            if (targetDistance <= (settings.attackRange + (size / 2)))
            {
                EatFood();
            }
        }

        private void OnDrawGizmos()
        {
            Support.DrawWireDisc(transform.position, settings.FOV, Color.cyan);
            Support.DrawWireDisc(transform.position, settings.attackRange, Color.red);
        }
    }
}

