#pragma strict

function Start () {
    // Set text of mesh to be our cost
    var cost = PlayerScores.buyRocketCost;

    var textMesh : TextMesh = GetComponentInChildren(TextMesh) as TextMesh;

    if (textMesh != null) {
        textMesh.text = "" + cost;
    }
}
