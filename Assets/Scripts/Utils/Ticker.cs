using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ticker : MonoBehaviour {
   
    public static float tickTime = 0.2f;
    private float _tickerTimer;

    public delegate void TickAction();
    public static event TickAction OnTickAction;


    void Update() {
        _tickerTimer += Time.deltaTime;
        if(_tickerTimer >= tickTime) {
            _tickerTimer = 0f;
            TickEvent();
        }
    }

    void TickEvent() {
        OnTickAction?.Invoke();
    }
}
