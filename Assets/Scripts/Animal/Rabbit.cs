using System.Collections;
using System.Collections.Generic;
using Animal.AnimalStates;
using UnityEngine;

namespace Animal {
    public class Rabbit : AbstractAnimal {
        public float foodValue = 30f;
        public float MaxfoodValue = 30f;

        new void Start() {
            InitBefore();
            base.Start();
            Init();
            // foodValue = Random.Range(30, 70);
            MaxfoodValue = Random.Range(30, 100);
            GetList().Add(States.Eat, new EatState(this));
            GetList().Add(States.Flee, new FleeState(this));
            Statistics.countTotalRabbit++;
            // r.Add(this);
            Statistics.r.Add(Statistics.countTotalRabbit,new Dictionary<string, float> {
                { "speed", speed },
                { "vision", visionRadius },
                { "run", runSpeed },
                { "ttl", timeLived }
            });
        }

        public void Initialize() {
            Start();
        }

        new void Update() {
            base.Update();
            foodValue = Mathf.Clamp(MaxfoodValue * _transform.localScale.x, 10f, MaxfoodValue);
        }

        new void Tick() {
            base.Tick();
            //foodValue = Mathf.Clamp(MaxfoodValue * _transform.localScale.x, 10f, MaxfoodValue);
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
            gameObject.name = gameObject.tag + " " + Statistics.countTotalRabbit;

            hungerDecayRate = 0.2f; //0.2 0.1
            thirstDecayRate = 0.25f; //0.15 0.075
            reproductionDecayRate = 2.7f;

            eatRate = 22f;
            drinkRate = 22f;

            hungerThreshold = 35f; //50
            thirstThreshold = 35f; //40
            reproductionThreshold = 25f; //30

            fieldOfView = 350f;
            // maxAge = Random.Range(230, 260);
            maxAge = Random.Range(150, 200);
            // numOfChildren = Random.Range(1,7);
            // numOfChildren += Random.Range(1,3);
            growthDuration = 20f;

            healthGain = 2f;
            hungerHealthLoss = -0.5f;
            thirstHealthLoss = -0.6f;
            thirstAndHungerHealthLoss = hungerHealthLoss + thirstHealthLoss;

            // speed = 2.5f;
            // visionRadius = 15f;
        }

        private void InitBefore() {
            maxSpeed = 15f;
            minSpeed = 3f;
            maxRunSpeedMultiplier = 1.5f;
        }
    }
}