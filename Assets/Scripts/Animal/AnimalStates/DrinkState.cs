using System.Linq;
using Animal.Interfaces;
using UnityEngine;

namespace Animal.AnimalStates {
    public class DrinkState :  IAnimalState {
        private AbstractAnimal _animal;
        private WanderState w;

        public DrinkState(AbstractAnimal animal) {
            _animal = animal;
            w = new WanderState(_animal) {
                minRange = 15f,
                maxRange = 35f
            };
        }

        public void Enter() {
            // Debug.Log("Entering Drink State");
        }

        public void Execute() {
            if (!GoToWater()) {
                w.Execute();
                if (_animal is Rabbit) {
                    if (_animal.GrassDetected() 
                        && _animal.IsHungry 
                        && !_animal.isEating 
                        && !_animal.isDrinking 
                        && _animal.HungerThirstDifference(6f)
                        )
                        _animal.ChangeState(States.Eat);
                }
                else {
                    if (_animal.RabbitDetected() 
                        && _animal.IsHungry 
                        && !_animal.isEating 
                        && !_animal.isDrinking
                        && _animal.HungerThirstDifference(6f)
                        )
                        _animal.ChangeState(States.Hunt);
                }
            }
        }

        public void Exit() {
            // Debug.Log("Exiting Drink State");
        }

        public States GetState() {
            return States.Drink;
        }
        
        private bool GoToWater() {
            var waterColliders = _animal.Detect("Water Point",_animal.visionRadius*4f,_animal.waterPointMask);
            foreach (var waterPoint in waterColliders) {
                if (!_animal.waterSources.Contains(waterPoint)) {
                    var source = new WaterSource(waterPoint, _animal.forgetWaterTime);
                    _animal.waterSources.Add(waterPoint, source);
                }
            }
            
            if (_animal.waterSources.Count <= 0) return false;

            var list = _animal.waterSources.Values
                .Cast<WaterSource>()
                .OrderBy(c => Vector3.Distance(_animal._transform.position,c.waterCollider.bounds.ClosestPoint(_animal._transform.position)))
                .ToList();
            
            Collider target = list[0].waterCollider;
            Vector3 closestPoint = target.bounds.ClosestPoint(_animal._transform.position);
            // Debug.Log($"Water detected at {closestPoint}. Moving to drink.");
            _animal.GoTo(closestPoint);

            // if (Vector3.Distance(_animal._transform.position, closestPoint) <= 4f) {
            //     Drink();
            // }
            
            if (target.bounds.Intersects(_animal._collider.bounds)) {
                Drink();
            }

            return true;
        }

        void Drink() {
            if (_animal.isDrinking || _animal.isEating) return;
            _animal.StartCoroutine(_animal.Drink(_animal.thirst));
        }
    }
}