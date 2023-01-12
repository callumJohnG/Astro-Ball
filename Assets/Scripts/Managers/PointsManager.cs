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
    private int currentMultiplier = 1;
    private bool comboActive = false;
    private int comboPoints = 0;

    private float comboBasePitch = 1;
    [SerializeField] private float comboIncPitch = 0.1f;

    public void EndCombo(){
        if(!comboActive)return;

        if((comboPoints * currentMultiplier) > bestCombo)bestCombo = comboPoints * currentMultiplier;

        comboActive = false;

        //commit the combo points
        points += (comboPoints * currentMultiplier);

        //Reset all the combo values
        comboPoints = 0;
        currentMultiplier = 1;
        comboTitle.SetActive(false);
        comboText.gameObject.SetActive(false);

        UpdatePointsUI();
        AudioManager.Instance.PlayComboOver();
    }

    public void GainComboPoints(int quantity){
        //Multiply by base multiplier related to difficulty
        quantity = Mathf.FloorToInt(quantity * GameSettingsManager.Instance.pointsMultiplier);

        AudioManager.Instance.PlayBumper(comboBasePitch + (comboIncPitch * (currentMultiplier + 1)));

        if(!comboActive){
            comboActive = true;
        }
        currentMultiplier++;

        comboText.gameObject.SetActive(true);

        if(currentMultiplier >= 2) comboTitle.SetActive(true);

        comboPoints += quantity;

        comboParticles.Play();

        UpdatePointsUI();
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
}
