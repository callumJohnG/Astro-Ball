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
    private int points;
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
        comboActive = false;

        //commit the combo points
        points += (comboPoints * currentMultiplier);

        //Reset all the combo values
        comboPoints = 0;
        currentMultiplier = 1;
        comboTitle.SetActive(false);
        comboText.gameObject.SetActive(false);

        UpdatePointsUI();
    }

    public void GainComboPoints(int quantity){
        AudioManager.Instance.PlayBumper(comboBasePitch + (comboIncPitch * currentMultiplier));

        if(comboActive){
            currentMultiplier++;
        } else {
            comboActive = true;
        }

        comboText.gameObject.SetActive(true);

        if(currentMultiplier >= 2) comboTitle.SetActive(true);

        comboPoints += quantity;

        comboParticles.Play();

        UpdatePointsUI();
    }

    public void GainPoints(int quantity){
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
