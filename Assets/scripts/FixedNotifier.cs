using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class FixedNotifier : MonoBehaviour
{
    // Start is called before the first frame update
    [DllImport("__Internal")] private static extern void FixedNotification();
    void Start()
    {
        FixedNotification();
    }
}
