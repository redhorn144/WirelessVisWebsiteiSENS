using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.UI;
/********************************************************************************************
public class CanvasSelector : MonoBehaviour {
    [DllImport("__Internal")]
    private static extern void ImageUploaderInit();
    [DllImport("__Internal")] private static extern void Hello();

    public string data;
    public static CanvasSelector Instance;
    IEnumerator LoadTexture (string url) {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();
            data = webRequest.downloadHandler.text;
            Hello();
        }
    }

    void FileSelected (string url) {
        StartCoroutine(LoadTexture (url));
    }

    void Start () {
        ImageUploaderInit ();
    }
}
*/

public class CanvasSelector : MonoBehaviour {

    [DllImport("__Internal")] private static extern void ImageUploaderCaptureClick();

    [DllImport("__Internal")] private static extern void Hello();
    public string data;
    
    IEnumerator LoadTexture (string url) {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();
            data = webRequest.downloadHandler.text;
            Hello();
        }
    }

    void FileSelected (string url) {
        StartCoroutine(LoadTexture (url));
    }

    public void OnButtonPointerDown () {
        ImageUploaderCaptureClick();
    }
}
