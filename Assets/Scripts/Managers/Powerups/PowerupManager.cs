using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{

    public static PowerupManager Instance;

    private void Awake(){
        Instance = this;
    }

    private void Update(){
        CheckPowerupDurations();
    }

    private List<Powerup> currentPowerups = new List<Powerup>();

    public void AddPowerup(PowerupType type){
        Powerup currentPowerup = GetPowerupOfType(type);
        if(currentPowerup == null){
            Powerup powerup = new Powerup(type, GameSettingsManager.Instance.powerupDuration + Time.time);
            currentPowerups.Add(powerup);
            ApplyPowerup(powerup);
            DisplayPowerup(powerup);
        } else {
            //Refresh the timer on our powerup
            currentPowerup.SetEndTime(GameSettingsManager.Instance.powerupDuration + Time.time);
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
        }
    }

    private void RemovePowerup(Powerup powerup){
        currentPowerups.Remove(powerup);
        switch(powerup.myType){
            case PowerupType.Bouncy : RemoveBouncy();break;
        }
    }

    private void DisplayPowerup(Powerup powerup){
        Debug.Log("POWER UP ADDED OF TYPE " + powerup.myType.ToString());
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

    private void ApplyBouncy(){
        playerObject.GetComponent<Collider2D>().sharedMaterial = bouncyPlayerMaterial;
    }

    private void RemoveBouncy(){
        playerObject.GetComponent<Collider2D>().sharedMaterial = normalPlayerMaterial;
    }

    #endregion

    #endregion
}
