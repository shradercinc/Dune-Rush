using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuConStart : MenuController
{
    [SerializeField] string SceneName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isOver && Input.GetKeyUp(KeyCode.Mouse0))
        { 
            SceneManager.LoadScene(SceneName);
        }
    }
}
