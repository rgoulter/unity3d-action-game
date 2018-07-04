#pragma strict

@script RequireComponent(GameObject);
@script RequireComponent(TeamPlayer);

// for shooting
var shootDelay : float = 3.0f;
var shootTimer : float = 0;
var projectileObj : UnityEngine.GameObject;
var shooterTransform : UnityEngine.Transform;
var currentSpawnChild : int = 0;

// for aiming
var currentTarget : UnityEngine.GameObject;
var turret : UnityEngine.GameObject;

var rotateSpeedPerSecond = Mathf.PI / 2;
var maxRange = 8;

// Until I find a better way to do this...
var towerRedMMIcon : UnityEngine.Texture2D;
var towerBlueMMIcon : UnityEngine.Texture2D;

private var isQuitting = false; // Needed for cheap hack.

function Start () {
    currentTarget = null;

    GetComponent(TeamPlayer).pointsValue = PlayerScores.killTowerBonus;

    SnapObjectToGround(gameObject);
}

function OnApplicationQuit () {
    isQuitting = true;
}

public function UpdateTextureToTeamColor () {
    // For the team player thing
    var teamSelf : TeamPlayer = GetComponent(TeamPlayer);

    var meshRenderers : Component[];
    meshRenderers = gameObject.GetComponentsInChildren(MeshRenderer);

    if (meshRenderers == null) { Debug.Log("MRs is NULL!!!"); }

    // Team 0, blue.
    // Team 1, red.
    var i = 0;
    var mat = Resources.Load((teamSelf.team == 0) ? "MatBlue" : "MatRed");
    var mrenderer : MeshRenderer;
    for (i = 0; i < meshRenderers.length; i += 1) {
        mrenderer = meshRenderers[i] as MeshRenderer;
        mrenderer.material = mat as Material;
    }

    // Add a minimap icon for ourselves.
    // NOTE: This means we don't want to be able to convert a live tower, okay?
    AddMinimapIcon();
}

function AddMinimapIcon () {
    var mmIconTex : Texture2D = (GetComponent(TeamPlayer).team == 0) ? towerBlueMMIcon : towerRedMMIcon;
    var mmCam : MinimapCamera = Camera.FindObjectOfType(MinimapCamera);
    mmCam.AddMinimapIconTo(transform, mmIconTex);
}

function OnCollisionEnter (collision : Collision) {
}

function OnDestroy () {
    if (isQuitting) { return; }

    // The tower was destroyed; replace it with a towerDead object.
    var pt : Vector3 = transform.position;
    var r : Quaternion = transform.rotation;

    var deadTower : GameObject = Instantiate(Resources.Load("TowerDead"), pt, r) as GameObject;
    deadTower.transform.parent = transform.parent;
}

function Update () {
    // Aiming
    var currentDir : Vector3 = turret.transform.forward;
    var goalDir : Vector3;

    if (currentTarget != null) {
        // Rotate towards target.
        goalDir = currentTarget.GetComponent(TeamPlayer).GetAimAtPoint() - turret.transform.position;

        if (maxRange < Vector3.Distance(turret.transform.position, currentTarget.transform.position)) {
            currentTarget = null;
        }
    } else {
        // Look back towards a 'default' rotation
        goalDir = new Vector3(0, 0, 1);

        // Try and find another target.
        currentTarget = FindClosestTargetWithinRange(maxRange);
    }

    // Okay, to make the tower rotation smoother, we need to interpolate more explicitly.
    // (Vector3.RotateTowards was too unrestricted).

    var v1 = Vector3.RotateTowards(new Vector3(currentDir.x, 0, currentDir.z),
        new Vector3(goalDir.x, 0, goalDir.z),
        rotateSpeedPerSecond * Time.deltaTime,
        1);

    var v2 = Vector3.RotateTowards(new Vector3(0, currentDir.y, 0),
        new Vector3(0, goalDir.y, 0),
        rotateSpeedPerSecond * Time.deltaTime / 3,
        1);
    var res = v1 + v2;

    turret.transform.LookAt(turret.transform.position + res);


    // Shooting
    if (shootTimer < shootDelay) {
        shootTimer += Time.deltaTime;
    }

    if (shootTimer >= shootDelay &&
        currentTarget != null) {
        Debug.Log("Still need to rotate " + Quaternion.FromToRotation(currentDir, goalDir).eulerAngles.y +
            " " + Mathf.DeltaAngle(Quaternion.FromToRotation(currentDir, goalDir).eulerAngles.y, 0));
    }
    if (shootTimer >= shootDelay &&
        currentTarget != null &&
        Mathf.Abs(Mathf.DeltaAngle(Quaternion.FromToRotation(currentDir, goalDir).eulerAngles.y, 0)) < 10) { // MAGIC
        var bullet : UnityEngine.GameObject;


        // Point to spawn the bullet at.
        // How we do this: We assume the shooter object has certain points
        // which are its children; we then pick the first child and shoot from that.
        var spawnTransform : Transform = shooterTransform.GetChild(currentSpawnChild);
        currentSpawnChild = (currentSpawnChild + 1) % shooterTransform.childCount; // move to next child
        var spawnPt : UnityEngine.Vector3 = spawnTransform.position;


        // Rotation to spawn the bullet towards.
        // If the targetter has a target, aim at that.
        // Otherwise, just aim 'forward'.
        var quaternion : UnityEngine.Quaternion;

        var targetPt : Vector3 = currentTarget.GetComponent(TeamPlayer).GetAimAtPoint();

        var oldRotation = spawnTransform.rotation;
        spawnTransform.LookAt(targetPt);
        quaternion = spawnTransform.rotation;
        spawnTransform.rotation = oldRotation;


        bullet = Instantiate(projectileObj, spawnPt, quaternion);
        shootTimer -= shootDelay;
    }
}

function FindClosestTargetWithinRange (maxRange : float) {
    var t : UnityEngine.GameObject = FindClosestTarget();

    if (t != null && Vector3.Distance(t.transform.position, turret.transform.position) < maxRange){
        return t;
    } else {
        return null;
    }
}

function FindClosestTarget () {
    var potentialTargets : TeamPlayer[] = UnityEngine.Object.FindObjectsOfType(TeamPlayer) as TeamPlayer[];

    var bestTarget : GameObject = null;
    var bestDist : float = 99999;

    var i : int = 0;

    for (i = 0; i < potentialTargets.length; i += 1) {
        var t : TeamPlayer = potentialTargets[i];
        var d = Vector3.Distance(turret.transform.position, t.gameObject.transform.position);

        if (t.gameObject.Equals(this)){ continue; }

        if (d < bestDist && !t.SameTeam(this.GetComponent(TeamPlayer))) {
            bestDist = d;
            bestTarget = t.gameObject;
        }
    }

    return bestTarget;
}



static function SnapAllTowersToGround () {
    var towers : TowerLive[] = FindObjectsOfType(TowerLive) as TowerLive[];
    var i : int = 0;

    for (i = 0; i < towers.length; i++) {
        SnapObjectToGround(towers[i].gameObject);
    }
}



static function SnapObjectToGround (obj : GameObject) {
    // Raycast downward to find a collision with the ground.
    var hitInfo : RaycastHit;

    if (Physics.Raycast(obj.transform.position, new Vector3(0, -1, 0), hitInfo, 100)) {
        var hitObject = hitInfo.collider.gameObject;

        obj.transform.position.y = hitInfo.point.y;
    }
}
