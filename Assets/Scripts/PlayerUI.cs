using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour {
    Texture2D mgIcon;
    BulletShooter mgShooter;
    Texture2D rktIcon;
    BulletShooter rktShooter;

    Rect normalizedGuiRect;

    private Texture2D healthBgTex;
    private Texture2D healthFgTex;


    // It would be a good idea to show the player a description of the objective,
    // at the start of the game, and every time they respawn.
    // Easiest way to manage this is with an opacity value here.
    public float objectiveOpacity = 2 + 1; // Stay for 3, fade for 1.


    void Start () {
        // Red BG for healthbar
        healthBgTex = new Texture2D(1, 1, TextureFormat.RGB24, false);
        healthBgTex.SetPixel(0, 0, Color.red);
        healthBgTex.Apply();

        healthFgTex = new Texture2D(1, 1, TextureFormat.RGB24, false);
        healthFgTex.SetPixel(0, 0, Color.green);
        healthFgTex.Apply();
    }

    void Update () {
        if (objectiveOpacity > 0) {
            // Debug.Log("A " + objectiveOpacity + " - " + Time.deltaTime);
            objectiveOpacity -= Time.deltaTime;
        }
    }

    void OnGUI () {
        GUIStyle style = new GUIStyle();

        //Color c = Color.white
        //style.normal.textColor = Color.white;
        //style.normal.textColor.a = 1;
        Color c = Color.white;
        c.a = 1;
        style.normal.textColor = c;
        style.fontSize = 36;

        char infinite = '∞';

        float x1 = Screen.width * normalizedGuiRect.xMin;
        float x2 = Screen.width * normalizedGuiRect.xMax;
        float y1 = Screen.height * normalizedGuiRect.yMin;
        float y2 = Screen.height * normalizedGuiRect.yMax;


        // Label for MG ammo
        GUI.Label(new Rect(x1 + 15, y2 - 65, 150, 50), new GUIContent("" + mgShooter.ammoCount, mgIcon), style);

        // Label for Rkt ammo
        GUI.Label(new Rect(x1 + 15 + 15 + 150, y2 - 65, 150, 50), new GUIContent("" + rktShooter.ammoCount, rktIcon), style);


        // Label for the Score/Points
        style.fontSize = 42;
        style.alignment = TextAnchor.LowerRight;
        int score = PlayerScores.GetTeamScore(GetComponent<TeamPlayer>().team);
        GUI.Label(new Rect(x2 - 200, y2 - 65, 150, 50), "Score: " + score, style);


        // Show the Action text
        if (GetComponentInChildren<PlayerActionScript>().status != null) {
            style.fontSize = 36;
            style.alignment = TextAnchor.MiddleCenter;
            GUI.Label(new Rect((x1 + x2) / 2 - 250, y1 + 3 * (y2 - y1) / 4, 500, 50), GetComponentInChildren<PlayerActionScript>().status, style);
        }


        // Healthbar
        GUI.backgroundColor = new Color(1, 0, 0);
        GUI.Button(new Rect(x1 + 10, y1 + 20, 300, 40), "Health");
        GUI.backgroundColor = new Color(0, 1, 0);
        GUI.Button(new Rect(x1 + 10, y1 + 20, (GetComponent<TeamPlayer>().health / 100) * 300, 40), "");


        // Respawn text
        GUIStyle centeredStyle = GUI.skin.GetStyle("Label");
        centeredStyle.normal.textColor = Color.red;
        centeredStyle.fontSize = 45;
        centeredStyle.alignment = TextAnchor.UpperCenter;
        if (GetComponent<PlayerRespawner>().enabled) {
            var textWidth = 500;
            var textHeight = 200;

            GUI.Label(new Rect(x1 + (x2 - x1 - textWidth) / 2,
                y1 + (y2 - y1 - textHeight) / 2,
                textWidth,
                textHeight),
                "You were destroyed.\n" +
                "Respawning in:\n" +
                      GetComponent<PlayerRespawner>().respawnTime.ToString("#.000"),
                centeredStyle);
        }


        if (objectiveOpacity > 0) {
            Color vc = Color.white;
            vc.a = objectiveOpacity;
            centeredStyle.normal.textColor = vc;
            GUI.Label(new Rect(x1 + (x2 - x1 - 400) / 2,
                y1 + 100,
                400,
                100),
                VictoryConditions.description,
                centeredStyle);
        }
    }
}
