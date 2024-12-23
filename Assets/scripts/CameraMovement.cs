using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public float speed = 0.5f;
    [SerializeField] public float rotSpeed = 10f;
    void Update()
    {
        if(Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(speed * Time.deltaTime,0,0));
        }
        if(Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-speed * Time.deltaTime,0,0));
        }
        if(Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(0,0,-speed * Time.deltaTime));
        }
        if(Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(0,0,speed * Time.deltaTime));
        }
        if (Input.GetMouseButton(0))
        {
            transform.eulerAngles += rotSpeed* new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
        }
    }
}
