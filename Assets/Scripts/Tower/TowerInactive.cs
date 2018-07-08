using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerInactive : MonoBehaviour {
    public Texture blueTex;
    public Texture redTex;

    public Texture2D towerGreyMMIcon;

    private int replacedByTeam = -1;

    void Start () {
        AddMinimapIcon();
    }

    public void ReplaceWithLiveTower (int team) {
        if (replacedByTeam >= 0) {
            return;
        }
        replacedByTeam = team;

        Vector3 pt = gameObject.transform.position;
        Quaternion r = gameObject.transform.rotation;

        Destroy(gameObject);

        GameObject newTower =
            Instantiate(Resources.Load("TowerLive"), pt, r) as GameObject;
        newTower.transform.parent = transform.parent;

        // For now, we will only replace with blue team.
        Debug.Log("Replacing tower, new team = " + team);

        newTower.GetComponent<TeamPlayer>().team = team;
        newTower.GetComponent<TowerLive>().UpdateTextureToTeamColor();
    }

    void AddMinimapIcon () {
        Texture2D mmIconTex = towerGreyMMIcon;
        //MinimapCamera mmCam = Camera.FindObjectOfType<MinimapCamera>();
        MinimapCamera.AddMinimapIconTo(transform, mmIconTex);
    }
}
