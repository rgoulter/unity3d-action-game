#pragma strict

// Use this for managing how a player aims at things.
// -- apply to a 'trigger'.

var currentTarget : GameObject;

var maxRange : float = 8;

function Start () {
    currentTarget = null;
}

function Update () {
    // parent here must be playermech. Should be fine as long as this targetter used only for players
    if(currentTarget != null &&
        maxRange < Vector3.Distance(transform.parent.position, currentTarget.transform.position)){
        currentTarget = null;
    }
}

function OnTriggerEnter (other : Collider) {
    var otherObj = other.gameObject;

    if(otherObj.GetComponent(TeamPlayer) != null){
        //Debug.Log("entered team target: " + other.gameObject.name);

        if(currentTarget == null){
            currentTarget = otherObj;
        }
    }
}

function OnTriggerExit (other : Collider) {
    var otherObj = other.gameObject;

    if(otherObj.Equals(currentTarget)){
        //Debug.Log("Lost focus on target");

        currentTarget = null;
    }
}
