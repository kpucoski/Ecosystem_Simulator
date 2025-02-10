using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class SettingsMenu : MonoBehaviour {

    public TMP_InputField fox;
    public TMP_InputField rabbit;

    public static int numFox = 20;
    public static int numRabbit = 100;

    private void Start() {
        Int32.TryParse(fox.text, out numFox);
        Int32.TryParse(rabbit.text, out numRabbit);
    }

    public void UpdateType() {
        Int32.TryParse(fox.text, out numFox);
        Int32.TryParse(rabbit.text, out numRabbit);
    }
   
}
