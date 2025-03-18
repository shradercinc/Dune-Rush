using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class FakePartical : MonoBehaviour
{
    [SerializeField] float offset;
    [SerializeField] float offsetRotation;
    [SerializeField] float amount;
    public bool isOrigin = false;

    // Start is called before the first frame update
    void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (isOrigin) 
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject myParticle = Instantiate(gameObject, transform.position, Quaternion.identity);
                myParticle.GetComponent<FakePartical>().isOrigin = false;
                RectTransform newTrans = myParticle.GetComponent<RectTransform>();
                newTrans.SetParent(rectTransform);
                newTrans.position = new Vector3(newTrans.position.x + Random.Range(offset,-offset), newTrans.position.y + Random.Range(offset, -offset), newTrans.position.z + Random.Range(offset, -offset));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
