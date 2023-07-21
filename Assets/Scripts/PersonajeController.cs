using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonajeController : MonoBehaviour
{
    
     private int Vidas = 3;

     //float NivelPiso = -0.1f; 
     float Niveltecho = 2.99f;
     float velocidad = 5f;
     float fuerzaImpulso = 20000;
     float fuerzasalto = 30;
     bool Piso = false;
     bool hasJump = false;

    bool enElMuroL  = false; // Bandera que verifica que el personaje ha tocado el muro izquierdo
    bool enElMuroR  = false; // Bandera que verifica que el personaje ha tocado el muro derecho


     private Rigidbody2D rb2d;
     private Animator animator;
     private SpriteRenderer spriteR;
     private RaycastHit2D HitL, HitR;
     
     [SerializeField] private AudioSource Salto_SFX;
     [SerializeField] private LayerMask rayMask;

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

        Debug.DrawRay(transform.position, 0.7f*transform.right, Color.red);
        Debug.DrawRay(transform.position, -0.7f*transform.right, Color.red);

        HitR = Physics2D.Raycast(transform.position, transform.right, 0.7f, rayMask);
        HitL = Physics2D.Raycast(transform.position, transform.right, -0.7f, rayMask);

        /*if(gameObject.transform.rotation.z > 0.3 || gameObject.transform.rotation.z < -0.3){
            Debug.Log("ROTATION: " + gameObject.transform.rotation.z);
            gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }*/


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

        // ** Detector de movimiento descendente **
        //    Sirve para cambiar la animación del personaje y como
        //    límite para realizar un segundo salto
        if(rb2d.velocity.y < -0.1){
            hasJump = false;
            animator.SetBool("Falling", true);
            animator.SetBool("Jump", false);
            animator.SetBool("DoubleJump", false);
        }

        /*if (Input.GetKeyDown(KeyCode.Space) && Piso)
        {
            Debug.Log("UP - Piso: " + Piso);
            rb2d.AddForce(new Vector2(0, -fuerzasalto*Physics2D.gravity[1]*rb2d.mass));
            Salto_SFX.Play();
            Piso = false;
            animator.SetBool("Jump", true);
        }*/

        if((Input.GetKeyDown("space") && Piso)||(Input.GetKeyDown("space") && hasJump)){
            Debug.Log("UP - Piso: " + Piso);
            if(hasJump){
                // Esto se ejecuta cuando YA HA SALTADO por primera vez
                animator.SetBool("DoubleJump", true);
                hasJump  = false;
                float d_i = 1;
                if(rb2d.velocity.x < 0) d_i = -1; // ¿El personaje va para la derecha o la izquierda?
                //fuerza vertical y horizontal - como el personaje está en el aire es necesario imprimirle también fuerza horizontal
                rb2d.AddForce(new Vector2(d_i*fuerzaImpulso, -0.5f*fuerzasalto*Physics2D.gravity[1]*rb2d.mass));
            }
            else{
                // Esto se ejecuta cuando es el PRIMER SALTO
                Salto_SFX.Play();
                hasJump  = true;
                animator.SetBool("Jump", true);
                animator.SetBool("DoubleJump", false);
                //fuerza vertical - el desplazamiento horizontal lo da la inercia que lleve el personaje
                rb2d.AddForce(new Vector2(0, -fuerzasalto*Physics2D.gravity[1]*rb2d.mass));
            }
            Piso = false;
        }


        // Implementación del salto del muro
        if(Input.GetKeyDown("space") && (enElMuroL || enElMuroR)){
            Debug.Log("WALL JUMP");
            animator.SetBool("Jump", true);
            if(enElMuroL){
                rb2d.AddForce(new Vector2(fuerzaImpulso, -0.5f*fuerzasalto*Physics2D.gravity[1]*rb2d.mass));
                enElMuroL = false;
            }
            else{
                rb2d.AddForce(new Vector2(-fuerzaImpulso, -0.5f*fuerzasalto*Physics2D.gravity[1]*rb2d.mass));
                enElMuroR = false;
            }            
        }

        // Personaje tocando un muro
        if(HitL.collider != null){ // izquierdo
            Debug.Log("WALL LEFT");
            rb2d.gravityScale = 0.1f;
            animator.SetBool("Wall", true);
            enElMuroL = true;
            Piso = false;
        }
        else if(HitR.collider != null){ //derecho
            Debug.Log("WALL RIGHT");
            rb2d.gravityScale = 0.1f;
            animator.SetBool("Wall", true);
            enElMuroR = true;
            Piso = false;
        }

         // Personaje en el aire
        if((HitL.collider == null) && (HitR.collider == null) && !Piso){
            Debug.Log("AIRE");
            animator.SetBool("Wall", false);
            enElMuroL = false;
            enElMuroR = false;
            rb2d.gravityScale = 1f;
        }

      

        
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.transform.tag == "Ground"){
            Piso = true;
            Debug.Log("GROUND COLLISION");
            animator.SetBool("Jump", false);
            animator.SetBool("Falling", false);
            
        }
        else if(collision.transform.tag == "Obstaculo"){
            Piso = true;
            Debug.Log("OBSTACLE COLLISION");
            animator.SetBool("Jump", false);
            animator.SetBool("Falling", false);
        }
    }

     private void OnTriggerEnter2D(Collider2D collision){
        /*if(collider.tag == "Trap"){
           Debug.Log("Trampa");
        }
        else if(collider.tag == "FallDetector"){

        }*/
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
