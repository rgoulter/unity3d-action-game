#pragma strict

var mechSpawnPoint : Transform;
var tankSpawnPoint : Transform;
var jetSpawnPoint : Transform;

var team : int;

// TODO: Until I find a better way to do this...
var tankRedMMIcon : UnityEngine.Texture2D;
var tankBlueMMIcon : UnityEngine.Texture2D;

function SpawnBotTank () {
    Debug.Log("Spawn Tank");

    var pt : Vector3 = tankSpawnPoint.position;
    var r : Quaternion = tankSpawnPoint.rotation;

    var newTank : GameObject;
    newTank = Instantiate(Resources.Load("BotTank2"), pt, r) as GameObject;
    // newTank.GetComponent(TeamPlayer).team = team;
    // TODO: Update team texture...

    // Add a minimap icon for the tank.
    var mmIconTex : Texture2D = (team == 0) ? tankBlueMMIcon : tankRedMMIcon;
    var mmCam : MinimapCamera = Camera.FindObjectOfType(MinimapCamera);
    mmCam.AddMinimapIconTo(newTank.transform, mmIconTex);
}
