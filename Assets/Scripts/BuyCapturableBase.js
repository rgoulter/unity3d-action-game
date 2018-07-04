#pragma strict

function Start () {
    // Set text of mesh to be our cost
    var cost = PlayerScores.uncapturedBaseCost;

    var textMesh : TextMesh = GetComponentInChildren(TextMesh) as TextMesh;

    if (textMesh != null) {
        textMesh.text = "" + cost;
    }
}

function CaptureBase (buyer : PlayerTankForceMovement){
    var base : TeamBase;
    base = transform.parent.GetComponent(TeamBase); // Assume that BuyCaptureBase is a child of the base.

    if (base == null) {
        Debug.Log("BuyCaptureBase could not find the base we're capturing!?");
    }

    // Set the team of the base
    if (buyer == null) { Debug.Log("CapBase.. buyer is null"); }
    if (buyer.GetComponent(TeamPlayer) == null) { Debug.Log("CapBase.. buyer TP is null"); }
    base.team = buyer.GetComponent(TeamPlayer).team;

    // Now enable all the colliders and renderers in the children...
    // ... for some reason, the polymorphism here was wonky. *sigh*.
    var i : int;

    var colliders : Component[];
    colliders = base.GetComponentsInChildren.<Collider>(true);
    for (i = 0; i < colliders.length; i++) {
        colliders[i].GetComponent(Collider).enabled = true;
    }
    // */

    var renderers : MeshRenderer[] = base.transform.GetComponentsInChildren.<MeshRenderer>(true);
    for (i = 0; i < renderers.length; i++) {
        renderers[i].enabled = true;
    }

    var towers : TowerLive[] = base.transform.GetComponentsInChildren.<TowerLive>(true);
    for (i = 0; i < towers.length; i++) {
        towers[i].enabled = true;
        towers[i].GetComponent(TeamPlayer).team = base.team;
        towers[i].UpdateTextureToTeamColor();
    }

    // And hide the buy base..
    GetComponentInChildren(Renderer).enabled = false;
    GetComponent.<Collider>().enabled = false;
}
