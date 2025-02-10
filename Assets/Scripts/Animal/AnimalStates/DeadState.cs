using Animal.Interfaces;
using UnityEngine;

namespace Animal.AnimalStates {
    public class DeadState : IAnimalState {
        private AbstractAnimal _animal;
        private float Decay = 20f;

        public DeadState(AbstractAnimal animal) {
            _animal = animal;
            Decay = Random.Range(15, 40);
        }
        public void Enter() {
            _animal.agent.isStopped = true;
            _animal.canMate = false;
            // UnityEngine.Object.Destroy(_animal);
        }

        public void Execute() {
            Decay -= Time.deltaTime;
            if (Decay <= 0) Object.Destroy(_animal);
        }

        public void Exit() {
        }

        public States GetState() {
            return States.Dead;
        }
    }
}