using UnityEngine;

public class ParallaxScriptRunning : MonoBehaviour
{
    private float length, startpos;
    public float parallaxSpeed = 1f;
    public float parallaxEffect = 0.5f;

    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float dist = Mathf.Repeat(Time.time * parallaxSpeed * parallaxEffect, length);
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);
    }
}