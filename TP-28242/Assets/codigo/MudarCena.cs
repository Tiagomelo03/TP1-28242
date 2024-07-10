using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MudarCena : MonoBehaviour
{
    public void PlayGame(){

        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame(){

        Application.Quit();
    }

    public void VoltarMenu(){
        SceneManager.LoadSceneAsync(0);
    }

    public void Garagem(){
        SceneManager.LoadSceneAsync(2);
    }
}
