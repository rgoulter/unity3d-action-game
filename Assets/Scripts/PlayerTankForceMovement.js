#pragma strict

var speed : float = 6.0;
var rotateSpeed : float = Mathf.PI;

var forwardAxis = "Vertical";
var turnAxis = "Horizontal";
var strafeAxis = "Strafe";

function Start () {
    GetComponent(TeamPlayer).pointsValue = PlayerScores.killMechBonus;
}

function Update () {
    // Rotate around y - axis
    // TODO: rotate speed needs to shift to degrees/second, not per frame.
    transform.Rotate(0, Input.GetAxis(turnAxis) * rotateSpeed * Time.deltaTime * Mathf.Rad2Deg, 0);

    // Move forward / backward, and/or strafe
    var forwardVec : Vector3 = Input.GetAxis(forwardAxis) * Vector3.forward;
    var strafeVec : Vector3 = Input.GetAxis(strafeAxis) * Vector3.right;

    var moveVec : Vector3 = forwardVec + strafeVec;
    moveVec.Normalize();


    transform.Translate(moveVec * speed * Time.deltaTime);
}


function OnCollisionEnter (collision : Collision) {
    //Debug.Log("Player got collision from:" + collision.collider.name);

    // What is this doing here?? The code for this kindof thing should
    // be in mgBullet only..
    // Alas, mgBullet collides with a trigger. So, we kinda have to have this code.
    if (collision.gameObject.GetComponent(Bullet) != null) {
        // This seems to work now?
        Debug.Log("Mech take damage from mgBullet");
        GetComponent(TeamPlayer).health -= collision.gameObject.GetComponent(Bullet).damageAmount;

        Destroy(collision.gameObject);
    }
}

function OnDestroy () {
    // We got blown up or something.

    // Add/Remove points as necessary.

    // Now we need to "respawn".
    // Respawn policy: No delay (atm) TODO
    // Respawn policy: At an arbitrary(?) team-base's mech spawn.
    // Respawn policy: Weapons and ammo are kept...(?)
    // Respawn policy: Health will be 100

    var baseToRespawnAt : TeamBase;
    var bases : TeamBase[] = FindObjectsOfType(TeamBase) as TeamBase[];
    for (var base : TeamBase in bases) {
        // Some base of the same team.
        if(base.team == gameObject.GetComponent(TeamPlayer).team){
            baseToRespawnAt = base;
            break;
        }
    }

    /*

    var newPlayer : GameObject;
    // Can't just clone ourselves, since gameObj is null by now.
    newPlayer = Instantiate(Resources.Load("PlayerMech"),
                            baseToRespawnAt.mechSpawnPoint.position,
                            baseToRespawnAt.mechSpawnPoint.rotation);

// Set the health to 100
//newPlayer.GetComponent(TeamPlayer).health = 100;

// ... and, re-associate the camera with the new player.
    var cameras : Camera[] = Camera.allCameras;
    for (var c : Camera in cameras) {
        if (c.GetComponent(CameraFollowPlayer) != null &&
           c.GetComponent(CameraFollowPlayer).followObj == null) {

            c.GetComponent(CameraFollowPlayer).followObj = newPlayer;
        }
    }
    */
}
