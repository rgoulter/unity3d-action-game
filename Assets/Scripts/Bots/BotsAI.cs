using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TeamPlayer))]
[RequireComponent(typeof(AIFollow))]
public class BotsAI : MonoBehaviour {
    // for Pathfinding
    AIFollow aiFollow;
    float recuperateSpeed = Mathf.PI / 10;

    // for shooting
    float shootDelay = 3.0f;
    float shootTimer = 0;
    GameObject projectileObj;
    Transform shooterTransform;
    int currentSpawnChild = 0;
    string tankState = "Spawn_Bot";

    // for aiming
    GameObject currentTarget;
    GameObject turret;

    float rotateSpeedPerSecond = Mathf.PI / 2;
    float findEnemyRange = 100;
    float engageRange = 20;

    void Start () {
        // Set default values here
        currentTarget = null;
        GetComponent<TeamPlayer>().pointsValue = 1; // PlayerScores.killBotTankBonus;
        aiFollow = GetComponent<AIFollow>();
    }

    void Update () {
        // check health <= 0  change state to tank destroy

        // set overall states here
        if (GetComponent<TeamPlayer>().health <= 20) {
            if (GetComponent<TeamPlayer>().health <= 0) {
                tankState = "Destroy";
                //GameObject explosion =
                    //Instantiate(Resources.Load("Explosion"), transform.position, transform.rotation) as GameObject; // TIDYME: 2018-07-07: no Explosion
            } else {
                tankState = "Fleeto_Home_Base";
                aiFollow.Resume();
            }
        }


        switch (tankState) {
            case "Spawn_Bot":
                // play spawn animation
                // update all player details
                GetComponent<TeamPlayer>().health = 100;
                tankState = "Goto_Enemy_Base";
                break;

            case "Goto_Enemy_Base":
                // set the current target to to an enemy target within range
                currentTarget = FindClosestTarget();


                if (currentTarget == null) {
                    // if no available targets within range move towards enemy base
                    currentTarget = GameObject.Find("EnemyBase"); // TIDYME: 2018-07-07: Use of Find
                } else {
                    // if target found change state to engage target
                    tankState = "Engage_targets";
                }

                // set that path finding target to whichever target that has been set
                aiFollow.target = currentTarget.transform;
                break;

            case "Engage_targets":
                // change target if another target is closer to you - as closer targets are more dangeerous
                currentTarget = FindClosestTarget();
                if (currentTarget != null) {
                    aiFollow.target = currentTarget.transform;
                }

                if (currentTarget != null) {
                    float distFromTarget = Vector3.Distance(turret.transform.position, currentTarget.transform.position);

                    if (distFromTarget <= engageRange) {
                        aiFollow.Stop();
                        RotateTurret();
                        if (CheckFiringStatus()){
                            BattleEnemy();
                        } else if (TurretAimStatus()) {
                            aiFollow.Resume();
                        }
                    } else if (distFromTarget <= findEnemyRange &&  distFromTarget > engageRange) {
                        // go near enemy and engage him since he is far

                        // dont engage but move to enemy base
                        aiFollow.Resume();
                        // default action
                        // can add animation or sound here
                    } else {
                        // since no enemy within findEnemyRange go towards enemy base
                        aiFollow.Resume();
                        tankState = "GotoEnemyBase";
                    }
                } else {
                    aiFollow.Resume();
                    tankState = "GotoEnemyBase";
                }

                break;

            case "Fleeto_Home_Base":
                // move to home base
                currentTarget = GameObject.Find("HomeBase");
                aiFollow.target = currentTarget.transform;
                if (GetComponent<TeamPlayer>().health >= 50) {
                    tankState = "Goto_Enemy_Base";
                    break;
                } else {
                    currentTarget = FindClosestTarget();
                    if (CheckPickups()) {
                        // change state to getPickups
                        tankState = "Get_PickUps";
                    }
                }

                if (GetComponent<TeamPlayer>().health >= 50) {
                    tankState = "Goto_Enemy_Base";
                }

                break;

            case "Get_PickUps":
                if (CheckPickups() != null) {
                    // get and move to pickup

                    // if managed to get pickup play Pickup animation here

                    // update pickup points here
                    GetComponent<TeamPlayer>().health += 30;
                } else {
                    tankState = "Fleeto_Home_Base";
                }

                if (GetComponent<TeamPlayer>().health >= 50) {
                    tankState = "Goto_Enemy_Base";
                }

                break;

            case "Recuperate":
                // play recuperate animation

                // increase health with time
                GetComponent<TeamPlayer>().health += Time.deltaTime * recuperateSpeed;

                // stay till health is beyond 50 percent
                if (GetComponent<TeamPlayer>().health >= 50) {
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
    GameObject CheckPickups () {
        // if current target is not in range or if no current target and pickup is withing range set target to pickup
        return null;
    }

    bool TurretAimStatus () {
        // aiming
        //Vector3 currentDir = turret.transform.forward;
        //Vector3 goalDir;

        //if (currentTarget != null) {
        //    // rotate towards target.
        //    float angleThresholdToCheck = 10;

        //    // FIXME: 2018-07-07 goalDir is never assigned
        //    //float angle = Vector3.Angle(goalDir, turret.transform.forward);

        //    //if (angle >= angleThresholdToCheck) {
        //    //    return true;
        //    //}
        //}

        return false;
    }

    void RotateTurret () {
        // aiming
        Vector3 currentDir = turret.transform.forward;
        Vector3 goalDir;

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

    void BattleEnemy () {
        // shooting
        if (shootTimer < shootDelay) {
            shootTimer += Time.deltaTime;
        }

        if (shootTimer >= shootDelay && currentTarget != null) {
            // Point to spawn the bullet at.
            // How we do this: We assume the shooter object has certain points
            // which are its children; we then pick the first child and shoot from that.
            Transform spawnTransform = shooterTransform.GetChild(currentSpawnChild);
            currentSpawnChild = (currentSpawnChild + 1) % shooterTransform.childCount; // move to next child
            Vector3 spawnPt = spawnTransform.position;

            // Rotation to spawn the bullet towards.
            // If the targetter has a target, aim at that.
            // Otherwise, just aim 'forward'.
            Quaternion quaternion;

            Vector3 targetPt = currentTarget.GetComponent<TeamPlayer>().GetAimAtPoint();

            Quaternion oldRotation = spawnTransform.rotation;
            spawnTransform.LookAt(targetPt);
            quaternion = spawnTransform.rotation;
            spawnTransform.rotation = oldRotation;

            Instantiate(projectileObj, spawnPt, quaternion);
            shootTimer -= shootDelay;
            GetComponent<Animation>().Play();
        }
    }


    bool CheckFiringStatus () {
        if (currentTarget != null) {
            Transform spawnTransform = shooterTransform.GetChild(currentSpawnChild);
            currentSpawnChild = (currentSpawnChild + 1) % shooterTransform.childCount; // move to next child

            Vector3 spawnPt = spawnTransform.position;
            RaycastHit hitinfo;

            // FIXME: 2018-07-07 this ... isn't the correct invocation
            if (Physics.Raycast(spawnPt, turret.transform.forward, out hitinfo, engageRange)) {
                if ((hitinfo.collider.gameObject).name == currentTarget.name) {
                    return true;
                }
            }
        }

        return false;
    }

    // TIDYME: 2018-07-07: Code-duplication from TowerLive
    GameObject FindClosestTargetWithinRange (float engageRange) {
        GameObject t = FindClosestTarget();

        if (t != null && Vector3.Distance(t.transform.position,this.transform.position) < engageRange){
            return t;
        } else {
            return null;
        }
    }

    GameObject FindClosestTarget () {
        TeamPlayer[] potentialTargets = UnityEngine.Object.FindObjectsOfType(typeof(TeamPlayer)) as TeamPlayer[];

        GameObject bestTarget = null;
        float bestDist = 99999;

        foreach (TeamPlayer t in potentialTargets) {
            float d = Vector3.Distance(this.transform.position, t.gameObject.transform.position);

            if (t.gameObject.Equals(this)) {
                continue;
            }

            if (d < bestDist && !t.SameTeam(this.GetComponent<TeamPlayer>())) {
                bestDist = d;
                bestTarget = t.gameObject;
            }
        }

        return bestTarget;
    }
}
