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
    [SerializeField] private Transform tracker;
    private float centerX;

    List<Vector2> positions = new List<Vector2>();
    List<GameObject> dots = new List<GameObject>();

    private void Start() {
        centerX = transform.position.x;
    }

    private void Update() {
        UpdatePosition();
    }

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

    public void SetHeight(float height){
        Vector3 newPosition = transform.position;
        newPosition.y = height;
        transform.position = newPosition;
    }

    private void UpdatePosition(){
        if(tracker.position.x < (centerX - Delta)){
            centerX -= Delta;
        } else if (tracker.position.x > (centerX + Delta)){
            centerX += Delta;
        } else return;

        Vector3 newPos = transform.position;
        newPos.x = centerX;
        transform.position = newPos;
    }

}
