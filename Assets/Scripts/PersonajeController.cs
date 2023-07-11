using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonajeController : MonoBehaviour
{
    
     private int Vidas = 3;

     //float NivelPiso = -0.1f; 
     float Niveltecho = 2.99f;
     //float limiteR = 11.58f;
     //float limiteL = -11.56f;
     float velocidad = 5f;
     //float Alturasalto = 1.5f;
     float fuerzasalto = 30;
     bool Piso = true;

     private Rigidbody2D rb2d;
     private Animator animator;
     private SpriteRenderer spriteR;
     
     [SerializeField] private AudioSource Salto_SFX;
    // Start is called before the first frame update // Personaje iniciara en posicion X -8.57 Y 2.2
    void Start()
    {
        gameObject.transform.position = new Vector3(-7.56f, Niveltecho,0);
        Debug.Log("INT");
        Debug.Log("VIDAS");
        rb2d = GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        spriteR = GetComponent<SpriteRenderer>();
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
              animator.SetBool("Running", true);
              spriteR.flipX=false;
        }
        else if (Input.GetKey("left")){
              gameObject.transform.Translate(-velocidad*Time.deltaTime, 0, 0);
              animator.SetBool("Running", true);
              spriteR.flipX=true;
        } 

        if( !(Input.GetKey("right") || Input.GetKey("left")) ){
            animator.SetBool("Running", false);

        }

        if (Input.GetKeyDown(KeyCode.Space) && Piso)
        {
            Debug.Log("UP - Piso: " + Piso);
            rb2d.AddForce(new Vector2(0, -fuerzasalto*Physics2D.gravity[1]*rb2d.mass));
            Salto_SFX.Play();
            Piso = false;
            animator.SetBool("Suelo", true);
        }

      

        
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.transform.tag == "Ground"){
            Piso = true;
            Debug.Log("GROUND COLLISION");
            animator.SetBool("Suelo", false);
        }
        else if(collision.transform.tag == "Obstaculo"){
            Piso = true;
            Debug.Log("OBSTACLE COLLISION");
            animator.SetBool("Suelo", false);
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
