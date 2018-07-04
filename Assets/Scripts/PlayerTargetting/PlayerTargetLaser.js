#pragma strict

var targetter : PlayerTargetter;

function Start () {
    // Targetting laser will belong to a 'sibling' object
    // of the targetting laser, for the mech.

    targetter = transform.parent.GetComponentInChildren(PlayerTargetter);

    if (targetter == null) {
        Debug.Log("Could not find a targetter for the reticule");
        Destroy(gameObject);
    }
}

function Update () {
    var target : UnityEngine.GameObject = targetter.currentTarget;

    var laserCylinder = transform.GetChild(0).gameObject;

    if (target != null) {
        laserCylinder.GetComponent.<Renderer>().enabled = true; // make ourselves visible

        // Now we want to 'point to' the target,
        var targetPt : UnityEngine.Vector3 = target.GetComponent(TeamPlayer).GetAimAtPoint();

        transform.LookAt(targetPt);

        var dist : float = Vector3.Distance(transform.position, targetPt);
        transform.localScale.z = dist / 2;
    } else {
        laserCylinder.GetComponent.<Renderer>().enabled = false;
    }
}
