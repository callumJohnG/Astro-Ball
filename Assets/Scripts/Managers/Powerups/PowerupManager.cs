using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerupManager : MonoBehaviour
{

    public static PowerupManager Instance;

    private void Awake(){
        Instance = this;
    }

    private void Start(){
        powerupAnimator = powerupText.GetComponent<Animator>();
        powerupTitle = powerupText.GetComponent<TextMeshProUGUI>();
        powerupText.SetActive(false);
    }

    private void Update(){
        CheckPowerupDurations();
    }

    private List<Powerup> currentPowerups = new List<Powerup>();
    [SerializeField] private GameObject powerupUIPrefab;
    [SerializeField] private Transform powerupContainer;
    [SerializeField] private GameObject powerupText;

    //Get a random powerup and add it
    public void AddPowerup(){
        AddPowerup(GetRandomPowerup());
    }

    private PowerupType GetRandomPowerup(){
        return (PowerupType)Random.Range(0, System.Enum.GetValues(typeof(PowerupType)).Length);
    }

    public void AddPowerup(PowerupType type){
        Powerup currentPowerup = GetPowerupOfType(type);
        if(currentPowerup == null){
            
            GameObject powerUpUI = Instantiate(powerupUIPrefab, Vector3.zero, Quaternion.identity);
            powerUpUI.transform.SetParent(powerupContainer, false);
            powerUpUI.GetComponent<PowerupUI>().SetData(GetPowerupSprite(type), Time.time, Time.time + GameSettingsManager.Instance.powerupDuration);

            Vector3 newPosition = powerUpUI.gameObject.transform.position;
            newPosition.z = 0;
            //powerUpUI.gameObject.transform.position = newPosition;

            Powerup powerup = new Powerup(type, GameSettingsManager.Instance.powerupDuration + Time.time, powerUpUI.GetComponent<PowerupUI>());
            currentPowerups.Add(powerup);
            ApplyPowerup(powerup);
            DisplayPowerup(powerup);
        } else {
            //Refresh the timer on our powerup
            currentPowerup.SetEndTime(GameSettingsManager.Instance.powerupDuration + Time.time);
            currentPowerup.powerupUI.SetTime(Time.time, Time.time + GameSettingsManager.Instance.powerupDuration);
            DisplayPowerup(currentPowerup);
        }

        CheckParticles();
    }

    private Powerup GetPowerupOfType(PowerupType type){
        foreach(Powerup powerup in currentPowerups){
            if(powerup.myType == type){
                return powerup;
            }
        }
        return null;
    }

    private Sprite GetPowerupSprite(PowerupType type){
        switch (type){
            case PowerupType.Bouncy : return bouncySprite;
            case PowerupType.Inverted : return invertedSprite;
            case PowerupType.Moon : return moonSprite;
            case PowerupType.Weak : return weakSprite;
            case PowerupType.Double_Points : return doublePointsSprite;
        }

        return null;
    }

    private void CheckPowerupDurations(){
        foreach(Powerup powerup in currentPowerups){
            if(powerup.IsPowerupDurationComplete()){
                RemovePowerup(powerup);
                return;
            }
        }
    }

    private void ApplyPowerup(Powerup powerup){
        switch(powerup.myType){
            case PowerupType.Bouncy : ApplyBouncy();break;
            case PowerupType.Inverted : ApplyInverted();break;
            case PowerupType.Moon : ApplyMoon();break;
            case PowerupType.Weak : ApplyWeak();break;
            case PowerupType.Double_Points : ApplyDoublePoints();break;
        }
    }

    private void RemovePowerup(Powerup powerup, bool remove = true){
        if(remove){
            AudioManager.Instance.PlayPowerDown();
            currentPowerups.Remove(powerup);
        }
        
        powerup.powerupUI.KillMe();
        switch(powerup.myType){
            case PowerupType.Bouncy : RemoveBouncy();break;
            case PowerupType.Inverted : RemoveInverted();break;
            case PowerupType.Moon : RemoveMoon();break;
            case PowerupType.Weak : RemoveWeak();break;
            case PowerupType.Double_Points : RemoveDoublePoints();break;
        }

        CheckParticles();
    }

    private void DisplayPowerup(Powerup powerup){
        AudioManager.Instance.PlayPowerUp();
        string powerupTitle = string.Join(" ", powerup.myType.ToString().Split("_"));
        PlayPowerupAnimation(powerupTitle);
    }

    public void ClearPowerupText(){
        powerupTitle.text = "";
    }

    public void WipeAllPowerups(){
        foreach(Powerup powerup in currentPowerups){
            RemovePowerup(powerup, false);
        }
        currentPowerups.Clear();
        CheckParticles();
    }

    #region Specific Powerups

    private GameObject playerObject;

    public void SetPlayerObject(GameObject playerObject){
        this.playerObject = playerObject;
    }

    #region Bouncy

    [Header("Bouncy")]
    [SerializeField] private PhysicsMaterial2D normalPlayerMaterial;
    [SerializeField] private PhysicsMaterial2D bouncyPlayerMaterial;
    [SerializeField] private Sprite bouncySprite;

    private void ApplyBouncy(){
        playerObject.GetComponent<Collider2D>().sharedMaterial = bouncyPlayerMaterial;
    }

    private void RemoveBouncy(){
        playerObject.GetComponent<Collider2D>().sharedMaterial = normalPlayerMaterial;
    }

    #endregion

    #region Inverted

    [Header("Inverted")]
    [SerializeField] private Sprite invertedSprite;

    private void ApplyInverted(){
        GameSettingsManager.Instance.flipAim = true;
    }

    private void RemoveInverted(){
        GameSettingsManager.Instance.flipAim = false;
    }

    #endregion

    #region Moon

    [Header("Moon")]
    [SerializeField] private Sprite moonSprite;
    private float normalGravity;
    [SerializeField] private float moonGravity;

    private void ApplyMoon(){
        normalGravity = playerObject.GetComponent<Rigidbody2D>().gravityScale;
        playerObject.GetComponent<Rigidbody2D>().gravityScale = moonGravity;
    }

    private void RemoveMoon(){
        playerObject.GetComponent<Rigidbody2D>().gravityScale = normalGravity;
    }

    #endregion

    #region Weak

    [Header("Weak")]
    [SerializeField] private Sprite weakSprite;
    private float normalForce;
    [SerializeField] private float weakForce;

    private void ApplyWeak(){
        normalForce = playerObject.GetComponent<PlayerController>().launchForce;
        playerObject.GetComponent<PlayerController>().launchForce = weakForce;
    }

    private void RemoveWeak(){
        playerObject.GetComponent<PlayerController>().launchForce = normalForce;
    }

    #endregion

    #region DoublePoints

    [Header("DoublePoints")]
    [SerializeField] private Sprite doublePointsSprite;
    private float normalMultiplier;
    [SerializeField] private float multiplier;

    private void ApplyDoublePoints(){
        normalMultiplier = GameSettingsManager.Instance.pointsMultiplier;
        GameSettingsManager.Instance.pointsMultiplier = multiplier;
    }

    private void RemoveDoublePoints(){
        GameSettingsManager.Instance.pointsMultiplier = normalMultiplier;
    }

    #endregion


    #endregion

    #region Animation


    private Animator powerupAnimator;
    private TextMeshProUGUI powerupTitle;
    private void PlayPowerupAnimation(string powerupString){
        powerupText.SetActive(true);
        StartCoroutine(CrossFadeAnimAsync(powerupString));
    }
    private IEnumerator CrossFadeAnimAsync(string powerupString) {
        powerupAnimator.CrossFade("default", 0.0f, 0); ///Crossfade to DefaultAnimationName
        yield return new WaitForEndOfFrame();
        powerupAnimator.CrossFade("GainPowerup", 0.0f, 0); ///Crossfade to the real AnimationName
        powerupTitle.text = powerupString;
    }

    #endregion

    #region Particles

    [SerializeField] private List<ParticleSystem> allParticles;

    private void CheckParticles(){
        bool shouldPlay = currentPowerups.Count > 0;

        foreach(ParticleSystem ps in allParticles){
            if(shouldPlay){
                ps.Play();
            } else {
                ps.Stop();
            }
        }

    }

    #endregion
}
