#pragma strict

var shootDelay : float = 3.0f;
var shootTimer : float = 0;
var spwnObj : UnityEngine.GameObject;
var shootAxis = "Fire1";
var targetter : PlayerTargetter;

var ammoCount : int = 1000;

var currentSpawnChild : int = 0;

function Start () {
    // Targetting laser will belong to a 'sibling' object
    // of the targetting laser, for the mech.

    targetter = transform.parent.GetComponentInChildren(PlayerTargetter);

    if (targetter == null) {
        Debug.Log("Could not find a targetter for the projectile shooter.");
        Destroy(gameObject);
    }
}

function Update () {
    if (shootTimer < shootDelay) {
        shootTimer += Time.deltaTime;
    }

    if (Input.GetAxis(shootAxis) > 0 && shootTimer >= shootDelay && ammoCount > 0) {
        var bullet : UnityEngine.GameObject;

        // Point to spawn the bullet at.
        // How we do this: We assume the shooter object has certain points
        // which are its children; we then pick the first child and shoot from that.
        var spawnTransform : Transform = transform.GetChild(currentSpawnChild);
        currentSpawnChild = (currentSpawnChild + 1) % transform.childCount; // move to next child
        var spawnPt : UnityEngine.Vector3 = spawnTransform.position;


        // Rotation to spawn the bullet towards.
        // If the targetter has a target, aim at that.
        // Otherwise, just aim 'forward'.
        var quaternion : UnityEngine.Quaternion;

        if (targetter.currentTarget != null) {
            var targetPt : Vector3 = targetter.currentTarget.transform.position;
            targetPt.y += 1;

            var oldRotation = spawnTransform.rotation;
            spawnTransform.LookAt(targetPt);
            quaternion = spawnTransform.rotation;
            spawnTransform.rotation = oldRotation;
        } else {
            quaternion = spawnTransform.rotation;
        }

        bullet = Instantiate(spwnObj, spawnPt, quaternion);
        shootTimer -= shootDelay;
        ammoCount -= 1;

        // Try for muzzle flash
        var mflash : MuzzleFlash = spawnTransform.gameObject.GetComponentInChildren(MuzzleFlash);
        if (mflash != null) {
            mflash.Flash();
        }
    }
}
