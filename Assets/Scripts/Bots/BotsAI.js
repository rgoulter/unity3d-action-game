#pragma strict

@script RequireComponent(GameObject);
@script RequireComponent(TeamPlayer);
@script RequireComponent(AIFollow);

// for Pathfinding
var csharpcomponent : AIFollow;
var recuperateSpeed = Mathf.PI / 10;

// for shooting
var shootDelay : float = 3.0f;
var shootTimer : float = 0;
var projectileObj : UnityEngine.GameObject;
var shooterTransform : UnityEngine.Transform;
var currentSpawnChild : int = 0;
var tankState : String = "Spawn_Bot";

// for aiming
var currentTarget : UnityEngine.GameObject;
var turret : UnityEngine.GameObject;

var rotateSpeedPerSecond = Mathf.PI / 2;
var findEnemyRange = 100;
var engageRange = 20;

function Start () {
    // Set default values here
    currentTarget = null;
    GetComponent(TeamPlayer).pointsValue = PlayerScores.killBotTankBonus;
    csharpcomponent = this.GetComponent(AIFollow);
}

function Update () {
    // check health <= 0  change state to tank destroy

    // set overall states here
    if (GetComponent(TeamPlayer).health <= 20) {
        if(GetComponent(TeamPlayer).health <= 0) {
            tankState = "Destroy";
            var explosion : GameObject =
                Instantiate(Resources.Load("Explosion"), transform.position, transform.rotation) as GameObject;
        } else {
            tankState = "Fleeto_Home_Base";
            csharpcomponent.Resume();
        }
    }


    switch(tankState){
        case "Spawn_Bot":
            // play spawn animation
            // update all player details
            GetComponent(TeamPlayer).health = 100;
            tankState = "Goto_Enemy_Base";
            break;

        case "Goto_Enemy_Base":
            // set the current target to to an enemy target within range
            currentTarget = FindClosestTarget();


            if (currentTarget == null) {
                // if no available targets within range move towards enemy base
                currentTarget = gameObject.Find("EnemyBase");
            } else {
                // if target found change state to engage target
                tankState = "Engage_targets";
            }

            // set that path finding target to whichever target that has been set
            csharpcomponent.target = currentTarget.transform;
            break;

        case "Engage_targets":
            // change target if another target is closer to you - as closer targets are more dangeerous
            currentTarget = FindClosestTarget();
            if (currentTarget != null) {
                csharpcomponent.target = currentTarget.transform;
            }

            if (currentTarget != null) {
                var distFromTarget = Vector3.Distance(turret.transform.position, currentTarget.transform.position);

                if (distFromTarget <= engageRange) {
                    csharpcomponent.Stop();
                    RotateTurret();
                    if (CheckFiringStatus()){
                        BattleEnemy();
                    } else if (TurretAimStatus()) {
                        csharpcomponent.Resume();
                    }
                } else if (distFromTarget <= findEnemyRange &&  distFromTarget > engageRange) {
                    // go near enemy and engage him since he is far

                    // dont engage but move to enemy base
                    csharpcomponent.Resume();
                    // default action
                    // can add animation or sound here
                } else {
                    // since no enemy within findEnemyRange go towards enemy base
                    csharpcomponent.Resume();
                    tankState = "GotoEnemyBase";
                }
            } else {
                csharpcomponent.Resume();
                tankState = "GotoEnemyBase";
            }

            break;

        case "Fleeto_Home_Base":
            // move to home base
            currentTarget = gameObject.Find("HomeBase");
            csharpcomponent.target = currentTarget.transform;
            if (GetComponent(TeamPlayer).health >= 50) {
                tankState = "Goto_Enemy_Base";
                break;
            } else {
                currentTarget = FindClosestTarget();
                if (CheckPickups()) {
                    // change state to getPickups
                    tankState = "Get_PickUps";
                }
            }

            if (GetComponent(TeamPlayer).health >= 50) {
                tankState = "Goto_Enemy_Base";
            }

            break;

        case "Get_PickUps":
            if (CheckPickups() != null) {
                // get and move to pickup

                // if managed to get pickup play Pickup animation here

                // update pickup points here
                GetComponent(TeamPlayer).health += 30;
            } else {
                tankState = "Fleeto_Home_Base";
            }

            if (GetComponent(TeamPlayer).health >= 50) {
                tankState = "Goto_Enemy_Base";
            }

            break;

        case "Recuperate":
            // play recuperate animation

            // increase health with time
            GetComponent(TeamPlayer).health += Time.deltaTime * recuperateSpeed;

            // stay till health is beyond 50 percent
            if (GetComponent(TeamPlayer).health >= 50) {
                tankState = "Goto_Enemy_Base";
            }

            break;

        case "Destroy":
            // kill bot here
            // Play explosion animation here
            break;
    }
}

// 20180731: STYLE: DEAD CODE
function CheckPickups () {
    // if current target is not in range or if no current target and pickup is withing range set target to pickup
    return null;
}

function TurretAimStatus () {
    // aiming
    var currentDir : Vector3 = turret.transform.forward;
    var goalDir : Vector3;

    if (currentTarget != null) {
        // rotate towards target.
        var angleThresholdToCheck : float = 10;

        var angle : float = Vector3.Angle(goalDir, turret.transform.forward);

        if (angle >= angleThresholdToCheck) {
            return true;
        }
    }

    return false;
}

function RotateTurret () {
    // aiming
    var currentDir : Vector3 = turret.transform.forward;
    var goalDir : Vector3;

    if (currentTarget != null) {
        // rotate towards target.
        goalDir = currentTarget.transform.position - turret.transform.position;

        if (engageRange < Vector3.Distance(turret.transform.position, currentTarget.transform.position)) {
            currentTarget = null;
        }
    } else {
        // look back towards a 'default' rotation
        goalDir = new Vector3(0, 0, 1);

        // try and find another target.
        currentTarget = FindClosestTargetWithinRange(engageRange);
    }

    turret.transform.LookAt(turret.transform.position +
        Vector3.RotateTowards(currentDir,
            goalDir,
            rotateSpeedPerSecond * Time.deltaTime,
            1));
}

function BattleEnemy () {
    // shooting
    if (shootTimer < shootDelay) {
        shootTimer += Time.deltaTime;
    }

    if (shootTimer >= shootDelay && currentTarget != null) {
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
        GetComponent.<Animation>().Play();
    }
}


function CheckFiringStatus () {
    if (currentTarget != null) {
        var spawnTransform : Transform = shooterTransform.GetChild(currentSpawnChild);
        currentSpawnChild = (currentSpawnChild + 1) % shooterTransform.childCount; // move to next child

        var spawnPt : UnityEngine.Vector3 = spawnTransform.position;
        var hitinfo: RaycastHit;

        if (Physics.Raycast(spawnPt, turret.transform.forward, hitinfo, engageRange)) {
            if ((hitinfo.collider.gameObject).name == currentTarget.name) {
                return true;
            }
        }
    }

    return false;
}

function FindClosestTargetWithinRange (engageRange : float) {
    var t : UnityEngine.GameObject = FindClosestTarget();

    if (t != null && Vector3.Distance(t.transform.position,this.transform.position) < engageRange){
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
        var d = Vector3.Distance(this.transform.position, t.gameObject.transform.position);

        if (t.gameObject.Equals(this)) {
            continue;
        }

        if (d < bestDist && !t.SameTeam(this.GetComponent(TeamPlayer))) {
            bestDist = d;
            bestTarget = t.gameObject;
        }
    }

    return bestTarget;
}
