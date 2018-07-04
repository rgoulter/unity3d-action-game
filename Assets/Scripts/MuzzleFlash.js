#pragma strict

var muzFlashTex1 : Texture2D;
var muzFlashTex2 : Texture2D;

private var current : int = 0;

function Update () {
    if (current == 0) {
        this.GetComponent.<Renderer>().material.mainTexture = muzFlashTex1;
        current = 1;
    } else {
        this.GetComponent.<Renderer>().material.mainTexture = muzFlashTex2;
        current = 2;
    }

    if (current >= 2) {
        this.GetComponent.<Renderer>().enabled = false;
    }
}

public function Flash () {
    current = 0;
    this.GetComponent.<Renderer>().enabled = true;

    // Rotate randomly, just because
    transform.localRotation = Quaternion.Euler(Random.Range(-35, 35), 90, 0);
}
