using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableOnStart : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }
}
