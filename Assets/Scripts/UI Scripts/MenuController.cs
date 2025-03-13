using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected bool isOver = false;

    [SerializeField] protected float hoverSize;
    protected Vector3 baseSize;

    // Start is called before the first frame update
    void Start()
    {
        print(transform.localScale);
        baseSize = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData data)
    {
        print("enter");
        isOver = true;
        transform.localScale = baseSize * hoverSize;


    }
    public void OnPointerExit(PointerEventData data)
    {
        print("exit");
        isOver = false;
        transform.localScale = baseSize;

    }

    // Update is called once per frame
    void Update()
    {


    }
}
