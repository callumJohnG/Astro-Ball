using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerXPManager : MonoBehaviour
{

    public static PlayerXPManager Instance {get; private set;}

    private void Awake() {
        Instance = this;
        InitialiseXP();
    }

    private void Start() {
        CreateSegments();
        //TestSegments();
    }

    [SerializeField] private float pointsPerXP = 150; //The amount of points needed to gain 1 xp
    private const string XP_KEY = "xp";

    //Stores all the text components in the game that display the player's currency
    [SerializeField] private List<TextMeshProUGUI> playerXPTexts;
    [SerializeField] private TextMeshProUGUI gainedXPText;

    [SerializeField] private GainXPAnimation xpAnimation;
    [SerializeField] private List<PointSegment> segments;

    private void InitialiseXP(){
        int currentXP = PlayerPrefs.GetInt(XP_KEY, 0);
        PlayerPrefs.SetInt(XP_KEY, currentXP);
        bool cheat = true;
        if(cheat){
            PlayerPrefs.SetInt(XP_KEY, 2000);
        }
        UpdateTexts();
    }

    public bool SpendXP(int value){
        int currentXP = PlayerPrefs.GetInt(XP_KEY);
        if(currentXP < value) return false;

        currentXP -= value;
        PlayerPrefs.SetInt(XP_KEY, currentXP);
        UpdateTexts();
        Debug.Log("Spent " + value + " coins - new value:" + GetXP());
        return true;
    }

    public void GainXP(int value){
        int currentXP = PlayerPrefs.GetInt(XP_KEY);
        currentXP += value;
        PlayerPrefs.SetInt(XP_KEY, currentXP);
        UpdateTexts();
    }

    public int GetXP(){
        return PlayerPrefs.GetInt(XP_KEY, 0);
    }

    public void CalculateXPGain(int points){
        /*Debug.Log("Gaining xp from " + points + " points");
        int newXP = Mathf.FloorToInt(points/pointsPerXP);

        xpAnimation.SetUpValues(newXP, GetXP());
        xpAnimation.PlayXPAnimation();

        GainXP(newXP);*/

        int newXP = CalculateXPGainFromSegments(points);
        xpAnimation.SetUpValues(newXP, GetXP());
        xpAnimation.PlayXPAnimation();
        GainXP(newXP);

        //gainedXPText.text = "Gained +" + newXP + " coins";
    }

    private void CreateSegments(){
        segments = new List<PointSegment>();
        segments.Add(new PointSegment(0, 500, 30));
        segments.Add(new PointSegment(501, 1000, 70));
        segments.Add(new PointSegment(1001, 2500, 100));
        segments.Add(new PointSegment(2501, 5000, 180));
        segments.Add(new PointSegment(5001, 10000, 400));
        segments.Add(new PointSegment(10001, Mathf.Infinity, 800));
    }

    public int CalculateXPGainFromSegments(int points){
        int remainder = points;
        int result = 0;
        foreach(PointSegment seg in segments){
            if(points < seg.minRange) return result;

            if(points < seg.maxRange){
                //We end on this loop
                //Get the number of points inside this segment
                int pointsInSegment = points - (int)seg.minRange;
                int segmentResult = Mathf.FloorToInt(pointsInSegment / seg.pointsPerXP);
                result += segmentResult;
                return result;
            } else {
                int pointsInSegment = (int)seg.maxRange - (int)seg.minRange;
                int segmentResult = Mathf.FloorToInt(pointsInSegment / seg.pointsPerXP);
                result += segmentResult;
            }
        }

        return result;
    }

    private void TestSegments(){
        List<int> testPoints = new List<int>{80240};
        foreach(int point in testPoints){
            Debug.Log(point + ":" + CalculateXPGainFromSegments(point));
        }
    }

    private void UpdateTexts(){
        foreach(TextMeshProUGUI display in playerXPTexts){
            display.text = GetXP().ToString();
        }
    }
}

public struct PointSegment{
    public float minRange;
    public float maxRange;
    public int pointsPerXP; 

    public PointSegment(float minRange, float maxRange, int pointsPerXP){
        this.minRange = minRange;
        this.maxRange = maxRange;
        this.pointsPerXP = pointsPerXP;
    }
}