#pragma strict

var blueTex : UnityEngine.Texture;
var redTex: UnityEngine.Texture;

var towerGreyMMIcon : UnityEngine.Texture2D;

private var replacedByTeam : int = -1;

function Start () {
    AddMinimapIcon();
}

public function ReplaceWithLiveTower (team : int) {
    if (replacedByTeam >= 0) {
        return;
    }
    replacedByTeam = team;

    var pt : Vector3 = gameObject.transform.position;
    var r : Quaternion = gameObject.transform.rotation;

    Destroy(gameObject);

    var newTower : UnityEngine.GameObject;
    newTower = Instantiate(Resources.Load("TowerLive"), pt, r) as GameObject;
    newTower.transform.parent = transform.parent;

    // For now, we will only replace with blue team.
    Debug.Log("Replacing tower, new team = " + team);

    newTower.GetComponent(TeamPlayer).team = team;
    newTower.GetComponent(TowerLive).UpdateTextureToTeamColor();
}

function AddMinimapIcon () {
    var mmIconTex : Texture2D = towerGreyMMIcon;
    var mmCam : MinimapCamera = Camera.FindObjectOfType(MinimapCamera);
    mmCam.AddMinimapIconTo(transform, mmIconTex);
}
