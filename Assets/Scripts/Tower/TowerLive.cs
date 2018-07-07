using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TeamPlayer))]
public class TowerLive : MonoBehaviour {
    // for shooting
    float shootDelay = 3.0f;
    float shootTimer = 0.0f;
    GameObject projectileObj;
    Transform shooterTransform;
    int currentSpawnChild = 0;

    // for aiming
    GameObject currentTarget;
    GameObject turret;

    float rotateSpeedPerSecond = Mathf.PI / 2;
    float maxRange = 8.0f;

    // Until I find a better way to do this
    Texture2D towerRedMMIcon;
    Texture2D towerBlueMMIcon;

    private bool isQuitting = false; // Needed for cheap hack.

    void Start () {
        currentTarget = null;

        GetComponent<TeamPlayer>().pointsValue = PlayerScores.killTowerBonus;

        SnapObjectToGround(gameObject);
    }

    void OnApplicationQuit () {
        isQuitting = true;
    }

    public void UpdateTextureToTeamColor () {
        // For the team player thing
        TeamPlayer teamSelf = GetComponent<TeamPlayer>();  // TIDYME: 2018-07-07: should be done in Start()

        Component[] meshRenderers =
            gameObject.GetComponentsInChildren(typeof(MeshRenderer));

        if (meshRenderers == null) { Debug.Log("TowerLive: MeshRenderers is NULL!!!"); }

        // Team 0, blue.
        // Team 1, red.
        int i = 0;
        Material mat = Resources.Load((teamSelf.team == 0) ? "MatBlue" : "MatRed") as Material; // TIDYME: 2018-07-07: Can we avoid Resources.Load here, by using Material Property?
        MeshRenderer mrenderer;
        for (i = 0; i < meshRenderers.Length; i += 1) {
            mrenderer = meshRenderers[i] as MeshRenderer;
            mrenderer.material = mat as Material;
        }

        // Add a minimap icon for ourselves.
        // NOTE: This means we don't want to be able to convert a live tower, okay?
        AddMinimapIcon();
    }

    public void AddMinimapIcon () {
        Texture2D mmIconTex = (GetComponent<TeamPlayer>().team == 0) ? towerBlueMMIcon : towerRedMMIcon;
        //MinimapCamera mmCam = Camera.FindObjectOfType(typeof(MinimapCamera)) as MinimapCamera;
        MinimapCamera.AddMinimapIconTo(transform, mmIconTex);
    }

    void OnDestroy () {
        if (isQuitting) { return; }

        // The tower was destroyed; replace it with a towerDead object.
        Vector3 pt = transform.position;
        Quaternion r = transform.rotation;

        GameObject deadTower = Instantiate(Resources.Load("TowerDead"), pt, r) as GameObject; // TIDYMY 2018-07-07: Can avoid Resources.Load here
        deadTower.transform.parent = transform.parent;
    }

    void Update () {
        // Aiming
        Vector3 currentDir = turret.transform.forward;
        Vector3 goalDir;

        if (currentTarget != null) {
            // Rotate towards target.
            goalDir = currentTarget.GetComponent<TeamPlayer>().GetAimAtPoint() - turret.transform.position;

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

        Vector3 v1 = Vector3.RotateTowards(new Vector3(currentDir.x, 0, currentDir.z),
            new Vector3(goalDir.x, 0, goalDir.z),
            rotateSpeedPerSecond * Time.deltaTime,
            1);

        Vector3 v2 = Vector3.RotateTowards(new Vector3(0, currentDir.y, 0),
            new Vector3(0, goalDir.y, 0),
            rotateSpeedPerSecond * Time.deltaTime / 3,
            1);
        Vector3 res = v1 + v2;

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
        }
    }

    GameObject FindClosestTargetWithinRange (float maxRange) {
        GameObject t = FindClosestTarget();

        if (t != null && Vector3.Distance(t.transform.position, turret.transform.position) < maxRange){
            return t;
        } else {
            return null;
        }
    }

    GameObject FindClosestTarget () {
        TeamPlayer[] potentialTargets = UnityEngine.Object.FindObjectsOfType<TeamPlayer>() as TeamPlayer[];

        GameObject bestTarget = null;
        float bestDist = 99999; // TIDYME: 2018-07-07: better to use Infinity?

        // TIDYME: 2018-07-07: Use foreach loop
        for (int i = 0; i < potentialTargets.Length; i += 1) {
            TeamPlayer t = potentialTargets[i];
            var d = Vector3.Distance(turret.transform.position, t.gameObject.transform.position);

            if (t.gameObject.Equals(this)){ continue; }

            if (d < bestDist && !t.SameTeam(this.GetComponent<TeamPlayer>())) {
                bestDist = d;
                bestTarget = t.gameObject;
            }
        }

        return bestTarget;
    }

    static void SnapAllTowersToGround () {
        TowerLive[] towers = FindObjectsOfType<TowerLive>() as TowerLive[];

        // TIDYME: 2018-07-07: Use foreach loop
        for (int i = 0; i < towers.Length; i++) {
            SnapObjectToGround(towers[i].gameObject);
        }
    }

    static void SnapObjectToGround (GameObject obj) {
        // Raycast downward to find a collision with the ground.
        RaycastHit hitInfo;

        if (Physics.Raycast(obj.transform.position, new Vector3(0, -1, 0), out hitInfo, 100)) {
            GameObject hitObject = hitInfo.collider.gameObject;

            Vector3 p = obj.transform.position;
            p.y = hitInfo.point.y;
            obj.transform.position = p;
        }
    }
}
