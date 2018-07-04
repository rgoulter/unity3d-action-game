#pragma strict

// An alternative to MG Bullet, but instead,
// to do damage, this will just hitscan to the
// target.

var maxRange : float = 60; // Roughly what the mgBullet has by default.
var damageAmount : float = 5;

function Start () {
    // Raycast for our target

    // Bit shift the index of the layer (2) to get a bit mask
    var layerMask = 1 << 2;
    layerMask = ~layerMask;

    var hitInfo : RaycastHit;

    if (Physics.Raycast(transform.position, transform.forward, hitInfo, maxRange, layerMask)) {
        var hitObject = hitInfo.collider.gameObject;

        if (hitObject.GetComponent(TeamPlayer)) {
            hitObject.GetComponent(TeamPlayer).health -= damageAmount;
        } else {
            Debug.Log("Hit non TP object, " + hitObject);
        }

        // TODO: Maybe create a 'sparkle' sfx at the hitpoint??
        var pt : Vector3 = hitInfo.point;
    }

    Destroy(gameObject);
}
