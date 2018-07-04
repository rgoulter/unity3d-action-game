#pragma strict

var respawner : PlayerRespawner;

function Start () {
    respawner = GetComponent(CameraFollowPlayer).followObj.GetComponent(PlayerRespawner);

    if (respawner == null) {
        Debug.Log("Respawner is null in the object the camera is following!!");
    }
}

function OnEnable () {
    Debug.Log("Death cam enabled.");
    GetComponent(CameraFollowPlayer).enabled = false;
}

function Update () {
    if (respawner.respawnTime < 0) {
        enabled = false;
        GetComponent(CameraFollowPlayer).enabled = true;
    }
}
