using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightPoints : MonoBehaviour
{

    [SerializeField] private int pointsPerUnit;
    [SerializeField] private float unitHeight;

    private float maxHeight;
    private float heightRemainder = 0;

    private void Start() {
        maxHeight = transform.position.y;

        SimpleDottedLineRenderer.Instance.CreateNewLine(-lineWidth, lineWidth);
    }

    void Update()
    {
        if(!GameplayManager.Instance.gameIsActive)return;

        CheckHeight();
        DrawHeightLine();
    }

    private void CheckHeight(){
        float currentY = transform.position.y;
        if(currentY <= maxHeight){
            return;
        }

        //Get the difference between past height and currentHeight
        float heightDifference = currentY - maxHeight;
        maxHeight = currentY;

        //Add on the remainder from previous frame
        heightDifference += heightRemainder;

        //Calculate how many segments we have crossed with this height difference
        int unitsPassed = Mathf.FloorToInt(heightDifference / unitHeight);

        //Add points equal to units passed
        if(unitsPassed != 0){
            PointsManager.Instance.GainPoints(unitsPassed * pointsPerUnit);
        }

        //Set the next remainder for next frame
        heightRemainder = heightDifference % unitHeight;
    }

    private float lineWidth = 200;

    private void DrawHeightLine(){
        SimpleDottedLineRenderer.Instance.SetHeight(maxHeight);
    }
}
