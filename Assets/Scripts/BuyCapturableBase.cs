using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyCapturableBase : MonoBehaviour {
    void Start () {
        // Set text of mesh to be our cost
        int cost = PlayerScores.uncapturedBaseCost;

        TextMesh textMesh = GetComponentInChildren<TextMesh>() as TextMesh;

        if (textMesh != null) { // TIDYME: 2018-07-07: Use Assert
            textMesh.text = "" + cost;
        }
    }

    public void CaptureBase (PlayerTankForceMovement buyer) {
        TeamBase teamBase =
            transform.parent.GetComponent<TeamBase>(); // Assume that BuyCaptureBase is a child of the base.

        if (teamBase == null) { // TIDYME: 2018-07-07: Use Assert
            Debug.Log("BuyCaptureBase could not find the base we're capturing!?");
        }

        // Set the team of the base
        if (buyer == null) { Debug.Log("CapBase.. buyer is null"); }
        if (buyer.GetComponent<TeamPlayer>() == null) { Debug.Log("CapBase.. buyer TP is null"); }
        teamBase.team = buyer.GetComponent<TeamPlayer>().team;

        // Now enable all the colliders and renderers in the children...
        // ... for some reason, the polymorphism here was wonky. *sigh*.

        Component[] colliders = teamBase.GetComponentsInChildren<Collider>(true);
        foreach (Component collider in colliders) {
            collider.GetComponent<Collider>().enabled = true;
        }

        MeshRenderer[] renderers = base.transform.GetComponentsInChildren<MeshRenderer>(true);
        foreach (MeshRenderer renderer in renderers) {
            renderer.enabled = true;
        }

        TowerLive[] towers = teamBase.transform.GetComponentsInChildren<TowerLive>(true);
        foreach (TowerLive tower in towers) {
            tower.enabled = true;
            (tower.GetComponent<TeamPlayer>() as TeamPlayer).team = teamBase.team;
            tower.UpdateTextureToTeamColor();
        }

        // And hide the buy base..
        GetComponentInChildren<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
    }
}
