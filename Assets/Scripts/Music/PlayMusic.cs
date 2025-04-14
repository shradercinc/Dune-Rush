using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    [SerializeField] AK.Wwise.Event MusicIntro;
    [SerializeField] AK.Wwise.Event MusicLoop;
    // Start is called before the first frame update
    void Start()
    {
        MusicIntro.Post(gameObject, (uint)AkCallbackType.AK_EndOfEvent, CallBackHitFunction);
    }

    void CallBackHitFunction(object in_cookie, AkCallbackType callType, object in_info)
    {
        if (callType == AkCallbackType.AK_EndOfEvent)
        {
            MusicLoop.Post(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
