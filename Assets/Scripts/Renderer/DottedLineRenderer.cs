using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DottedLineRenderer : MonoBehaviour
{
    // Inspector fields
    [SerializeField] private Sprite Dot;
    [Range(0.01f, 1f)]
    [SerializeField] private float Size;
    [Range(0.1f, 10f)]
    [SerializeField] private float Delta;

    //Static Property with backing field
    private static DottedLineRenderer instance;
    public static DottedLineRenderer Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<DottedLineRenderer>();
            return instance;
        }
    }

    //Utility fields
    List<Vector2> positions = new List<Vector2>();
    List<GameObject> dots = new List<GameObject>();

    // Update is called once per frame
    void FixedUpdate()
    {
        if (positions.Count > 0)
        {
            DestroyAllDots();
            positions.Clear();
        }
    }

    private void DestroyAllDots()
    {
        foreach (var dot in dots)
        {
            Destroy(dot);
        }
        dots.Clear();
    }

    GameObject GetOneDot()
    {
        var gameObject = new GameObject();
        gameObject.transform.localScale = Vector3.one * Size;
        gameObject.transform.parent = transform;

        var sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = Dot;
        return gameObject;
    }

    public void DrawDottedLine(Vector2 start, Vector2 end)
    {
        DestroyAllDots();

        Vector2 point = start;
        Vector2 direction = (end - start).normalized;

        while ((end - start).magnitude > (point - start).magnitude)
        {
            positions.Add(point);
            point += (direction * Delta);
        }

        Render();
    }

    private void Render()
    {
        foreach (var position in positions)
        {
            var g = GetOneDot();
            g.transform.position = position;
            dots.Add(g);
        }
    }
}