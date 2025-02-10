using System.Collections;
using Animal.Interfaces;
using UnityEngine;

namespace Animal.AnimalStates {
    public class IdleState : MonoBehaviour, IAnimalState {
        private AbstractAnimal _animal;
        
        public IdleState(AbstractAnimal animal) {
            _animal = animal;
        }
        public void Enter() {
        }

        public void Execute() {
            StartCoroutine(Idle());
        }

        public void Exit() {
        }

        public States GetState() {
            return States.Idle;
        }

        IEnumerator Idle() {
            yield return new WaitForSeconds(2f);
        }

      
    }
}