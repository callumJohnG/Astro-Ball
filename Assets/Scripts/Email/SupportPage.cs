using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SupportPage : MonoBehaviour
{


    [SerializeField] private TMP_InputField messageField;
    [SerializeField] private TextMeshProUGUI responseText;

    private const string RESPONSE_MESSAGE_SENT = "Message sent successfully!";
    private const string RESPONSE_TOO_SOON = "Please wait before sending another message, thanks!";
    private const string RESPONSE_MESSAGE_FAILED = "Could not send message, make sure you are connected to the internet.";

    private const string EMAIL_TIME_KEY = "TimeOfLastEmail";
    private const float sendInterval = 600;
    private bool canSendEmail = false;

    public void TrySendMail(){
        string messageBody = messageField.text;
        if(string.IsNullOrEmpty(messageBody))return;

        CheckCanSendEmail();
        if(!canSendEmail)return;

        messageBody = AttachPlayerInfo(messageBody);

        try{  
            //Send the email
            EmailSender.SendEmail(messageBody, "AstroBall In-Game Support Message");

            //Email was sent sucessfully
            //Wipe the input field
            messageField.text = "";
            
            SetEmailTime();
            responseText.text = RESPONSE_MESSAGE_SENT;
        } catch {
            Debug.LogError("Email could not be sent");
            responseText.text = RESPONSE_MESSAGE_FAILED;
        }
    }

    private void SetEmailTime(){
        PlayerPrefs.SetFloat(EMAIL_TIME_KEY, GetTimeInSeconds());
    }

    private float GetTimeInSeconds(){
        return (float)(System.DateTime.UtcNow - new System.DateTime(1970, 1, 1)).TotalSeconds;
    }

    public void CheckCanSendEmail(){
        float timeOfLastEmail = PlayerPrefs.GetFloat(EMAIL_TIME_KEY, 0);
        if(GetTimeInSeconds() < timeOfLastEmail + sendInterval){
            //Cant send the email - user needs to wait

            canSendEmail = false;
            responseText.text = RESPONSE_TOO_SOON;
        } else {
            canSendEmail = true;
            responseText.text = "";
        }
    }

    private string AttachPlayerInfo(string message){
        string newMessage = "";

        newMessage += "Message sent from In-Game contact screen.\n";
        newMessage += "-------------------------------------------\n";
        newMessage += "Player Name : " + PlayerPrefs.GetString("Name") + "\n";
        newMessage += "Player ID : " + PlayerPrefs.GetString("PlayerID") + "\n";
        newMessage += "-------------------------------------------\n";
        newMessage += "Message body : \n\n\n";
        newMessage += message;
        newMessage += "\n\n\n-------------------------------------------\n";

        newMessage += "This email was automatically sent from within the AstroBall app.";


        return newMessage;
    }

}
