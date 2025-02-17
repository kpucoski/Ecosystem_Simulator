using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animal.AnimalStates;
using Animal.Interfaces;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Utils;
using Random = UnityEngine.Random;

namespace Animal {
    public class AbstractAnimal : MonoBehaviour {
        #region Variables

        public Transform _transform;
        private IAnimalState currentState;
        [SerializeField] public States currState;
        private Dictionary<States, IAnimalState> states;
        public NavMeshAgent agent;
        public Gender gender;
        public AbstractAnimal _mate;
        public Hashtable waterSources;
        public Collider _collider;

        [Header("Basic needs")] public float health = 100f;
        public float hunger = 0f;
        public float thirst = 0f;
        public float reproduction = 0f;
        public float stamina = 0f;
        public float age;

        public float speed = 5f;
        public float runSpeed = 7f;
        public float runSpeedMultiplier = 1f;
        public float numOfChildren = 3f;

        public bool isPregnant;
        public bool isRunning;
        public bool isAdult;
        public bool canMate = true;
        public bool isDrinking;
        public bool isEating;
        public bool isLookingForMate;
        public bool isTired = false;
        public bool initialPopulation = true;

        [Header("Timers")] 
        public float pregnancyTimer = 10f;
        public float growthDuration = 20f;
        public float matingCooldown = 20f;
        private float lastMatingTime = -Mathf.Infinity;
        public float forgetWaterTime = 60f;
        public float timeSpawned;
        public float timeLived;


        [Header("Limits")] 
        public float maxSpeed = 10f;
        public float minSpeed = 2f;
        // private const float MaxRunSpeed = 1.5f;
        // private const float MinRunSpeed = 1.1f;
        public float maxRunSpeedMultiplier = 1.5f;
        public float minRunSpeedMultiplier = 1.2f;
        // public float maxVisionRadius = 15f;
        // public float minVisionRadius = 5f;
        public float maxVisionRadius = 40f;
        public float minVisionRadius = 5f;
        public float maxPregnancyTimer = 30f;
        public float maxAge = 300f;


        [Header("Decay Rates")] 
        public float hungerDecayRate = 0.2f; // hunger decay per second
        public float thirstDecayRate = 0.3f; // thirst decay per second
        public float reproductionDecayRate = 0.3f; // reproduction decay per second
        private float staminaDecay = 15f;
        private float staminaRegen = 7f;
        public float hungerThreshold = 50f; // start seeking food
        public float thirstThreshold = 50f; // start seeking water
        public float reproductionThreshold = 50f; // start seeking water
        public float eatRate = 15f;
        public float drinkRate = 15f;

        [Header("Health Gain and Loss")] 
        public float healthGain = 1f;
        public float hungerHealthLoss = -1f;
        public float thirstHealthLoss = -1.5f;
        public float thirstAndHungerHealthLoss = -1.5f;


        [Header("Vision")] 
        public float visionRadius = 10f; // detection radius
        public float fieldOfView = 120f; // field of vision angle (in degrees)

        public bool IsHungry => hunger >= hungerThreshold; // check if animal is hungry
        public bool IsThirsty => thirst >= thirstThreshold; // check if animal is thirsty

        public bool IsRepro => reproduction >= reproductionThreshold;

        public bool IsAlive => !(health <= 0);
        public string DeathCause { get; set; }

        private static Collider[] _overlapResults = new Collider[10];
        public int waterPointMask;
        public int grassMask;
        public int foxMask;
        public int rabbitMask;

        #endregion


        #region EnableDisable
        void OnEnable() {
            Ticker.OnTickAction += Tick;
        }
        
        void OnDisable() {
            Ticker.OnTickAction -= Tick;

        }
        #endregion

        public virtual void Start() {
            _transform = transform;
            _collider = GetComponent<Collider>();
            agent = GetComponent<NavMeshAgent>();
            waterPointMask = LayerMask.GetMask("Water Point");
            grassMask = LayerMask.GetMask("Grass");
            foxMask = LayerMask.GetMask("Fox");
            rabbitMask = LayerMask.GetMask("Rabbit");
            waterSources = new Hashtable();
            states = new Dictionary<States, IAnimalState>() {
                { States.Wander, new WanderState(this) },
                { States.Mate, new MateState(this) },
                { States.Birth, new BirthState(this) },
                { States.Drink, new DrinkState(this) },
                // { States.Dead, new DeadState(this) }
            };
            timeSpawned = Time.time;
            InitStats();
            agent.speed = speed;
            agent.acceleration = speed * 1.25f;
            flockingForce = Vector3.zero;

            ChangeState(States.Wander);
        }

        private void InitStats() {
            health = 100f;
            thirst = 0f;
            hunger = 0f;
            stamina = 100f;
            reproduction = 0f;
            pregnancyTimer = maxPregnancyTimer;
            growthDuration = 30f;
            matingCooldown = 20f;
            lastMatingTime = -Mathf.Infinity;
            // numOfChildren = Mathf.Max(2, Mathf.Floor(maxPregnancyTimer / 10));
            // numOfChildren = Random.Range(1,5);
            isPregnant = false;
            age = 0f;
            thirst = Random.Range(0f, 21f);
            hunger = Random.Range(0f, 21f);
            if (initialPopulation) {
                // maxPregnancyTimer = Random.Range(10, 40);
                maxPregnancyTimer = Random.Range(20, 51);
                reproduction = Random.Range(0f, 11f);
                visionRadius = Random.Range(minVisionRadius, maxVisionRadius);
                speed = Random.Range(minSpeed, maxSpeed);
                runSpeedMultiplier = Random.Range(minRunSpeedMultiplier, maxRunSpeedMultiplier);
                age = Random.Range(0f, 61f);
            }

            numOfChildren = Mathf.Max(2, Mathf.Floor(maxPregnancyTimer / 10));
            speed = Mathf.Clamp(speed, minSpeed, maxSpeed); //-speed*(1-(2/numOfChildren))
            runSpeedMultiplier = Mathf.Clamp(runSpeedMultiplier, minRunSpeedMultiplier, maxRunSpeedMultiplier);
            runSpeed = speed * runSpeedMultiplier;
            visionRadius = Mathf.Clamp(visionRadius, minVisionRadius, maxVisionRadius);
            AssignRandomGender();
            isAdult = _transform.localScale == Vector3.one;
            if (!isAdult) {
                StartCoroutine(GrowOverTime());
            }
        }

        // run every 0.2s
        public void Tick() {
            // if (!IsAlive) {
            //     Die();
            //     return;
            // }
            //
            // UpdateStuck();
            // UpdateNeeds();

            //Flock();

            States s = DetermineState();

            if (s != currentState.GetState()) ChangeState(s);

            currState = currentState.GetState();
            currentState?.Execute();
        }

        public void Update() {
            if (!IsAlive) {
                Die();
                return;
            }
        
            UpdateStuck();
            UpdateNeeds();
        
            //Flock();
        
            // States s = DetermineState();
            //
            // if (s != currentState.GetState()) ChangeState(s);
            //
            // currState = currentState.GetState();
            // currentState?.Execute();
        }


        #region States

        public Dictionary<States, IAnimalState> GetList() {
            return states;
        }

        public void ChangeState(States state) {
            ChangeState(states[state]);
        }

        private void ChangeState(IAnimalState newState) {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }

        private Needs DeterminePriority() {
            if (this is Rabbit && PredatorDetected("Fox"))
                return Needs.Flee;
            if (pregnancyTimer <= 0 && isPregnant) //&& gender == Gender.Female
                return Needs.Birth;

            if (thirst > hunger && IsThirsty)
                return Needs.Thirst;

            if (hunger >= thirst && IsHungry && this is Rabbit)
                return Needs.Hunger;

            if (hunger >= thirst && IsHungry && this is Fox)
                return Needs.Hunt;

            // if (hunger == thirst) {
            //     if (Random.value > 0.5f) 
            //         return Needs.Thirst;
            //     return this is Rabbit ? Needs.Hunger : Needs.Hunt;
            // }

            if (reproduction > thirst && reproduction > hunger && IsRepro && canMate)
                return Needs.Mate;


            return Needs.None;
        }

        States DetermineState() {
            Needs priority = DeterminePriority();
            States s = States.Wander;

            switch (priority) {
                case Needs.None:
                    s = States.Wander;
                    break;
                case Needs.Mate:
                    s = States.Mate;
                    break;
                case Needs.Thirst:
                    s = States.Drink;
                    break;
                case Needs.Hunger:
                    s = States.Eat;
                    break;
                case Needs.Hunt:
                    s = States.Hunt;
                    break;
                case Needs.Birth:
                    s = States.Birth;
                    break;
                case Needs.Flee:
                    s = States.Flee;
                    break;
            }

            return s;
        }

        #endregion

        #region Detection

        public bool IsInFieldOfView(Transform target) {
            Vector3 directionToTarget = (target.position - _transform.position).normalized;
            float angle = Vector3.Angle(_transform.forward, directionToTarget);

            return angle <= fieldOfView / 2f;
        }

        // public bool Detected(string target, float visionOffset = 0) {
        //     Collider[] nearbyObjects = Physics.OverlapSphere(_transform.position, visionRadius + visionOffset);
        //     return nearbyObjects.Any(c => c.CompareTag(target) && IsInFieldOfView(c.transform));
        // }

        public bool Detected(string target, float visionOffset = 0, LayerMask detectionMask = default) {
            Vector3 position = _transform.position;
            // int count = Physics.OverlapSphereNonAlloc(position, visionRadius + visionOffset, _overlapResults);//,
            //detectionMask);

            int count = Physics.OverlapSphereNonAlloc(position, visionRadius + visionOffset, _overlapResults,
                detectionMask);

            for (int i = 0; i < count; i++) {
                if (_overlapResults[i].CompareTag(target) && IsInFieldOfView(_overlapResults[i].transform)) {
                    return true;
                }
            }

            return false;
        }

        // public List<Collider> Detect(string target, float visionOffset = 0) {
        //     Collider[] nearbyObjects = Physics.OverlapSphere(_transform.position, visionRadius + visionOffset);
        //
        //     return nearbyObjects
        //         .Where(c => c.CompareTag(target) && IsInFieldOfView(c.transform))
        //         .OrderBy(c => Vector3.Distance(_transform.position, c.transform.position))
        //         .ToList();
        // }

        public List<Collider> Detect(string target, float visionOffset = 0, LayerMask detectionMask = default) {
            Vector3 position = _transform.position;
            //int count = Physics.OverlapSphereNonAlloc(position, visionRadius + visionOffset, _overlapResults);//,
            //detectionMask);

            int count = Physics.OverlapSphereNonAlloc(position, visionRadius + visionOffset, _overlapResults,
                detectionMask);

            List<Collider> detectedObjects = new List<Collider>();

            for (int i = 0; i < count; i++) {
                Collider col = _overlapResults[i];
                if (col.CompareTag(target) && IsInFieldOfView(col.transform)) {
                    detectedObjects.Add(col);
                }
            }

            if (count > 1)
                detectedObjects.Sort((a, b) => Vector3.Distance(position, a.transform.position)
                    .CompareTo(Vector3.Distance(position, b.transform.position)));

            return detectedObjects;
        }

        public bool WaterDetected() {
            return Detected("Water Point", visionRadius * 4f, waterPointMask);
        }

        public bool GrassDetected() {
            return Detected("Grass", 0, grassMask);
        }

        public bool PredatorDetected(string tag) {
            var tar = Detect(tag, detectionMask: foxMask);
            if (tar.Count > 0 && this is Rabbit) {
                var a = (FleeState)states[States.Flee];
                var fox = tar[0];
                float safeDist = 10f;
                Vector3 toFox = (fox.transform.position - _transform.position).normalized;

                float distSqr = (fox.transform.position - _transform.position).sqrMagnitude;
                float safeDistSqr = safeDist * safeDist;

                if (Vector3.Dot(fox.transform.forward.normalized, toFox) > 0.5f || distSqr <= safeDistSqr) {
                    if (a.getFox() is null || a.getFox() != fox)
                        a.setFox(fox);

                    return true;
                }
            }

            return false;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Rabbit"))
                print("rabbit");
        }

        public bool RabbitDetected() {
            return Detected("Rabbit");
        }

        #endregion

        private void UpdateNeeds() {
            UpdateFoodAndWater();
            UpdateStamina();
            UpdateReproduction();
            UpdateHealth();
            UpdateAge();
            IsReadyToMate();
            UpdatePregnancyTimer();
            ForgetWaterSource();
        }
        
        public void UpdateStamina() {
            if (stamina <= 0) isTired = true;
            if (stamina >= 100) isTired = false;
            
            if (isRunning && !isTired) {
                stamina = Mathf.Clamp(stamina - staminaDecay * Time.deltaTime, 0, 100);
                agent.speed = runSpeed;
            }

            // if (isTired || !isRunning) {
            //     stamina = Mathf.Clamp(stamina + staminaRegen * Time.deltaTime, 0, 100);
            // }
            if (!isRunning) {
                stamina = Mathf.Clamp(stamina + staminaRegen * Time.deltaTime, 0, 100);
            }

            if (isTired)
                agent.speed = speed;
        }

        #region Food And Water

        private void UpdateFoodAndWater() {
            if (!isEating)
                hunger = Mathf.Clamp(hunger + hungerDecayRate * agent.speed * Time.deltaTime, 0, 100);
            if (!isDrinking)
                thirst = Mathf.Clamp(thirst + thirstDecayRate * agent.speed * Time.deltaTime, 0, 100);
        }

        public IEnumerator Eat(float foodValue) {
            if (isDrinking || isEating) yield break;
            isEating = true;
            lastEat = Time.time;
            agent.isStopped = true;
            float totalTime = foodValue / eatRate;
            float elapsedTime = 0f;
            while (elapsedTime < totalTime) {
                hunger = Mathf.Clamp(hunger - eatRate * Time.deltaTime, 0, 100);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            isEating = false;
            if (IsAlive)
                agent.isStopped = false;
            ChangeState(States.Wander);
        }

        public IEnumerator Drink(float waterValue) {
            if (isDrinking || isEating) yield break;
            isDrinking = true;
            lastDrink = Time.time;
            agent.isStopped = true;

            float totalTime = waterValue / drinkRate;
            float elapsedTime = 0f;

            while (elapsedTime < totalTime) {
                thirst = Mathf.Clamp(thirst - drinkRate * Time.deltaTime, 0, 100);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            isDrinking = false;
            if (IsAlive)
                agent.isStopped = false;
            ChangeState(States.Wander);
        }

        private void ForgetWaterSource() {
            foreach (var source in waterSources.Keys) {
                ((WaterSource)waterSources[source]).timer -= Time.deltaTime;
                if (((WaterSource)waterSources[source]).timer <= 0) {
                    waterSources.Remove(source);
                    break;
                }
            }
        }

        #endregion

        #region Health and Age

        private void UpdateHealth() {
            var minusHealth = healthGain;
            if (thirst == 100f) {
                minusHealth = thirstHealthLoss;
            }

            if (hunger == 100f) {
                minusHealth = hungerHealthLoss;
            }

            if (hunger >= 100 && thirst >= 100) {
                minusHealth = thirstAndHungerHealthLoss;
            }

            health = Mathf.Clamp(health + minusHealth * Time.deltaTime, 0, 100);
        }

        void Die() {
            // Destroy(gameObject); // TODO: add as a food item if animal dead for X seconds delete
            if (health == 0 && IsThirsty && thirst >= hunger) DeathCause = "Malnutrition";
            if (health == 0 && IsHungry && hunger >= thirst) DeathCause = "Malnutrition";
            if (health == 0 && IsHungry && IsThirsty && hunger == thirst) DeathCause = "Malnutrition";
            if (health == -1f) DeathCause = "Predation";
            if (health == 0 && age == maxAge) DeathCause = "Old Age";
            timeLived = Time.time - timeSpawned;

            if (this is Rabbit) {
                Statistics.deathStatisticsRabbit[DeathCause]++;
                AnimalSpawner.DespawnRabbit(this);
                Statistics.rabbitDeaths++;
            }
            else {
                Statistics.deathStatisticsFox[DeathCause]++;
                AnimalSpawner.DespawnFox(this);
                Statistics.foxDeaths++;
            }
        }

        private void UpdateAge() {
            if (age >= maxAge) {
                health = 0;
            }

            age = Mathf.Clamp(age + Time.deltaTime, 0, maxAge);
        }

        private IEnumerator GrowOverTime() {
            isAdult = false;
            float elapsedTime = 0f;

            while (elapsedTime < growthDuration) {
                _transform.localScale =
                    Vector3.Lerp(new Vector3(0.5f, 0.5f, 0.5f), Vector3.one, elapsedTime / growthDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _transform.localScale = Vector3.one;
            isAdult = true;
            canMate = true;
        }

        #endregion

        private void AssignRandomGender() {
            gender = Random.value > 0.5f ? Gender.Male : Gender.Female;
        }

        #region Mating

        private void UpdateReproduction() {
            if (!canMate) return;
            reproduction = Mathf.Clamp(reproduction + reproductionDecayRate * Time.deltaTime, 0, 100);
        }

        private void IsReadyToMate() {
            canMate = Time.time >= lastMatingTime + matingCooldown && isAdult;
        }

        public void StartMatingCooldown() {
            lastMatingTime = Time.time;
            canMate = false;
            reproduction = 0;
        }

        public bool CanMate(AbstractAnimal other) {
            return gender != other.gender && canMate && other.canMate;
        }

        private void UpdatePregnancyTimer() {
            if (!isPregnant || pregnancyTimer <= 0) return;
            pregnancyTimer = Mathf.Clamp(pregnancyTimer - Time.deltaTime, 0, 10);
        }

        #endregion

        bool FitnessFunction(AbstractAnimal o) {
            return speed >= 5;
        }

        #region Movement

        public void GoTo(Vector3 target) {
            agent.SetDestination(target);
        }

        public void StartRunning() {
            isRunning = true;
            agent.speed = runSpeed;
        }

        public void StopRunning() {
            isRunning = false;
            agent.speed = speed;
        }
        #endregion

        private Vector3 lastPosition;
        private float stuckTimer = 0f;
        private float lastDrink = 0f;
        private float lastEat = 0f;

        void UpdateStuck() {
            UpdateStuckPosition();
            UpdateStuckEatingDrinking();
        }

        void UpdateStuckPosition() {
            if (Vector3.Distance(_transform.position, lastPosition) < 0.1f) {
                stuckTimer += Time.deltaTime;
                if (stuckTimer > 12f) {
                    ResolveStuckAgent();
                    stuckTimer = 0f;
                }
            }
            else {
                stuckTimer = 0f;
            }

            lastPosition = _transform.position;
        }

        void UpdateStuckEatingDrinking() {
            if (isDrinking)
                if (Time.time - lastDrink > 11f)
                    isDrinking = false;
            if (isEating)
                if (Time.time - lastEat > 11f)
                    isEating = false;
        }

        void ResolveStuckAgent() {
            agent.ResetPath();
            Vector3 newTarget = transform.position + new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
            agent.SetDestination(newTarget);
        }

        public bool HungerThirstDifference(float difference = 0) {
            return Mathf.Abs(hunger - thirst) <= difference;
        }

        [Header("Flocking Parameters")] 
        public float flockRadius = 10f; // Radius to find neighbors
        public float separationDistance = 2f; // Minimum distance to maintain
        public float cohesionWeight = 1.0f; // Weight for cohesion
        public float alignmentWeight = 1.0f; // Weight for alignment
        public float separationWeight = 3f; // Weight for separation

        private Vector3 flockingForce; // Flocking vector to apply

        void Flock() {
            flockingForce = CalculateFlockingForce();
            GoTo(agent.destination + flockingForce);
        }

        private Vector3 CalculateFlockingForce() {
            List<AbstractAnimal> neighbors = GetFlockmates();

            if (neighbors.Count == 0) return Vector3.zero;

            Vector3 cohesion = CalculateCohesionForce(neighbors);
            Vector3 alignment = CalculateAlignmentForce(neighbors);
            Vector3 separation = CalculateSeparationForce(neighbors);

            return cohesion * cohesionWeight +
                   alignment * alignmentWeight +
                   separation * separationWeight;
        }

        private List<AbstractAnimal> GetFlockmates() {
            Collider[] nearbyColliders = Physics.OverlapSphere(_transform.position, flockRadius);
            var t = this is Rabbit ? "Rabbit" : "Fox";
            return nearbyColliders
                .Where(c => c.gameObject != gameObject && c.CompareTag(t))
                .Select(c => c.GetComponent<AbstractAnimal>())
                .ToList();
        }

        private Vector3 CalculateCohesionForce(List<AbstractAnimal> neighbors) {
            Vector3 averagePosition = Vector3.zero;
            foreach (var neighbor in neighbors) {
                averagePosition += neighbor._transform.position;
            }

            averagePosition /= neighbors.Count;

            return (averagePosition - _transform.position).normalized;
        }

        private Vector3 CalculateAlignmentForce(List<AbstractAnimal> neighbors) {
            Vector3 averageDirection = Vector3.zero;
            // foreach (var neighbor in neighbors) {
            //     averageDirection += neighbor.agent.velocity;
            // }
            foreach (var neighbor in neighbors) {
                averageDirection += neighbor._transform.forward;
            }

            // averageDirection /= neighbors.Count;
            if (averageDirection != Vector3.zero)
                averageDirection.Normalize();

            return averageDirection;
        }

        private Vector3 CalculateSeparationForce(List<AbstractAnimal> neighbors) {
            Vector3 separationForce = Vector3.zero;
            foreach (var neighbor in neighbors) {
                var dir = neighbor._transform.position - _transform.position;
                var dist = dir.magnitude;
                var away = -dir.normalized;
                if (dist > 0) separationForce += (away / dist);
            }

            return separationForce.normalized;
        }
    }
}