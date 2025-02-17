using Animal;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utils {
    public class AnimalSpawner : MonoBehaviour {
        [Header("Spawn settings")] public GameObject foxPrefab;
        public int numberOfFoxes;
        public static int numFox;
        public GameObject rabbitPrefab;
        public int numberOfRabbits;
        public static int numRabbit;
        public LayerMask layerMask;
        public Vector2 positivePosition, negativePosition;
        public float heightOfCheck = 32f, rangeOfCheck = 18f;

        private static ObjectPool<Rabbit> rabbitPool;
        private static ObjectPool<Fox> foxPool;

        void Start() {
            //numberOfRabbits = numRabbit;
            //numberOfFoxes = numFox;
            rabbitPool = new ObjectPool<Rabbit>(rabbitPrefab, numberOfRabbits*5, transform);
            foxPool = new ObjectPool<Fox>(foxPrefab, numberOfFoxes*5, transform);
            SpawnAll();
        }

        public static ObjectPool<Rabbit> GetRabbitPool() {
            return rabbitPool;
            
        }

        public static ObjectPool<Fox> GetFoxPool() {
            return foxPool;
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Restart();
            }
        }

        void SpawnAnimals(GameObject animalPrefab, int n) {
            int c = 0;
            while (c < n) {
                float x = Random.Range(negativePosition.x, positivePosition.x);
                float z = Random.Range(negativePosition.y, positivePosition.y);
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(x, heightOfCheck, z), Vector3.down, out hit, rangeOfCheck, layerMask)) {
                    GameObject a = Instantiate(animalPrefab, hit.point,
                        Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)), transform);
        
        
                    c++;
        
                    // Debug.DrawLine(new Vector3(x, heightOfCheck, z),hit.point,Color.red,500);
                }
            }
        }
        
        void SpawnAnimals(string type, int n) {
            int c = 0;
            while (c < n) {
                float x = Random.Range(negativePosition.x, positivePosition.x);
                float z = Random.Range(negativePosition.y, positivePosition.y);
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(x, heightOfCheck, z), Vector3.down, out hit, rangeOfCheck, layerMask)) {
                    // GameObject a = Instantiate(animalPrefab, hit.point,
                    //     Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)), transform);
                    if (type.Equals("Rabbit")) SpawnRabbit(hit.point); 
                    else if (type.Equals("Fox")) SpawnFox(hit.point);
                    c++;

                    // Debug.DrawLine(new Vector3(x, heightOfCheck, z),hit.point,Color.red,500);
                }
            }
        }

        public static void SpawnRabbit(Vector3 position) {
            Rabbit newRabbit = (Rabbit)rabbitPool.Get();
            newRabbit.transform.position = position;
            Statistics.countRabbit++;
            Statistics.countTotalRabbit++;
            //newRabbit.Start(); // Reset state
            // newRabbit.Initialize();
        }

        public static void SpawnFox(Vector3 position) {
            Fox newFox = (Fox)foxPool.Get();
            newFox.transform.position = position;
            Statistics.countFox++;
            Statistics.countTotalFox++;
            //newFox.Start(); // Reset state
            //newFox.Initialize();
        }

        public static void DespawnRabbit(AbstractAnimal rabbit) {
            rabbitPool.ReturnToPool(rabbit);
            Statistics.countRabbit--;
        }

        public static void DespawnFox(AbstractAnimal fox) {
            foxPool.ReturnToPool(fox);
            Statistics.countFox--;
        }

        void SpawnFoxes(int n) {
            SpawnAnimals(foxPrefab, n);
        }
        
        void SpawnRabbits(int n) {
            SpawnAnimals(rabbitPrefab, n);
        }

        void DestroyAll() {
            foreach (var fox in GameObject.FindGameObjectsWithTag("Fox")) {
                Destroy(fox);
            }

            foreach (var rabbit in GameObject.FindGameObjectsWithTag("Rabbit")) {
                Destroy(rabbit);
            }
        }

        void DespawnAll() {
            foreach (var fox in GameObject.FindGameObjectsWithTag("Fox")) {
                DespawnFox(fox.GetComponent<Fox>());
            }
            foreach (var rabbit in GameObject.FindGameObjectsWithTag("Rabbit")) {
                DespawnRabbit(rabbit.GetComponent<Rabbit>());
            }
        }

        void SpawnAll() {
            //numberOfRabbits = numRabbit;
            //numberOfFoxes = numFox;
            SpawnAnimals("Rabbit",numberOfRabbits);
            SpawnAnimals("Fox",numberOfFoxes);
        }

        public void Restart() {
            DespawnAll();
            // Invoke("Statistics.Restart",0f);
            // DestroyAll();
            // Start();
        }

        // void Awake() {
        //     Application.targetFrameRate = 120;
        // }
    }
}