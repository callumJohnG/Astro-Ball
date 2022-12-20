using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverObject : MonoBehaviour
{

    private Vector3 startPosition;
    [SerializeField] private float hoverBobSpeed;
    [SerializeField] private float hoverBobDistance;
    [SerializeField] private float spinSpeed;

    // Start is called before the first frame update
    void Awake()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Bob
        float bobHeight = Mathf.Sin(Time.time * hoverBobSpeed) * hoverBobDistance;
        transform.position = startPosition + new Vector3(0, bobHeight);

        //Spin
        float rotateAngle = Time.deltaTime * spinSpeed;
        transform.Rotate(new Vector3(0, 0, rotateAngle));
    }
}
