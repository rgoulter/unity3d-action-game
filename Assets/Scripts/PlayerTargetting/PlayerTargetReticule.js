#pragma strict

// This is to show some kind of target/reticule on the
// thing which the player is currently targetting.

var targetter : PlayerTargetter;
var forCamera : UnityEngine.Camera;

function Start () {
    // The targetter must be set externally.
    if (targetter == null) {
        Debug.Log("Could not find a targetter for the reticule");
        Destroy(gameObject);
    }
}

function Update () {
    var target : UnityEngine.GameObject = targetter.currentTarget;

    var p : UnityEngine.Vector3;

    if (target != null) {
        // The following, commented-out code is appropriate for some kind of icon
        // is to float above a target's head. This could be useful. KIV.
        //// // Move to its position.
        //// p = target.transform.position;
        //// p.y = p.y + 1;
        //// gameObject.transform.position = p;

        //
        // Have our plane so that it's positioned facing the camera.
        //
        // The idea is: from the point of the target, move towards the camera point,
        //  then look at the camera.
        var targetPt : UnityEngine.Vector3 = target.GetComponent(TeamPlayer).GetAimAtPoint();
        var cameraPt : UnityEngine.Vector3 = forCamera.transform.position;

        p = Vector3.MoveTowards(targetPt, cameraPt, 2); // 2 is MAGIC

        transform.position = p;
        transform.LookAt(cameraPt);
    } else {
        p = new Vector3(0, -10, 0);

        gameObject.transform.position = p;
    }
}
