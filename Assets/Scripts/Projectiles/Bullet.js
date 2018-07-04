#pragma strict

var lifetime : float = 3.0;

var shootSpeed : float = 20;
var damageAmount : float = 5;

var rocketTex : Texture2D;

function Start () {
    GetComponent.<Rigidbody>().velocity = transform.forward * shootSpeed;

    var mmIcon : GameObject = MinimapCamera.AddMinimapIconTo(transform, rocketTex);
    mmIcon.transform.localRotation.y = 180;
}

function Update () {
    if(lifetime > 0){
        lifetime -= Time.deltaTime;
    } else {
        GameObject.Destroy(gameObject);
        return;
    }
}

function OnCollisionEnter (collision : Collision) {
    Debug.Log("Bullet Collided with:" + collision.collider.name + ", gObj:" + collision.gameObject.name);

    // Generally, ignore triggers.
    if (collision.collider.isTrigger) {
        return;
    }

    // Damage any "Team Player" thing which we hit. (e.g. mech, tower, tank...).
    if (collision.gameObject.GetComponent(TeamPlayer) != null) {
        var objectHit : TeamPlayer = collision.gameObject.GetComponent(TeamPlayer);

        // Apply the damage.
        objectHit.health -= damageAmount;
    } else {
        Debug.Log("Hit non TP object, " + collision.gameObject);
    }

    // Since we hit something, destroy the bullet.
    Destroy(gameObject);
}
