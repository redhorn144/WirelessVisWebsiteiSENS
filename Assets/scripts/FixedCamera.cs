using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCamera : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject[] cameras;
    private int active = 0;
    private int numCameras;
    void Awake()
    {
        numCameras = cameras.Length;
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            active = (active + 1) % numCameras; 
            ChangeActive(active);
            Debug.Log(active);  
        }
    }

    void ChangeActive(int nextOne)
    {
        for (int i = 0; i < numCameras; i++)
        {
            if (i == nextOne)
            {
                cameras[i].SetActive(true);
            }
            else
            {
                cameras[i].SetActive(false);
            }
        }
        
    }
}
