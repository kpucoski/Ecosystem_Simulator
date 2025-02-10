using Animal;
using UnityEngine;

public static class Genes {
    
    private static float speedVar = 2f;
    private static float visionVar = 1f;
    private static float runSpeedVar = 0.1f;
    private static float pregnancyTimerVar = 20f;
    

    public static void Crossover(AbstractAnimal offspring, AbstractAnimal parent1, AbstractAnimal parent2) {
        offspring.visionRadius = Random.value < 0.5f ? parent1.visionRadius : parent2.visionRadius;
        offspring.speed = Random.value < 0.5f ? parent1.speed : parent2.speed;
        offspring.runSpeedMultiplier = Random.value < 0.5f ? parent1.runSpeedMultiplier : parent2.runSpeedMultiplier;
        offspring.maxPregnancyTimer =  Random.value < 0.5f ? parent1.maxPregnancyTimer : parent2.maxPregnancyTimer; // parent1.gender == Gender.Female ?
    }

    public static void Mutate(AbstractAnimal offspring, float mutationRate) {
        if (Random.value < mutationRate)
            offspring.speed += Random.Range(-speedVar, speedVar);
        if (Random.value < mutationRate)
            offspring.visionRadius += Random.Range(-visionVar, visionVar);
        if(Random.value < mutationRate)
            offspring.runSpeedMultiplier += Random.Range(-runSpeedVar, runSpeedVar);
        if (Random.value < mutationRate)
            offspring.maxPregnancyTimer += Random.Range(-pregnancyTimerVar, pregnancyTimerVar);
    }
}
