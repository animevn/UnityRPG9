using System.Collections.Generic;
using Script.Core;
using Script.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace Script.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] private float maxSpeed = 5.66f;
        // ReSharper disable once InconsistentNaming
        private NavMeshAgent navMeshAgent;
        private Health health;

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        private void Update()
        {
            if (health.IsDead()) navMeshAgent.enabled = false;
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            var velocity = navMeshAgent.velocity;
            var localVelocity = transform.InverseTransformDirection(velocity);
            var speed = localVelocity.z;
            // ReSharper disable once Unity.PreferAddressByIdToGraphicsParams
            GetComponent<Animator>().SetFloat("forward", speed);
        }
        
        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

//        //dictionary way to save data
//        public object CaptureState()
//        {
//            var objectTransform = transform;
//            var data = new Dictionary<string, object>
//            {
//                ["position"] = new SerializableVector3(objectTransform.position),
//                ["rotation"] = new SerializableVector3(objectTransform.eulerAngles)
//            };
//
//            return data;
//        }
//
//        public void RestoreState(object state)
//        {
//            var data = (Dictionary<string, object>) state;
//            var position = (SerializableVector3) data["position"];
//            var rotation = (SerializableVector3) data["rotation"];
//            GetComponent<NavMeshAgent>().enabled = false;
//            transform.position = position.ToVector();
//            transform.eulerAngles = rotation.ToVector();
//            GetComponent<NavMeshAgent>().enabled = true;
//        }


        //struct way
        
        [System.Serializable]
        private struct MoverData
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation;
        }
        
        public object CaptureState()
        {
            var objectTransform = transform;
            var data = new MoverData
            {
                position = new SerializableVector3(objectTransform.position),
                rotation = new SerializableVector3(objectTransform.eulerAngles)
            };
            return data;
        }

        public void RestoreState(object state)
        {
            var data = (MoverData) state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = data.position.ToVector();
            transform.eulerAngles = data.rotation.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}