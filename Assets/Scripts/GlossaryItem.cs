using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GlossaryItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Transform prefabLockTransform;

    public void SetUp(string title, string description, GameObject item){
        titleText.text = title;
        descriptionText.text = description;
        GameObject spawnedItem = Instantiate(item, prefabLockTransform.position, Quaternion.identity, prefabLockTransform);
    }
}
