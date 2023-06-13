using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glossary : MonoBehaviour
{
    [SerializeField] private GlossaryItem glossaryItemObject;
    [SerializeField] private GameObject glossarySpacer;
    [SerializeField] private List<GlossaryData> glossaryDatas;
    [SerializeField] private Transform contentContainer;

    private void Awake() {
        GameObject recentSpacer = null;
        foreach(GlossaryData glossaryData in glossaryDatas){
            GlossaryItem spawnedGlossaryItem = Instantiate(glossaryItemObject, Vector3.zero, Quaternion.identity, contentContainer);
            spawnedGlossaryItem.gameObject.SetActive(true);
            spawnedGlossaryItem.SetUp(glossaryData.title, glossaryData.description, glossaryData.itemPrefab);

            //Spawn a blocker
            recentSpacer = Instantiate(glossarySpacer, Vector3.zero, Quaternion.identity, contentContainer);
            recentSpacer.SetActive(true);
        }

        //Delete the bottom spacer
        if(recentSpacer != null){
            Destroy(recentSpacer);
        }
    }


}

[System.Serializable]
public struct GlossaryData {
    public string title;
    public string description;
    public GameObject itemPrefab;
}
