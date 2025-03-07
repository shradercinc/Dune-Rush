using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected bool isOver = false;

    [SerializeField] protected Vector3 hoverSize;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnPointerEnter(PointerEventData data)
    {
        print("enter");
        isOver = true;
        transform.localScale = Vector3.one + hoverSize;


    }
    public void OnPointerExit(PointerEventData data)
    {
        print("exit");
        isOver = false;
        transform.localScale = Vector3.one;

    }

    // Update is called once per frame
    void Update()
    {


    }
}
