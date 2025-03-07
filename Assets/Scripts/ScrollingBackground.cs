using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingBackground : MonoBehaviour
{
    RawImage myImage;
    [SerializeField] float panSpeed = 1;
    [SerializeField] Vector2 panAngle = Vector2.left;
    [SerializeField] float occilationOffset;
    [SerializeField] float occilationSpeed;
    [SerializeField] string textName;
    // Start is called before the first frame update
    void Start()
    {
        myImage = GetComponent<RawImage>();
        textName = myImage.material.name;
        panAngle =- new Vector2(Random.Range(-1f,1f),Random.Range(-1f,1f) );
    }

    // Update is called once per frame
    void Update()
    {
        myImage.uvRect = new Rect(myImage.uvRect.position + (panAngle * panSpeed * Time.deltaTime), myImage.uvRect.size);
    }
}
