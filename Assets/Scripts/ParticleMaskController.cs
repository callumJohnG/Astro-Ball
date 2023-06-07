using UnityEngine;
 
public class ParticleMaskController : MonoBehaviour
{
    [SerializeField] private GameObject maskObject;
    private RectTransform maskRect;
    private ParticleSystem particles;
    private bool isPlaying;

    void Start()
    {
        maskRect = maskObject.GetComponent<RectTransform>();
        particles = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        Vector3[] corners = new Vector3[4];
        maskRect.GetWorldCorners(corners);

        Vector3 bottomLeft = corners[0];
        Vector3 topRight = corners[2];

        bool isInsideMask = particles.transform.position.x > bottomLeft.x
            && particles.transform.position.x < topRight.x
            && particles.transform.position.y > bottomLeft.y
            && particles.transform.position.y < topRight.y;

        if (isInsideMask && !isPlaying)
        {
            particles.Play();
            isPlaying = true;
        }
        else if (!isInsideMask && isPlaying)
        {
            particles.Stop();
            isPlaying = false;
        }
    }
}