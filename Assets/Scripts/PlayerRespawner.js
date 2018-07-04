#pragma strict

var respawnTime : float = 3.0f;

function OnEnable () {
    respawnTime = 3.0f;

    // Disable the components so that the player appears to be gone.
    GetComponent.<Collider>().enabled = false;
    GetComponent(PlayerTankForceMovement).enabled = false;
    transform.position.y = -100;

    // Disable mg shooters, so we can't shoot when we are dead.
    var comps : BulletShooter[] = transform.GetComponentsInChildren.<BulletShooter>(true);
    for(var i = 0; i < comps.length; i++){
        comps[i].enabled = false;
    }
}

function OnDisable () {
    // Re-enable shooters because we disabled them, as a cheap hack.
    var comps : BulletShooter[] = transform.GetComponentsInChildren.<BulletShooter>(true);
    for(var i = 0; i < comps.length; i++){
        comps[i].enabled = true;
    }
}

function Update () {
    respawnTime -= Time.deltaTime;

    if (respawnTime <= 0) {
        // Need to disable the respawn component,
        enabled = false;

        // and re-enable the components which we disabled before.
        GetComponent.<Collider>().enabled = true;
        GetComponent(PlayerTankForceMovement).enabled = true;


        // and "Respawn"
        // it's easier to just retain the object as it was.
        var bestDist : float = Mathf.Infinity;
        var baseToRespawnAt : TeamBase;
        var bases : TeamBase[] = FindObjectsOfType(TeamBase) as TeamBase[];

        for (var base : TeamBase in bases) {
            var distanceTo : float = Vector3.Distance(transform.position, base.transform.position);

            // The closest base of the same team.
            if (base.team == gameObject.GetComponent(TeamPlayer).team &&
                distanceTo < bestDist) {
                baseToRespawnAt = base;
                bestDist = distanceTo;
            }
        }

        if (baseToRespawnAt == null) {
            // Game over...
        }

        GetComponent(TeamPlayer).health = 100;
        transform.position = baseToRespawnAt.mechSpawnPoint.position;
        transform.rotation = baseToRespawnAt.mechSpawnPoint.rotation;

        // Get the player UI, so we can re-show the objective.
        GetComponent(PlayerUI).objectiveOpacity = 0.5 + 1; // MAGIC.
    }
}
