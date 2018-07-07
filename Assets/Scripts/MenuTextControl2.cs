using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuTextControl2 : MonoBehaviour {
    void Start () {
        GetComponent<Renderer>().material.color = Color.gray;
    }

    void OnMouseEnter () {
        GetComponent<Renderer>().material.color = Color.blue;
    }

    void OnMouseExit () {
        GetComponent<Renderer>().material.color = Color.gray;
    }

    void OnMouseUp () {
        // load the game
        SceneManager.LoadScene("FutureCop");
    }
}
