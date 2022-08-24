using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SwitchSession : MonoBehaviour
{
    [SerializeField] ARSession session;
    public void changeSession()
    {
        session.Reset();   
    }
}
