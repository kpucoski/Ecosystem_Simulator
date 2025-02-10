using System.Collections;
using UnityEngine;

public class AbstractPlant : MonoBehaviour {
    public bool isEaten;
    public float regrowthTime = 5f;
    public float maxRegrowthTime = 40f;
    public float minRegrowthTime = 10f;
    public float foodValue = 30f;
    public Transform _transform;
    public Collider _collider;
    
    void Start() {
        Init();
        _transform = transform;
        _collider = GetComponent<Collider>();
    }

    void Init() {
        regrowthTime = Random.Range(minRegrowthTime, maxRegrowthTime);
        // foodValue = Random.Range(minRegrowthTime, maxRegrowthTime);
        foodValue = regrowthTime;
    }

    public void Eat() {
        if (isEaten) return;
        isEaten = true;
        StartCoroutine(SpawnNewPlantAfterDelay());
    }

    private IEnumerator SpawnNewPlantAfterDelay() {
        // hide plant after eating it
        Hide();
        
        // wait X seconds
        yield return new WaitForSeconds(regrowthTime);

        // "spawn"/show new plant
        Spawn();
    }

    private void Hide() {
        var h = _transform.position;
        h.y = 0;
        _transform.position = h;
        Statistics.plantCount--;
    }

    private void Spawn() {
        Vector3 randomOffset = new Vector3(
            Random.Range(-1f, 1f),
            0,
            Random.Range(-1f, 1f)
        );
        Vector3 spawnPosition = _transform.position + randomOffset;
        spawnPosition.y = Terrain.activeTerrain.SampleHeight(spawnPosition);
        _transform.position = spawnPosition;
        _transform.rotation = Quaternion.Euler(new Vector3(0,Random.Range(0,360),0));
        
        isEaten = false;
        Statistics.plantCount++;

        Init();
    }
   
}