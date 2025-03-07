using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillParticle : MonoBehaviour
{
    [SerializeField] float maxSize, minSize, drag, alphaMax, alphaMin, alphaDegrade, speed;
    Vector3 velocity;
    SpriteRenderer ren;
    // Start is called before the first frame update
    void Start()
    {
        ren = GetComponent<SpriteRenderer>();
        ren.color = new Color(ren.color.r, ren.color.g, ren.color.b,Random.Range(alphaMin,alphaMax));
        transform.localScale = Vector3.one * Random.Range(minSize,maxSize);
        velocity = new Vector3(Random.Range(-1f, 1f) * speed, Random.Range(-1f, 1f) * speed, Random.Range(-1f, 1f) * speed);

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + velocity * Time.deltaTime;
        velocity *= drag;
        ren.color = new Color(ren.color.r, ren.color.g, ren.color.b, ren.color.a - alphaDegrade);
        if (ren.color.a <= 0) Destroy(this.gameObject);
    }
}
