#pragma strict

var winPoints : int = 20;
private var won : boolean = false;

static var description : String = "Get over 20 points to win the game";

function Update () {
    if (PlayerScores.GetTeamScore(0) > winPoints) {
        WinForTeam(0);
    }

    if (PlayerScores.GetTeamScore(1) > winPoints) {
        WinForTeam(1);
    }
}

function HasBeenWon () {
    return won;
}

function WinForTeam (team : int) {
    won = true;

    // We want to disable(?) the PlayerUIs,
    // So we can render uninterrupted.
    var i : int = 0;
    var playerUIs : PlayerUI[] = FindObjectsOfType(PlayerUI) as PlayerUI[];
    for (i = 0; i < playerUIs.length; i++) {
        playerUIs[i].enabled = false;
    }

    // Pause the game's time
    Time.timeScale = 0;
}

function OnGUI () {
    if (!won) { return; }

    var style : GUIStyle = new GUIStyle();

    style.normal.textColor = Color.white;
    style.fontSize = 56;
    style.alignment = TextAnchor.UpperCenter;

    // Label for MG ammo
    GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height / 2 - 65, 150, 50), "Game Over", style);
    GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height / 2 + 65, 150, 50), "Player won.", style);
}
