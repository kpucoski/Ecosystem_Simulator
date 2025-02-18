using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ticker : MonoBehaviour {
   
    public static float tickTime = 0.2f;
     public static float tickTime05 = 0.5f;
    private float _tickerTimer;
    private float _tickerTimer05;

    public delegate void TickAction();
    public static event TickAction OnTickAction;


    void Update() {
        _tickerTimer += Time.deltaTime;
        if(_tickerTimer >= tickTime) {
            _tickerTimer = 0f;
            TickEvent();
        }
        if(_tickerTimer05 >= tickTime05) {
            _tickerTimer05 = 0f;
            TickEvent05();
        }
    }

    void TickEvent() {
        OnTickAction?.Invoke();
    }

     void TickEvent05() {
        OnTickAction?.Invoke();
    }
}
