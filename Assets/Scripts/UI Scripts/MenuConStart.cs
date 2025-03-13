using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuConStart : MenuController
{
    [SerializeField] string SceneName;
    // Update is called once per frame
    void Update()
    {
        if (isOver && Input.GetKeyUp(KeyCode.Mouse0))
        { 
            SceneManager.LoadScene(SceneName);
        }
    }
}
