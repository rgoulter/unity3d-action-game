#pragma strict

var followObj : UnityEngine.GameObject;
var cameraDistance : float = 3;

private var currentRotation : float = 0;

function Start () {
    /*
      Note on angles:
      Heck, don't care, but this seems to work okay as it is.
      I need to sit down and think to clean it up to something nicer.
      */
    currentRotation = (90 + followObj.transform.rotation.eulerAngles.y);
}

function Update () {
    // The idea here is that the camera is always a certain radius
    // from the game object, but we want to have a smooth transition
    // for when the player turns.. so we 'orbit' behind the player
    // to achieve a special effect.

    if (followObj == null) { return; }

    var tr = followObj.transform;
    var followPt = followObj.transform.position;

    // Get the direction followObj is facing. (i.e. its y axis)
    var goalRotation = (90 + followObj.transform.rotation.eulerAngles.y);
    currentRotation = Mathf.MoveTowardsAngle(currentRotation, goalRotation, (0.86 * 3.0));

    // Now, update our rotation to be *closer* to the rotation of the player.
    // -- Player rotate speed is about 3.0 (degrees/tick).

    // Calculate the position this camera needs to be at
    var cx = cameraDistance * Mathf.Cos(currentRotation * Mathf.Deg2Rad);
    var cz = cameraDistance * -Mathf.Sin(currentRotation * Mathf.Deg2Rad);

    var cy = cameraDistance * Mathf.Tan(3*Mathf.PI / 12);

    transform.position = followObj.transform.position + Vector3(cx, cy, cz);

    // Now we focus on a certain point. Make this 'above' the player, to be fancy.
    var lookatPt = followObj.transform.position;
    lookatPt.y = 3; // MAGIC
    transform.LookAt(lookatPt);
}

static function GetCameraForPlayer (p : PlayerTankForceMovement) {
    var cams : CameraFollowPlayer[] = UnityEngine.Object.FindObjectsOfType(CameraFollowPlayer) as CameraFollowPlayer[];

    for(var i = 0; i < cams.length; i++){
        // This more/less assumes that followObj has the playerTankForceMovement script...
        if(cams[i].followObj.GetComponent(PlayerTankForceMovement) != null &&
            cams[i].followObj.GetComponent(PlayerTankForceMovement).Equals(p)){
            return cams[i];
        }
    }

    Debug.Log("Couldn't find cam for " + p);

    return null;
}
