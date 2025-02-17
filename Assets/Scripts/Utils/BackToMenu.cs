using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour {
   
    void Start() {
        
    }

    void Update() {
       if (Input.GetKeyDown(KeyCode.Escape)) {
        //Invoke("",0f);
        //Invoke("Tracker.Restart", 0f);
        //Invoke("GrassSpawner.Restart", 0f);
        //Invoke("AnimalSpawner.Restart",0f);
        
        Statistics.Restart();
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
       }
    }
}
