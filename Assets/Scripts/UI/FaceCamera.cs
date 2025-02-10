using UnityEngine;

public class FaceCamera : MonoBehaviour {
    Camera _mainCamera;
    Transform _cameraTransform;
    Transform _transform;
    
    void Start() {
        _mainCamera = Camera.main;
        _cameraTransform = _mainCamera.transform;
        _transform = transform;
        GetComponent<Canvas>().worldCamera = _mainCamera;
        
    }
     void OnEnable() {
        Ticker.OnTickAction += Tick;
    }

    void OnDisable() {
        Ticker.OnTickAction -= Tick;

    }
    
    //void LateUpdate() {
    //   
    //}

    void Tick() {
        _transform.LookAt(_cameraTransform);
        _transform.Rotate(0, 180, 0);
    }
}