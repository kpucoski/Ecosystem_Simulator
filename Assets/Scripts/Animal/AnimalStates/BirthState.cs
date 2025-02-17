using Animal.Interfaces;
using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace Animal.AnimalStates {
    public class BirthState :  IAnimalState {
        private AbstractAnimal _animal;
        private AbstractAnimal _mate;

        public BirthState(AbstractAnimal animal) {
            _animal = animal;
            _mate = _animal._mate;
        }

        public void Enter() {
            // Debug.Log("Birthing");
        }

        public void Execute() {
            // _animal.agent.isStopped = true;
            _animal.pregnancyTimer = _animal.maxPregnancyTimer;
            _animal.isPregnant = false;
            // _animal.numOfChildren = Mathf.Max(1,Mathf.Floor(_animal.MaxPregnancyTimer / 25));
            for (int i = 0; i < _animal.numOfChildren; i++) {
                Vector3 offspringPosition = _animal._transform.position + Random.insideUnitSphere * 1f;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(offspringPosition, out hit, 1.0f, NavMesh.AllAreas)) {
                    // var a = Object.Instantiate(_animal.gameObject, hit.position,Quaternion.Euler(new Vector3(0,Random.Range(0,360),0)));
                    // var b = a.GetComponent<AbstractAnimal>();
                    // b._transform.position = hit.position;
                    // b._transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
                    // b._transform.localScale = new Vector3(0.5f,0.5f,0.5f);
                    // b.canMate = false;
                    // Genes.Crossover(b,_animal,_mate);
                    // Genes.Mutate(b,0.2f);
                    // b.initialPopulation = false;
                    // b._mate = null;
                    
                    AbstractAnimal b = _animal is Rabbit ? AnimalSpawner.GetRabbitPool().Get() : AnimalSpawner.GetFoxPool().Get();
                    
                    b.isAdult = false;
                    b.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
                    b.canMate = false;
                    b.initialPopulation = false;
                    b._mate = null;
                    Genes.Crossover(b,_animal,_mate);
                    Genes.Mutate(b,0.2f);

                    if (b is Rabbit){
                        Statistics.countTotalRabbit++;
                        ((Rabbit)b).Initialize();
                        
                        Statistics.rabbitBirths++;
                        Statistics.countRabbit++;

                    }
                    else {
                        Statistics.countTotalFox++;
                        ((Fox)b).Initialize();
                        Statistics.foxBirths++;
                        Statistics.countFox++;
                    }
                    b._transform.position = hit.position;
                    b._transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
                    
                }
             
            }
            // _animal.agent.isStopped = false;
            _animal.ChangeState(States.Wander);
        }

        public void Exit() {
            
        }

        public States GetState() {
            return States.Birth;
        }

        public void SetMate(AbstractAnimal mate) {
            _mate = mate;
        }

        void InitSpawn(AbstractAnimal animal, Vector3 pos) {
            if (animal is Rabbit)
                ((Rabbit)animal).Initialize();
            else {
                ((Fox)animal).Initialize();
            }
            animal._transform.position = pos;
            animal._transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
            animal._transform.localScale = new Vector3(0.5f,0.5f,0.5f);
            animal.canMate = false;
            Genes.Crossover(animal,_animal,_mate);
            Genes.Mutate(animal,0.2f);
            animal.initialPopulation = false;
            animal._mate = null;
        }
    }
}