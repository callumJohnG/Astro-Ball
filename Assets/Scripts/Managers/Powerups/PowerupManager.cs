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

    public void AddPowerup(PowerupType type){
        Powerup currentPowerup = GetPowerupOfType(type);
        if(currentPowerup == null){
            
            GameObject powerUpUI = Instantiate(powerupUIPrefab, transform.position, transform.rotation, powerupContainer);
            powerUpUI.GetComponent<PowerupUI>().SetData(GetPowerupSprite(type), Time.time, Time.time + GameSettingsManager.Instance.powerupDuration);

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
        }
    }

    private void RemovePowerup(Powerup powerup, bool remove = true){
        Debug.Log("REMOVING POWERUP");

        if(remove){
            currentPowerups.Remove(powerup);
            AudioManager.Instance.PlayPowerDown();
        }

        powerup.powerupUI.KillMe();
        switch(powerup.myType){
            case PowerupType.Bouncy : RemoveBouncy();break;
            case PowerupType.Inverted : RemoveInverted();break;
        }
    }

    private void DisplayPowerup(Powerup powerup){
        AudioManager.Instance.PlayPowerUp();
        Debug.Log("POWER UP ADDED OF TYPE " + powerup.myType.ToString());
        PlayPowerupAnimation(powerup.myType.ToString());
    }

    public void ClearPowerupText(){
        powerupTitle.text = "";
    }

    public void WipeAllPowerups(){
        foreach(Powerup powerup in currentPowerups){
            RemovePowerup(powerup, false);
        }
        currentPowerups.Clear();
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

    [Header("Bouncy")]
    [SerializeField] private Sprite invertedSprite;

    private void ApplyInverted(){
        GameSettingsManager.Instance.inverseAiming = true;
    }

    private void RemoveInverted(){
        GameSettingsManager.Instance.inverseAiming = false;
    }

    #endregion


    #endregion

    #region Animation


    private Animator powerupAnimator;
    private TextMeshProUGUI powerupTitle;
    private void PlayPowerupAnimation(string powerupString){
        powerupText.SetActive(true);
        powerupTitle.text = powerupString;
        powerupAnimator.CrossFade("GainPowerup", 0, 0);
    }

    #endregion
}
