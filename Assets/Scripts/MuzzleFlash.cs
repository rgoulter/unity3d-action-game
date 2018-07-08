using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour {
    public Texture2D muzFlashTex1;
    public Texture2D muzFlashTex2;

    private int current = 0;

    void Update () {
        if (current == 0) {
            this.GetComponent<Renderer>().material.mainTexture = muzFlashTex1;
            current = 1;
        } else {
            this.GetComponent<Renderer>().material.mainTexture = muzFlashTex2;
            current = 2;
        }

        if (current >= 2) {
            this.GetComponent<Renderer>().enabled = false;
        }
    }

    public void Flash () {
        current = 0;
        this.GetComponent<Renderer>().enabled = true;

        // Rotate randomly, just because
        transform.localRotation = Quaternion.Euler(Random.Range(-35, 35), 90, 0);
    }
}
