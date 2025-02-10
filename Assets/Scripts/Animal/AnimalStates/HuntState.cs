using System.Linq;
using Animal.Interfaces;
using UnityEngine;

namespace Animal.AnimalStates {
    public class HuntState : IAnimalState {
        private AbstractAnimal _fox;
        private Rabbit _rabbit;
        private WanderState w;
        private float distance;

        public HuntState(AbstractAnimal fox) {
            _fox = fox;
            w = new WanderState(_fox) {
                minRange = 15f,
                maxRange = 35f
            };
            distance = 3f;
        }

        public void Enter() {
            //_fox.StartRunning();
        }

        public void Execute() {
           if(!SeekRabbit()) {
               w.Execute();
               if (_fox.WaterDetected() 
                   && _fox.IsThirsty 
                   && !_fox.isEating
                   && !_fox.isDrinking 
                   && _fox.HungerThirstDifference(6f))
                   _fox.ChangeState(States.Drink);
           }
        }

        public void Exit() {
           _fox.StopRunning();
        }

        public States GetState() {
            return States.Hunt;
        }
        
        private bool SeekRabbit() {
            var targets = _fox.Detect("Rabbit",detectionMask:_fox.rabbitMask);
            if (targets.Count <= 0) return false;
            if(!_fox.isRunning)_fox.StartRunning();
            // if(_rabbit._collider != targets[0])
            _rabbit = targets[0].GetComponent<Rabbit>();
            var position = _rabbit._transform.position;
            _fox.GoTo(position);
            
            
            if (_fox._collider.bounds.Intersects(_rabbit._collider.bounds)) {
                CatchRabbit();
            }
            
            return true;
        }

        private void CatchRabbit() {
            if (_fox.isEating) return;
            _fox.StartCoroutine(_fox.Eat(_rabbit.foodValue));
            _rabbit.health = -1f;
            _fox.StopRunning();
        }
    }
}