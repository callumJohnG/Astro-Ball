using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointsManager : MonoBehaviour
{

    public static PointsManager Instance {get;private set;}

    // Start is called before the first frame update
    void Awake(){
        Instance = this;
    }


    private void Start(){
        UpdatePointsUI();
    }

    private void Update(){
        CheckCombo();
    }

    //I would like to make a combo system where you gain multipliers for many points gained in short succession
    public int points {get;private set;} = 0;
    private int xpPoints = 0;
    public int bestCombo {get; private set;} = 0;

    public void ResetPoints(){
        points = 0;
        bestCombo = 0;
        xpPoints = 0;

        comboActive = false;
        comboPoints = 0;
        currentMultiplier = 1;
        comboTitle.SetActive(false);
        comboText.gameObject.SetActive(false);


        UpdatePointsUI();
    }

    public void SubmitPointsToXP(){
        PlayerXPManager.Instance.CalculateXPGain(points - xpPoints);
        xpPoints = points;
    }

    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private GameObject comboTitle;
    [SerializeField] private Animator pointsAnimtor;
    [SerializeField] private ParticleSystem comboParticles;
    [SerializeField] private float comboTimeMax = 4f;
    private float currentComboTime;
    private int currentMultiplier = 1;
    private int comboPitchCounter = 1;
    private bool comboActive = false;
    private int comboPoints = 0;

    private float comboBasePitch = 1;
    [SerializeField] private float comboIncPitch = 0.1f;

    public void EndCombo(bool playSound = true){
        if(!comboActive)return;

        if((comboPoints * currentMultiplier) > bestCombo)bestCombo = comboPoints * currentMultiplier;

        comboActive = false;

        //commit the combo points
        points += (comboPoints * currentMultiplier);

        //Reset all the combo values
        comboPoints = 0;
        currentMultiplier = 1;
        comboPitchCounter = 1;
        comboTitle.SetActive(false);
        comboText.gameObject.SetActive(false);

        UpdatePointsUI();

        if(playSound){
            AudioManager.Instance.PlayComboOver();
        }
    }

    public void ResetComboPitchCounter(){
        comboPitchCounter = 1;
    }

    public void GainComboPoints(int quantity){
        //Multiply by base multiplier related to difficulty
        quantity = Mathf.FloorToInt(quantity * GameSettingsManager.Instance.pointsMultiplier);

        //AudioManager.Instance.PlayBumper(comboBasePitch + (comboIncPitch * (currentMultiplier + 1)));
        AudioManager.Instance.PlayBumper(comboPitchCounter);

        if(!comboActive){
            //Start the combo
            comboActive = true;
        }

        currentComboTime = comboTimeMax;

        currentMultiplier++;
        comboPitchCounter++;

        comboText.gameObject.SetActive(true);

        if(currentMultiplier >= 2) comboTitle.SetActive(true);

        comboPoints += quantity;

        comboParticles.Play();

        UpdatePointsUI();
    }



    private void CheckCombo(){
        if(!comboActive)return;

        currentComboTime -= Time.deltaTime;

        if(currentComboTime <= 0){
            EndCombo();
        }
    }

    public void GainPoints(int quantity){
        quantity = Mathf.FloorToInt(quantity * GameSettingsManager.Instance.pointsMultiplier);
        points += quantity;
        pointsAnimtor.CrossFade("GainPoints", 0, 0);
        UpdatePointsUI();
    }

    private void UpdatePointsUI(){
        pointsText.text = points.ToString();

        if(comboActive){
            comboText.text = comboPoints + " X" + currentMultiplier;
        }
    }

    public float GetComboTime(){
        if(!comboActive) return 0;
        else {
            return currentComboTime / comboTimeMax;
        }
    }
}
