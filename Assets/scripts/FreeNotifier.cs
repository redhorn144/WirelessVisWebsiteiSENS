using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class FreeNotifier : MonoBehaviour
{
    [DllImport("__Internal")] private static extern void FreeNotification();
    void Start()
    {
        FreeNotification();
    }

    // Update is called once per frame

}
