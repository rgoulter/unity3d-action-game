#pragma strict

/*
  Time to wait before spawning a new tower int this position.
  */
var spawnDelay : float = 5.0f;

function Start () {
    spawnDelay = 5.0f;
}

function Update () {
    spawnDelay -= Time.deltaTime;

    if (spawnDelay < 0) {
        // Destroy me, and make an inactive tower here.
        Destroy(gameObject);
        var blankTower : GameObject = Instantiate(Resources.Load("TowerBlank"),
            gameObject.transform.position,
            gameObject.transform.rotation) as GameObject;
        blankTower.transform.parent = transform.parent;

        // If this was on a captured base, set the team to the base team.
        if (blankTower.transform.parent != null &&
            blankTower.transform.parent.GetComponent(TeamBase) != null) {
            // Get the team, and replace with live tower..
            var team = blankTower.transform.parent.GetComponent(TeamBase).team;
            blankTower.GetComponent(TowerInactive).ReplaceWithLiveTower(team);
        }
    }
}
