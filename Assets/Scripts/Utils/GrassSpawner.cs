using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSpawner : MonoBehaviour {
    [Header("Spawn settings")] public GameObject resourcePrefab;
    public float spawnChance;

    [Header("Raycast setup")] public float distanceBetweenCheck;
    public float heightOfCheck = 32f, rangeOfCheck = 18f;
    public LayerMask layerMask;
    public Vector2 positivePosition, negativePosition;

    private void Start() {
        SpawnResources();
    }

    // private void Update() {
    //     if (Input.GetKeyDown(KeyCode.R)) {
    //         DeleteResources();
    //         SpawnResources();
    //     }
    // }

    void SpawnResources() {
        for (float x = negativePosition.x; x < positivePosition.x; x += distanceBetweenCheck) {
            for (float z = negativePosition.y; z < positivePosition.y; z += distanceBetweenCheck) {
                RaycastHit hit;
                float dist = 2f;
                float randx = Random.Range(x - dist, x + dist);
                float randz = Random.Range(z - dist, z + dist);
                if (Physics.Raycast(new Vector3(x + randx, heightOfCheck, z + randz), Vector3.down, out hit,
                        rangeOfCheck, layerMask)) {
                    if (spawnChance > Random.Range(0f, 101f)) {
                        // hit.point.Set(hit.point.x,Terrain.activeTerrain.SampleHeight(hit.point),hit.point.z);
                        var g = Instantiate(resourcePrefab, hit.point,
                            Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)), transform);
                        g.tag = "Grass";
                        Statistics.plantCount++;
                        // Debug.DrawLine(new Vector3(x+randx, heightOfCheck, z+randz),hit.point,Color.red,500);
                    }
                    // Debug.DrawRay(new Vector3(x+randx, heightOfCheck, z+randz),Vector3.down*30,Color.red,500);
                }
            }
        }
    }

    void DeleteResources() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
    }
    
}