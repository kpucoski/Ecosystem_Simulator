using System.Collections.Generic;
using Animal.AnimalStates;
using Unity.VisualScripting;
using UnityEngine;

namespace Animal {
    public class Fox : AbstractAnimal {

        new void Start() {
            base.Start();
            Init();
            GetList().Add(States.Hunt,new HuntState(this));     
            //Statistics.countTotalFox++;
            // DeathCounter.f.Add(this);
            // Statistics.f.Add(Statistics.countTotalFox,new Dictionary<string, float> {
            //     { "speed", speed },
            //     { "vision", visionRadius },
            //     { "run", runSpeed },
            //     { "ttl", timeLived }
            // });     
        }
        
        public void Initialize() {
            Start();
        }

        new void Update() {
            base.Update();
        }
        
        new void Tick() {
            base.Tick();
        }
        
        
        #region EnableDisable
        void OnEnable() {
            Ticker.OnTickAction += Tick;
        }
        
        void OnDisable() {
            Ticker.OnTickAction -= Tick;

        }
        #endregion

        private void Init() {
            gameObject.name = gameObject.tag + " " + Statistics.countTotalFox;
            // hungerDecayRate = 0.15f; //0.3 0.15
            hungerDecayRate = 0.4f; //0.3 0.15
            // thirstDecayRate = 0.20f; //0.2 0.1
            thirstDecayRate = 0.3f; //0.2 0.1
            // reproductionDecayRate = 0.35f;
            reproductionDecayRate = 1.0f;

            hungerThreshold = 40f; //50
            thirstThreshold = 35f; //55
            reproductionThreshold = 30f; //30

            fieldOfView = 260f;
            // maxAge = Random.Range(280, 320);
            maxAge = Random.Range(200, 250);
            // numOfChildren = Random.Range(1,5);
            growthDuration = 40f;

            healthGain = 2f;
            hungerHealthLoss = -0.33f;
            thirstHealthLoss = -0.44f;
            thirstAndHungerHealthLoss = hungerHealthLoss + thirstHealthLoss;

           
            // speed = 3.5f;
            // visionRadius = 20f;
        }
    }
}