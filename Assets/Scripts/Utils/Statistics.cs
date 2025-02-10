using System.Collections;
using System.Collections.Generic;
using Animal;
using UnityEngine;

public static class Statistics {
    public static int countFox = 0;
    public static int countRabbit = 0;
    public static int countTotalRabbit = 0;
    public static int countTotalFox = 0;
    public static int rabbitDeaths = 0;
    public static int rabbitBirths = 0;
    public static int foxDeaths = 0;
    public static int foxBirths = 0;
    public static int plantCount = 0;

    public static Dictionary<int, Dictionary<string, float>> f = new Dictionary<int, Dictionary<string, float>>();
    public static Dictionary<int, Dictionary<string, float>> r = new Dictionary<int, Dictionary<string, float>>();

    public static Dictionary<string, int> deathStatisticsRabbit = new Dictionary<string, int> {
        { "Malnutrition", 0 },
        { "Predation", 0 },
        { "Old Age", 0 }
    };
    
    public static Dictionary<string, int> deathStatisticsFox = new Dictionary<string, int> {
        { "Malnutrition", 0 },
        { "Old Age", 0 }
    };
}