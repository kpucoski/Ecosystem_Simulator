using Animal.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace Animal.AnimalStates {
    public class EatState :  IAnimalState {
        private AbstractAnimal _animal;
        private WanderState w;

        public EatState(AbstractAnimal animal) {
            _animal = animal;
            w = new WanderState(_animal) {
                minRange = 15f,
                maxRange = 35f
            };
        }

        public void Enter() {
            // Debug.Log("Entering Eat State");
        }

        public void Execute() {
            if (!GoToPlant()) {
                w.Execute();
                if (_animal.WaterDetected() 
                    && _animal.IsThirsty 
                    && !_animal.isEating
                    && !_animal.isDrinking 
                    && _animal.HungerThirstDifference(6f)
                    )
                    _animal.ChangeState(States.Drink);
            }
        }

        public void Exit() {
            // Debug.Log("Exiting Eat State");
        }

        public States GetState() {
            return States.Eat;
        }
        
        private bool GoToPlant() {
            var plants = _animal.Detect("Grass",detectionMask:_animal.grassMask);
            if (plants.Count <= 0) return false;

            // var targetPlant = plants[0].transform;
            var targetPlant = plants[0].GetComponent<AbstractPlant>();
            // Debug.Log($"Plant detected at {targetPlant.position}. Moving to eat.");
            
            if (!IsPositionAccessible(targetPlant._transform.position, 1f)) 
                return false;
            _animal.GoTo(targetPlant._transform.position);
            // if (Vector3.Distance(_animal._transform.position, targetPlant._transform.position) <= 3f) {
            //    EatPlant(targetPlant);
            // }
            if (targetPlant._collider.bounds.Intersects(_animal._collider.bounds)) {
                EatPlant(targetPlant);
            }

            return true;
        }

        void EatPlant(AbstractPlant targetPlant) {
            if (_animal.isEating || _animal.isDrinking || targetPlant.isEaten) return;
            _animal.StartCoroutine(_animal.Eat(targetPlant.foodValue));
            targetPlant.Eat();
        }
        
        private bool IsPositionAccessible(Vector3 position, float maxDistance) {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(position, out hit, maxDistance, NavMesh.AllAreas)) {
                NavMeshPath path = new NavMeshPath();
                if (_animal.agent.CalculatePath(hit.position, path) && path.status == NavMeshPathStatus.PathComplete) {
                    return true; 
                }
            }
            return false;
        }
    }
}