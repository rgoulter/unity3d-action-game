#pragma strict

var followPt : UnityEngine.Transform;
var height : int = 15;

function Start () {
    // We need to ensure a minimap icon is created for the objects which
    // are in the scene already.
    // For objects which are created afterwards, they will worry about this themselves.
    var towers : TowerLive[] = UnityEngine.Object.FindObjectsOfType(TowerLive) as TowerLive[];
    for (var i = 0; i < towers.length; i++) {
        towers[i].AddMinimapIcon();
    }
}

static function AddMinimapIconTo (trans : Transform, texture : Texture2D) {
    var position : Vector3 = trans.position;
    position.y = 5; // MAGIC

    var rot : UnityEngine.Quaternion = UnityEngine.Quaternion.identity;

    var mmIcon : GameObject = Instantiate(Resources.Load("MinimapIcon"),
        position,
        rot) as GameObject;
    mmIcon.transform.parent = trans; // Attach it to its parent

    // Now update the material/texture..
    mmIcon.GetComponent.<Renderer>().material.mainTexture = texture;

    return mmIcon;
}

function Update () {
    // Set the x & z components of the minimap camera to be above followPt
    var cam = GetComponent(Camera);
    cam.transform.position.x = followPt.position.x;
    cam.transform.position.z = followPt.position.z;
    cam.transform.position.y = height;
}

function OnGUI () {
    GUI.depth = 12;
    GUI.Box(Rect(GetComponent.<Camera>().pixelRect.x - 1, (Screen.height - GetComponent.<Camera>().pixelRect.yMax - 1), GetComponent.<Camera>().pixelWidth + 2, GetComponent.<Camera>().pixelHeight + 2), "");
}
