using System.Collections.Generic;
using System.Linq;
using Animal.Interfaces;
using UnityEngine;

namespace Animal.AnimalStates {
    public class MateState :  IAnimalState {
        private AbstractAnimal _animal;
        private AbstractAnimal _mate;
        private WanderState w;

        public MateState(AbstractAnimal animal) {
            _animal = animal;
            w = new WanderState(_animal) {
                minRange = 15f,
                maxRange = 35f
            };
        }

        public void Enter() {
            // Debug.Log("Entering Mate State");
            _animal.isLookingForMate = true;
        }

        public void Execute() {
            if(!SeekMate()) w.Execute();
        }

        public void Exit() {
            // Debug.Log("Exiting Mate State");
            _animal.isLookingForMate = false;
        }

        public States GetState() {
            return States.Mate;
        }

        public void SetMate(AbstractAnimal mate) {
            _mate = mate;
        }

        private void Mate() {
            if (_animal.gender == Gender.Female) _animal.isPregnant = true;
            else if (_mate.gender == Gender.Female) _mate.isPregnant = true;
            
            _animal.StartMatingCooldown();
            _mate.StartMatingCooldown();
            
            _animal._mate = _mate;
            _mate._mate = _animal;
            ((BirthState)_animal.GetList()[States.Birth]).SetMate(_mate);
            ((BirthState)_mate.GetList()[States.Birth]).SetMate(_animal);
            
            _animal.ChangeState(States.Wander);
            _mate.ChangeState(States.Wander);
        }
        

        private bool SeekMate() {
            var targets = _animal.Detect(_animal.tag, _animal is Rabbit ? _animal.visionRadius : _animal.visionRadius*2f,_animal is Rabbit ? _animal.rabbitMask : _animal.foxMask);
            if (targets.Count <= 0) return false;
            
            var mates = targets
                .Select(m => m.GetComponent<AbstractAnimal>())
                .Where(m => m.CanMate(_animal) && m.IsRepro)
                .ToList();
            if (mates.Count <= 0) return false;
            _mate = mates[0];
            _animal._mate = _mate;
            var position = _mate._transform.position;
            // Debug.Log($"Mate detected at {position}. Moving to mate's position.");
            _animal.GoTo(position);

            // var dist = _animal is Rabbit ? 1.7f : 4f;
            // Debug.Log(Vector3.Distance(_animal._transform.position, _mate._transform.position));
            // if (Vector3.Distance(_animal._transform.position, _mate._transform.position) <= dist) {
            //     Mate();
            // }
            
            if (_animal._collider.bounds.Intersects(_mate._collider.bounds)) {
                Mate();
            }
            
            return true;
        }
    }
}