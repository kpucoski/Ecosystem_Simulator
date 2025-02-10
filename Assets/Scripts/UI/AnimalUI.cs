using Animal;
using Animal.AnimalStates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class AnimalUI : MonoBehaviour {
    public Image healthBar;
    public Image hungerBar;
    public Image thirstBar;
    public Image reproductionBar;
    public Image staminaBar;
    public Image genderColor;
    private AbstractAnimal _animal;

    public TextMeshProUGUI health;
    public TextMeshProUGUI hunger;
    public TextMeshProUGUI thirst;
    public TextMeshProUGUI reproduction;
    public TextMeshProUGUI state;
    public TextMeshProUGUI stamina;
    public TextMeshProUGUI gender;
    
    private Color32 darkOrange;
    private Color32 darkRed;
    private Color32 blue;
    private Color32 pink;
    private Color32 yellow;

    #region EnableDisable
    void OnEnable() {
        Ticker.OnTickAction += Tick;
    }

    void OnDisable() {
        Ticker.OnTickAction -= Tick;

    }
    #endregion
    void Start() {
        _animal = GetComponentInParent<AbstractAnimal>();
        darkOrange = new Color32(211,86,16,255);
        darkRed = new Color32(190,18,18,255);
        blue = new Color32(27, 142, 199,255);
        pink = new Color32(220, 13, 173, 255);
        yellow = new Color32(220,184,13,255);
        genderColor.color = _animal.gender == Gender.Male ? Color.blue : Color.magenta;
        gender.text = _animal.gender == Gender.Male ? "♂" : "♀";
    }

    // void Update() {
    //     UpdateBars();
    // }
    
    private void Tick() {
        UpdateBars();
    }
    

    void UpdateBars() {
        healthBar.fillAmount = _animal.health / 100f;
        hungerBar.fillAmount = 1f - (_animal.hunger / 100f);
        thirstBar.fillAmount = 1f - (_animal.thirst / 100f);
        reproductionBar.fillAmount = 1f - (_animal.reproduction / 100f);
        staminaBar.fillAmount = (_animal.stamina / 100f);

        health.text = $"{Mathf.RoundToInt(_animal.health)} / 100";
        hunger.text = $"{Mathf.RoundToInt(_animal.hunger)} / 100";
        thirst.text = $"{Mathf.RoundToInt(_animal.thirst)} / 100";
        reproduction.text = $"{Mathf.RoundToInt(_animal.reproduction)} / 100";
        stamina.text = $"{Mathf.RoundToInt(_animal.stamina)} / 100";
        string t;
        var c = Color.white;
        switch (_animal.currState) {
            case States.Wander:
                t = "Wandering";
                c = Color.white;
                break;
            case States.Eat:
                t = "Seeking Food";
                c = darkOrange;
                break;
            case States.Drink:
                t = "Seeking Water";
                c = blue;
                break;
            case States.Mate:
                t = "Seeking Mate";
                c = pink;
                break;
            case States.Birth:
                t = "Giving Birth";
                c = darkRed;
                break;
            case States.Hunt:
                t = "Hunting";
                c = yellow;
                break;
            case States.Flee:
                t = "Fleeing";
                c = yellow;
                break;
            default:
                t = "Idle";
                break;
        }

        state.text = t;
        state.color = c;
    }
}