#pragma strict

function Start () {
    GetComponent.<Renderer>().material.color = Color.gray;
}

function OnMouseEnter () {
    GetComponent.<Renderer>().material.color = Color.blue;
}

function OnMouseExit () {
    GetComponent.<Renderer>().material.color = Color.gray;
}

function OnMouseUp () {
    // load the game
    Application.LoadLevel(2);
}
