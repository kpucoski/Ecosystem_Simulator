using System;
using System.Collections.Generic;
using System.Linq;
using Animal.Interfaces;
using UnityEngine;

namespace Animal.AnimalStates {
    public class SeekState : IAnimalState {
        private AbstractAnimal _animal;
        public float range; //radius of sphere
        public Transform centrePoint; //centre of the a
        private WanderState w;

        public SeekState(AbstractAnimal animal) {
            _animal = animal;
            w = new WanderState(_animal) {
                minRange = 15f,
                maxRange = 35f
            };
        }

        public void Enter() {
            // Debug.Log("Entering Seek State");
        }

        public void Execute() {
            // if (!DetermineAction()) {
            //     // if no plant is found transition back to Wander state
            //     w.Execute();
            // }
        }

        public void Exit() {
            // Debug.Log("Exiting Seek State");
        }

        public States GetState() {
            return States.Seek;
        }

        // private bool DetermineAction() {
        //     // var priority = _animal.DeterminePriority();
        //     // if (priority == Needs.Thirst) {
        //     //     _animal.ChangeState();
        //     //     return true;
        //     // }
        //     //
        //     // return _animal is Rabbit && SeekPlant();
        //     var isRabbit = _animal is Rabbit;
        //     
        //     // if (!_animal.WaterDetected() || 
        //     //     (isRabbit && !_animal.GrassDetected()) || 
        //     //     (!isRabbit && !_animal.RabbitDetected())) 
        //     //     return false;
        //     
        //     if (_animal.WaterDetected() && _animal.IsThirsty) {
        //         _animal.ChangeState(States.Drink);
        //         return true;
        //     }
        //
        //     if (isRabbit) {
        //         if (_animal.GrassDetected() && _animal.IsHungry) {
        //             _animal.ChangeState(States.Eat);
        //             return true;
        //         }
        //
        //         if (_animal.PredatorDetected("Fox")) {
        //             _animal.ChangeState(States.Flee);
        //             return true;
        //         }
        //     }
        //
        //     if (!isRabbit) {
        //         if (_animal.RabbitDetected() && _animal.IsHungry) {
        //             _animal.ChangeState(States.Hunt);
        //             return true;
        //         }
        //     }
        //     
        //     return false;
        // }

        // private bool SeekPlant() {
        //     var plants = _animal.Detect("Grass");
        //     if (plants.Count <= 0) return false; // no plants detected
        //
        //     var targetPlant = plants[0].transform;
        //     Debug.Log($"Plant detected at {targetPlant.position}. Moving to eat.");
        //     _animal.agent.SetDestination(targetPlant.position);
        //     if (Vector3.Distance(_animal.transform.position, targetPlant.position) <= _animal.agent.stoppingDistance) {
        //         var plant = targetPlant.GetComponent<AbstractPlant>();
        //         // if (plant.foodTime <= 0) {
        //         //     plant.Eat();
        //         //     _animal.Eat(plant.foodValue);
        //         // }
        //         //
        //         // plant.foodTime -= Time.deltaTime * 10;
        //         if (!_animal.isEating) _animal.StartCoroutine(_animal.Eat(plant.foodValue));
        //     }
        //
        //     return true; // plant found
        // }
        //
        // private bool SeekWater() {
        //     var waterColliders = _animal.Detect("Water Point",_animal.visionRadius*4f);
        //
        //     // if (waterColliders.Count <= 0) return false;
        //
        //     foreach (var waterPoint in waterColliders) {
        //         if (!_animal.waterSources.Contains(waterPoint)) {
        //             var source = new WaterSource(waterPoint, _animal.forgetWaterTime);
        //             _animal.waterSources.Add(waterPoint, source);
        //         }
        //     }
        //     
        //     if (_animal.waterSources.Count <= 0) return false;
        //     
        //     
        //     var list = _animal.waterSources.Values
        //         .Cast<WaterSource>()
        //         .OrderBy(c => Vector3.Distance(_animal.transform.position,c.waterCollider.bounds.ClosestPoint(_animal.transform.position)))
        //         .ToList();
        //     Collider target = list[0].waterCollider;
        //
        //     // if (target is null) return false;
        //     
        //     Vector3 closestPoint = target.bounds.ClosestPoint(_animal.transform.position);
        //     Debug.Log($"Water detected at {closestPoint}. Moving to drink.");
        //     _animal.agent.SetDestination(closestPoint);
        //
        //     if (Vector3.Distance(_animal.transform.position, closestPoint) <= _animal.agent.stoppingDistance) {
        //         // _animal.Drink(30f);
        //         if (!_animal.isDrinking) _animal.StartCoroutine(_animal.Drink(30f));
        //     }
        //
        //     return true;
        // }
    }
}