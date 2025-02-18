using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class BackToMenu : MonoBehaviour {
    // void Start() {

    // }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Tracker.Instance.Restart();
            GrassSpawner.Instance.Restart();
            AnimalSpawner.Instance.Restart();
            Statistics.Restart();

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
}