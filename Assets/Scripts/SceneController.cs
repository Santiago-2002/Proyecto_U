using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void changeScene(int _escena){
      Debug.Log("CALL -> changeScene: " + _escena);
      SceneManager.LoadScene(_escena);
   }

   public void quitGame(){
      #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
      #endif
      Application.Quit();
   }
}
