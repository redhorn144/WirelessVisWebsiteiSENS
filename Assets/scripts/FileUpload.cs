using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
public class FileUpload : MonoBehaviour
{
    [DllImport("__Internal")] private static extern void onPointerDown();
    [SerializeField] TextMeshProUGUI tmp;

    public string data;
    public void Select()
    {
        onPointerDown();
    }

    void FileSelected(string url)
    {
        StartCoroutine(LoadFile(url));
    }

    void ChangeName(string name)
    {
        tmp.text = name;
    }

    IEnumerator LoadFile (string url) {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();
            data = webRequest.downloadHandler.text;
        }
    }

}
