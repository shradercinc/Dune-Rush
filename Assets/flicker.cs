using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class flicker : MonoBehaviour
{
    [SerializeField] float flickerTimerMaxOn;
    [SerializeField] float flickerTimerMaxOff;
    [SerializeField] List<Image> FAssets = new List<Image>();
    float Ftimer;
    bool visible = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ftimer += Time.deltaTime;

        if (Ftimer >= flickerTimerMaxOn && visible)
        {
            visible = false;
            foreach (var f in FAssets) { f.enabled = false; }
            Ftimer = 0;
        }
        if (Ftimer >= flickerTimerMaxOff && !visible)
        {
            visible = true;
            foreach (var f in FAssets) { f.enabled = true; }
            Ftimer = 0;
        }

    }
}
