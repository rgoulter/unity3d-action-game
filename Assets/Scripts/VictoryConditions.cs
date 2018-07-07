using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryConditions : MonoBehaviour {
    int winPoints = 20;
    private bool won = false;

    public static string description = "Get over 20 points to win the game"; // TIDYME: 2018-07-07: Use C# Constant

    void Update () {
        if (PlayerScores.GetTeamScore(0) > winPoints) {
            WinForTeam(0);
        }

        if (PlayerScores.GetTeamScore(1) > winPoints) {
            WinForTeam(1);
        }
    }

    bool HasBeenWon () {
        return won;
    }

    void WinForTeam (int team) {
        won = true;

        // We want to disable(?) the PlayerUIs,
        // So we can render uninterrupted.
        int i = 0;
        PlayerUI[] playerUIs = FindObjectsOfType<PlayerUI>() as PlayerUI[];
        for (i = 0; i < playerUIs.Length; i++) { // TIDYME: 2018-07-07: Use foreach loop
            playerUIs[i].enabled = false;
        }

        // Pause the game's time
        Time.timeScale = 0;
    }

    void OnGUI () {
        if (!won) { return; }

        GUIStyle style = new GUIStyle();

        style.normal.textColor = Color.white;
        style.fontSize = 56;
        style.alignment = TextAnchor.UpperCenter;

        // Label for MG ammo
        GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height / 2 - 65, 150, 50), "Game Over", style);
        GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height / 2 + 65, 150, 50), "Player won.", style);
    }
}
