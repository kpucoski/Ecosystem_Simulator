using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Animal;

public class Tracker : MonoBehaviour {
    private float timer;
    public float interval = 20f;
    private string filePathFox;
    private string filePathRabbit;
    private string filePathDeathCausesRabbit;
    private string filePathDeathCausesFox;
    private string filePathFoxAll;
    private string filePathRabbitAll;
    private bool firstTrack = false;
    private float startTime;

    private GameObject[] foxes; 
    private GameObject[] rabbits; 
    
    void Start() {
        startTime = Time.time;
        firstTrack = false;
        timer = 0;
        
        filePathFox = Application.dataPath + "/fox_data.csv";
        File.WriteAllText(filePathFox, "Time,Population,AverageSpeed,AverageRunSpeed,AverageVision,AverageChildren,AveragePregnancy,Deaths,Births\n");
        
        filePathRabbit = Application.dataPath + "/rabbit_data.csv";
        File.WriteAllText(filePathRabbit, "Time,Population,AverageSpeed,AverageRunSpeed,AverageVision,AverageChildren,AveragePregnancy,Deaths,Births\n");
        
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Restart();
        }
        timer += Time.deltaTime;
        if (!firstTrack && timer < 0.5f) {
            firstTrack = true;
            Append();
        }

        if (timer >= interval) {
            timer = 0f;
            Append();
        }
    }

    public void Restart() {
        Start();
        foxes = null;
        rabbits = null;
    }

    void Append() {
        foxes = GameObject.FindGameObjectsWithTag("Fox");
        rabbits = GameObject.FindGameObjectsWithTag("Rabbit");
        
        int foxCount = foxes.Length;
        int rabbitCount = rabbits.Length;
            
        float avgFoxSpeed = 0;
        float avgFoxRunSpeed = 0;
        float avgFoxVision = 0;
        float avgFoxChildren = 0;
        float avgFoxPregnancy = 0;
       
        foreach (var fox in foxes) {
            var f = fox.GetComponent<Fox>(); 
            avgFoxSpeed += f.speed;
            avgFoxRunSpeed += f.runSpeed;
            avgFoxVision += f.visionRadius;
            avgFoxChildren += f.numOfChildren;
            avgFoxPregnancy += f.maxPregnancyTimer;
        }

        avgFoxSpeed /= foxCount;
        avgFoxRunSpeed /= foxCount;
        avgFoxVision /= foxCount;
        avgFoxChildren /= foxCount;
        avgFoxPregnancy /= foxCount;


        float avgRabbitSpeed = 0;
        float avgRabbitRunSpeed = 0;
        float avgRabbitVision = 0;
        float avgRabbitPregnancy = 0;
        float avgRabbitChildren = 0;
        

        
        foreach (var rabbit in rabbits) {
            var r = rabbit.GetComponent<Rabbit>();
            avgRabbitSpeed += r.speed;
            avgRabbitRunSpeed += r.runSpeed;
            avgRabbitVision += r.visionRadius;
            avgRabbitChildren += r.numOfChildren;
            avgRabbitPregnancy += r.maxPregnancyTimer;
        }

        avgRabbitSpeed /= rabbitCount;
        avgRabbitRunSpeed /= rabbitCount;
        avgRabbitVision /= rabbitCount;
        avgRabbitChildren /= rabbitCount;
        avgRabbitPregnancy /= rabbitCount;


        var time = Time.time - startTime;
        time = ((int)(time / 10f)) * 10f;
        
        
        File.AppendAllText(filePathFox, $"{time},{foxCount},{avgFoxSpeed},{avgFoxRunSpeed},{avgFoxVision},{avgFoxChildren},{avgFoxPregnancy},{Statistics.foxDeaths},{Statistics.foxBirths}\n");
        File.AppendAllText(filePathRabbit, $"{time},{rabbitCount},{avgRabbitSpeed},{avgRabbitRunSpeed},{avgRabbitVision},{avgRabbitChildren},{avgRabbitPregnancy},{Statistics.rabbitDeaths},{Statistics.rabbitBirths}\n");
    }

    private void OnDisable() {
        filePathDeathCausesRabbit = Application.dataPath + "/rabbit_death_data.csv";
        File.WriteAllText(filePathDeathCausesRabbit, "Malnutrition,Predation,Old Age\n");
        
        filePathDeathCausesFox = Application.dataPath + "/fox_death_data.csv";
        File.WriteAllText(filePathDeathCausesFox, "Malnutrition,Old Age\n");
        
        File.AppendAllText(filePathDeathCausesRabbit, $"{Statistics.deathStatisticsRabbit["Malnutrition"]},{Statistics.deathStatisticsRabbit["Predation"]},{Statistics.deathStatisticsRabbit["Old Age"]}");
        File.AppendAllText(filePathDeathCausesFox, $"{Statistics.deathStatisticsFox["Malnutrition"]},{Statistics.deathStatisticsFox["Old Age"]}");


        filePathFoxAll = Application.dataPath + "/fox_all_data.csv";
        filePathRabbitAll = Application.dataPath + "/rabbit_all_data.csv";
        File.WriteAllText(filePathRabbitAll, "Speed,Vision,Run,TTL\n");
        File.WriteAllText(filePathFoxAll, "Speed,Vision,Run,TTL\n");

        foreach (var fox in  Statistics.f.Keys) {
            var f = Statistics.f[fox];
            File.AppendAllText(filePathFoxAll, $"{f["speed"]},{f["vision"]},{f["run"]},{f["ttl"]}\n");

        }
        
        foreach (var rabbit in Statistics.r.Keys) {
            var r = Statistics.r[rabbit];
            File.AppendAllText(filePathRabbitAll, $"{r["speed"]},{r["vision"]},{r["run"]},{r["ttl"]}\n");
        }
        
    }
}