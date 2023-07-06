using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
   public GameObject Player1; 

   
    void Update()
    {
        transform.position = new Vector3(Player1.transform.position.x, transform.position.y, transform.position.z);
    }
}
