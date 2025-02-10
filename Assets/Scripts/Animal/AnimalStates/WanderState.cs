using System.Collections;
using System.Collections.Generic;
using Animal;
using Animal.AnimalStates;
using UnityEngine;
using UnityEngine.AI;

public class WanderState : Animal.Interfaces.IAnimalState {
    private AbstractAnimal _animal;
    public float range; //radius of sphere
    public float minRange = 5f;
    public float maxRange = 30f;
    
    // animations
    public float jumpHeight = 0.2f;
    public float jumpFrequency = 1f;
    private float originalY;


    public WanderState(AbstractAnimal animal) {
        _animal = animal;
    }

    public void Enter() {
        // Debug.Log("Entering Wander State");
    }

    public void Execute() {
        // originalY = _animal._transform.position.y;
        
        if (_animal.agent.remainingDistance <= _animal.agent.stoppingDistance) { //done with path
            Vector3 point;
            range = Random.Range(minRange, maxRange);
            float curiosityChance = Random.Range(0f, 1f);
            if (curiosityChance < 0.4f)
                range += maxRange;
            if (RandomPoint( _animal._transform.position, range, out point)) { //pass in our centre point and radius of area
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                _animal.GoTo(point);
            }
        }
        // Animate jumping
        // AnimateJump();

    }

    public void Exit() {
        // Debug.Log("Exiting Wander State");
    }

    public States GetState() {
        return States.Wander;
    }

    void AnimateJump() {
        float newY = originalY + Mathf.Sin(Time.time * jumpFrequency * Mathf.PI * 2) * jumpHeight;
        var transform = _animal._transform;
        var position = transform.position;
        position = new Vector3(position.x, newY, position.z);
        if (position.y < Terrain.activeTerrain.SampleHeight(transform.position)) {
            position.y = Terrain.activeTerrain.SampleHeight(transform.position);
        }

        transform.position = position;
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result) {
        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f,
                NavMesh.AllAreas)) //documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        {
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            Debug.DrawLine(center,result,Color.blue,10f);
            return true;
        }

        result = Vector3.zero;
        return false;
    }
}