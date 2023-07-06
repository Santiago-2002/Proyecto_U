using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonajeController : MonoBehaviour
{
    
     private int Vidas = 3;

     //float NivelPiso = -0.1f; 
     float Niveltecho = 2.99f;
     //float limiteR = 11.58f;
     float limiteL = -11.56f;
     float velocidad = 5f;
     float Alturasalto = 1.5f;
     float fuerzasalto = 30;
     bool Piso = true;
     
     [SerializeField] private AudioSource Salto_SFX;
    // Start is called before the first frame update // Personaje iniciara en posicion X -8.57 Y 2.2
    void Start()
    {
        gameObject.transform.position = new Vector3(-7.56f, Niveltecho,0);
        Debug.Log("INT");
        Debug.Log("VIDAS");
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.transform.rotation.z > 0.3 || gameObject.transform.rotation.z < -0.3){
            Debug.Log("ROTATION: " + gameObject.transform.rotation.z);
            gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }


        if(Input.GetKey("right")){
              gameObject.transform.Translate(velocidad*Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey("left") && gameObject.transform.position.x > limiteL){
              gameObject.transform.Translate(-velocidad*Time.deltaTime, 0, 0);
        } 

        if(Input.GetKeyDown("space") && Piso){
            Debug.Log("UP - Piso: " + Piso);
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -fuerzasalto*Physics2D.gravity[1]*gameObject.GetComponent<Rigidbody2D>().mass));
            Salto_SFX.Play();
            Piso = false;
        }

        
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.transform.tag == "Ground"){
            Piso = true;
            Debug.Log("GROUND COLLISION");
        }
        else if(collision.transform.tag == "Obstaculo"){
            Piso = true;
            Debug.Log("OBSTACLE COLLISION");
        }
    }

     private void OnTriggerEnter2D(Collider2D collision){
        Debug.Log("Caida");
        Vidas -= 1;
        Debug.Log("VIDAS: " + Vidas);
        if(Vidas <= 0){
            Debug.Log("GAME OVER");
            Vidas = 3;
        }
        gameObject.transform.position = new Vector3(-7.56f, Niveltecho,0);

     } 
        
    
}
