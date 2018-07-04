#pragma strict

// 0 == blue,
// 1 == red
public var team : int = 0;
var relAimAtPoint : Vector3 = new Vector3(0, 1, 0);

var health : float = 100;
var pointsValue : int;

function Update () {
    if (health < 0) {
        // Add the # points to the *other* team
        PlayerScores.AddTeamScore(1 - team, pointsValue);

        if (GetComponent(PlayerTankForceMovement) != null) {
            // Treat the player specially... since things can get a bit awkward otherwise

            // Any respawn / reassignment logic would need to happen here;
            GetComponent(PlayerRespawner).enabled = true;

            health = 100; // So that the player doesn't keep adding score.

            var cam = CameraFollowPlayer.GetCameraForPlayer(GetComponent(PlayerTankForceMovement));
            if (cam == null) { Debug.Log("Couldn't find camera for the player... awkward."); }
            cam.GetComponent(PlayerDeathCamera).enabled = true;
        } else {
            // // added explosion effect
            // if (this.tag == "Tank") {
            //     var explosion : GameObject = Instantiate(Resources.Load("Explosion"), transform.position, transform.rotation);
            // }
            var explosion : GameObject = Instantiate(Resources.Load("Explosion"), transform.position, transform.rotation) as GameObject;
            Destroy(gameObject);
        }
    }
}

public function SameTeam (other : TeamPlayer) {
    return other.team == team;
}

function GetAimAtPoint (){
    return transform.position + relAimAtPoint;
}
