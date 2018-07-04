#pragma strict

var speed : float = 500;
var drx : float = 0;
var dry : float = 0;
var drz : float = 0;
var lifetime : float = 0.3;

function Update () {
    lifetime -= Time.deltaTime;

    transform.Rotate(drx * Time.deltaTime, dry * Time.deltaTime, drx * Time.deltaTime);
    transform.Translate(transform.forward * speed * Time.deltaTime);

    if(lifetime < 0){
        Destroy(gameObject);
    }
}
