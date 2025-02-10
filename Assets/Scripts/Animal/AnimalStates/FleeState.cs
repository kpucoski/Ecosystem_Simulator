using System.Linq;
using Animal.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace Animal.AnimalStates {
    public class FleeState : IAnimalState {
        private AbstractAnimal _rabbit;
        private AbstractAnimal _fox;
        private Collider foxCollider;

        public FleeState(AbstractAnimal animal) {
            _rabbit = animal;
        }

        public void Enter() {
            // Debug.Log($"{_rabbit.name} has started fleeing.");
            _rabbit.StartRunning();
        }

        public void setFox(Collider fox) {
            // _fox = fox;
            if (foxCollider == fox) return;
            foxCollider = fox;
            _fox = fox.GetComponent<Fox>();
        }

        public Collider getFox() {
            // return _fox;
            return foxCollider;
        }

        public void Execute() {
            if (_fox is null) {
                _rabbit.ChangeState(States.Wander); // Predator lost, return to wandering
                return;
            }
            
            // AvoidFox();
            Flee();
        }

        public void Exit() {
            // Debug.Log($"{_rabbit.name} has stopped fleeing.");
            _rabbit.StopRunning();
        }

        public States GetState() {
            return States.Flee;
        }

        void Flee() {
            if (_rabbit._transform is null) return;
            if (_fox._transform is null) return;
            
            if(_rabbit.isDrinking) {
                _rabbit.StopCoroutine("Drink");
                _rabbit.agent.isStopped = false;
            }
            if(_rabbit.isEating) {
                _rabbit.StopCoroutine("Eat");
                _rabbit.agent.isStopped = false;
            }
            
            Vector3 fleeDirection = (_rabbit._transform.position - _fox._transform.position).normalized;
            
            if(Random.value < 0.5)
                fleeDirection = Quaternion.Euler(0, Random.Range(-90f, 90f), 0) * fleeDirection;

            float fleeDistance = 10f;
            Vector3 fleePosition = _rabbit._transform.position + fleeDirection * fleeDistance;
            // float fleeDistance = Mathf.Clamp(20f / Vector3.Distance(_rabbit._transform.position, _fox._transform.position), 5f, 15f);
            // Vector3 fleePosition = _rabbit._transform.position + fleeDirection * fleeDistance;

            
            NavMeshHit hit;
            if (NavMesh.SamplePosition(fleePosition, out hit, fleeDistance, NavMesh.AllAreas)) {
                fleePosition = hit.position;
            }
            else {
                fleePosition = _rabbit._transform.position + (_rabbit._transform.position - _fox._transform.position).normalized * fleeDistance;
            }


            _rabbit.GoTo(fleePosition);
        }
    }
}