using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinController : MonoBehaviour
{

    [SerializeField] private AudioSource Coin_SFX;

    private void OnTriggerEnter2D(Collider2D collision){ 
        Debug.Log("Moneda");
        Coin_SFX.Play(); //Sonido para la moneda antes de cambiar de escena 

        StartCoroutine(goNextLevel(Coin_SFX.clip.length));
        gameObject.GetComponent<Renderer>().enabled = false;
        

    }   
     
        private IEnumerator goNextLevel(float delay){
        yield return new WaitForSeconds(delay); 
        Destroy(gameObject);

        if(SceneManager.GetActiveScene().name=="Escena1"){
            SceneManager.LoadScene("Escena2");
        }
        else{
            SceneManager.LoadScene("Escena1");
        } 
    }

}