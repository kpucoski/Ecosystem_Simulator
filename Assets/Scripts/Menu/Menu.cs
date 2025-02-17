using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Utils;

public class Menu : MonoBehaviour {
    // void Start() {
        //Application.targetFrameRate = 144;
    // }
    public void StartSim() {
        //AnimalSpawner.numFox = SettingsMenu.numFox;
        //AnimalSpawner.numRabbit = SettingsMenu.numRabbit;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

     public void QuitGame() {
        Application.Quit();
    }
}
