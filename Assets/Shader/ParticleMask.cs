using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleMask : MonoBehaviour
{
    public RectMask2D mask;
    public Material mt;
    private void Awake()
    {
        mt = GetComponent<ParticleSystem>().GetComponent<Renderer>().material;
        mask = GetComponentInParent<RectMask2D>();
        // Recalculate the crop area when the ScrollView position changes
        GetComponentInParent<ScrollRect>().onValueChanged.AddListener((e) => { setClip(); });
        setClip();
    }
    
    void setClip()
    {
        Vector3[] wc = new Vector3[4];
                mask.GetComponent<RectTransform>().GetWorldCorners(wc); // Calculate the point coordinates in world space
                Vector4 clipRect = new Vector4(wc[0].x, wc[0].y, wc[2].x, wc[2].y);// Select the lower left and upper right corners
                mt.SetVector("_ClipRect", clipRect); // Set the crop area
                mt.SetFloat("_UseClipRect", 1.0f); // Turn on cropping
    }
}