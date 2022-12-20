using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDottedLineRenderer : MonoBehaviour
{
    public static SimpleDottedLineRenderer Instance {get; private set;}

    private void Awake() {
        Instance = this;
    }


    [SerializeField] private Sprite Dot;
    [Range(0.01f, 1f)]
    [SerializeField] private float Size;
    [Range(0.1f, 10f)]
    [SerializeField] private float Delta;
    [SerializeField] private float Length = 10;
    [SerializeField] private Color dotColor;

    List<Vector2> positions = new List<Vector2>();
    List<GameObject> dots = new List<GameObject>();

    public void CreateNewLine(float startX, float endX){
        transform.position = Vector3.zero;
        DeleteAllDots();

        Vector3 startPoint = new Vector3(startX, transform.position.y, transform.position.z);
        Vector3 endPoint = new Vector3(endX, transform.position.y, transform.position.z);

        Vector2 direction = (endPoint - startPoint).normalized;

        Vector2 currentPoint = startPoint;
        positions.Add(currentPoint);

        for(int i = 0; i < Length; i++){
            currentPoint += (direction * Delta);
            positions.Add(currentPoint);
        }
        
        CreateDots();
    }

    private void CreateDots()
    {
        foreach (Vector2 position in positions)
        {
            CreateDot(position);
        }
    }

    private void CreateDot(Vector2 position)
    {
        GameObject dot = new GameObject();
        dot.transform.localScale = Vector3.one * Size;
        dot.transform.parent = transform;
        dot.transform.position = position;

        SpriteRenderer sr = dot.AddComponent<SpriteRenderer>();
        sr.sprite = Dot;
        sr.color = dotColor;

        dots.Add(dot);
    }

    private void DeleteAllDots(){
        foreach(GameObject dot in dots){
            Destroy(dot);
        }
        dots.Clear();
    }

    public void SetPosition(Vector3 position){
        transform.position = position;
    }

}
