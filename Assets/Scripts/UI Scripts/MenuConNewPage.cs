using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuConNewPage : MenuController
{
    [SerializeField] GameObject MenuToVanish;
    [SerializeField] GameObject MenuToAppear;
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (isOver && Input.GetKeyUp(KeyCode.Mouse0))
        { 
            MenuToVanish.SetActive(false);
            MenuToAppear.SetActive(true);
        }
    }
}
