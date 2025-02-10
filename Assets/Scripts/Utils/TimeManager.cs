using Animal;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Utils {
    public class TimeManager : MonoBehaviour {
        public TextMeshProUGUI text;
        public TextMeshProUGUI textRabbits;
        public TextMeshProUGUI textFoxes;
        public TextMeshProUGUI textTime;
        public TextMeshProUGUI textPlants;

        private float startTime;
        private float elapsedTime;
        void Start() {
            Time.timeScale = 1f;
            text.text = $"Speed: {Time.timeScale}x";
            startTime = Time.time;
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.Comma)) {
                Time.timeScale = Mathf.Clamp(Time.timeScale - 1f, 1f, 5f);
            }

            if (Input.GetKeyDown(KeyCode.Period)) {
                Time.timeScale = Mathf.Clamp(Time.timeScale + 1f, 1f, 5f);
            }

            if (Input.GetKeyDown(KeyCode.R)) {
                startTime = Time.time;
            }

            elapsedTime = Time.time - startTime;
            textTime.text =  $"Time: {(int)elapsedTime}";
            text.text = $"Speed: {Time.timeScale}x";
            //textRabbits.text = $"Rabbits: {GameObject.FindGameObjectsWithTag("Rabbit").Length}";
            textRabbits.text = $"Rabbits: {Statistics.countRabbit}";

            //textFoxes.text = $"Foxes: {GameObject.FindGameObjectsWithTag("Fox").Length}";
            textFoxes.text = $"Foxes: {Statistics.countFox}";

            textPlants.text = $"Plants: {Statistics.plantCount}"; 
        }
    }
}