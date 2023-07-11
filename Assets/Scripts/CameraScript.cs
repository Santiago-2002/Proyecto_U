using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
   public GameObject Personaje; 

   
    void Update()
    {
        transform.position = new Vector3(Personaje.transform.position.x, transform.position.y, transform.position.z);
    }
}
