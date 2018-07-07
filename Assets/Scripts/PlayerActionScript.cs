using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionScript : MonoBehaviour {
    string actionAxis = "Jump";
    float actionFrequency = 0.5f;
    private float t = 0.0f;

    public string status;

    void OnTriggerStay (Collider other) {
        if (t > 0) {
            // Place a limit on how frequently actions will happen,
            // if the button is pressed down.
            return;
        }

        TeamPlayer mechTeamPlayer = transform.parent.GetComponent<TeamPlayer>();
        status = null;

        // For "Capturing" towers
        if (other.GetComponent<TowerInactive>() != null) {
            status = "< Press Action to Capture Tower >";

            if (Input.GetAxis(actionAxis) > 0) {
                TowerInactive inactiveTower = other.GetComponent<TowerInactive>();

                inactiveTower.ReplaceWithLiveTower(mechTeamPlayer.team);

                t = 1.0f / actionFrequency;
            }
        }

        // For buying bot tanks, jets
        if (other.GetComponent<BuyBotPOS>() != null) {
            if (PlayerScores.GetTeamScore(mechTeamPlayer.team) >= PlayerScores.botTankCost) {
                status = "< Press Action to Buy Bot >";
            } else {
                status = "< You Do not have enough points to Buy a Bot >";
            }

            if (Input.GetAxis(actionAxis) > 0 &&
                PlayerScores.GetTeamScore(mechTeamPlayer.team) >= PlayerScores.botTankCost) {
                // Get the base, which is the parent of the BuyBotPOS gameobj
                TeamBase teamBase = other.gameObject.transform.parent.GetComponent<TeamBase>();

                PlayerScores.AddTeamScore(mechTeamPlayer.team, -PlayerScores.botTankCost);

                // Will only spawn tanks for now..... TODO
                teamBase.SpawnBotTank();

                t = 1.0f / actionFrequency;
            }
        }



        // For buying ammo
        if (other.GetComponent<BuyAmmoPOS>() != null) {
            if (PlayerScores.GetTeamScore(mechTeamPlayer.team) >= PlayerScores.buyRocketCost) {
                status = "< Press Action to Buy Ammo >";
            } else {
                status = "< You Do not have enough points to Buy Ammo >";
            }

            if (Input.GetAxis(actionAxis) > 0 &&
                PlayerScores.GetTeamScore(mechTeamPlayer.team) >= PlayerScores.buyRocketCost) {

                // Get the weapon we add the ammo to.
                Transform rktShooter = transform.parent.Find("playerRktShooter"); // Somewhat MAGIC; 2018-07-07: Use Property, not Find

                if (rktShooter == null) {
                    Debug.Log("Couldn't find playerRktShooter of Player.");
                }

                // Add howevermany hitpoints to player's health,
                // and deduct however many points
                rktShooter.GetComponent<BulletShooter>().ammoCount = PlayerScores.buyRocketAmount;
                PlayerScores.AddTeamScore(mechTeamPlayer.team, -PlayerScores.buyRocketCost);

                t = 1.0f / actionFrequency;
            }
        }

        // For Health
        if (other.GetComponent<BuyHealthPOS>() != null) {

            if (PlayerScores.GetTeamScore(mechTeamPlayer.team) >= PlayerScores.buyHealthCost) {
                status = "< Press Action to Buy Health >";
            } else {
                status = "< You Do not have enough points to Buy Health >";
            }

            if (Input.GetAxis(actionAxis) > 0 &&
                PlayerScores.GetTeamScore(mechTeamPlayer.team) >= PlayerScores.buyHealthCost) {

                // Add howevermany hitpoints to player's health (max 100. MAGIC).
                // and deduct however many points
                mechTeamPlayer.health = Mathf.Min(100, mechTeamPlayer.health + PlayerScores.buyHealthAmount);
                PlayerScores.AddTeamScore(mechTeamPlayer.team, -PlayerScores.buyHealthCost);

                t = 1.0f / actionFrequency;
            }
        }

        // For buying/capturing a base
        if (other.GetComponent<BuyCapturableBase>() != null) {
            if (PlayerScores.GetTeamScore(mechTeamPlayer.team) >= PlayerScores.uncapturedBaseCost) {
                status = "< Press Action to Buy Forward Base >";
            } else {
                status = "< You Do not have enough points to Buy the Forward Base >";
            }

            if (Input.GetAxis(actionAxis) > 0 &&
                PlayerScores.GetTeamScore(mechTeamPlayer.team) >= PlayerScores.uncapturedBaseCost) {
                // Deduct the appropriate amount.
                PlayerScores.AddTeamScore(mechTeamPlayer.team, -PlayerScores.uncapturedBaseCost);

                other.GetComponent<BuyCapturableBase>().CaptureBase(transform.parent.GetComponent<PlayerTankForceMovement>());

                t = 1.0f / actionFrequency;
            }
        }
    }

    void Update () {
        if (t > 0) {
            if (Input.GetAxis(actionAxis) > 0) {
                // If the action button is being held down...
                t -= Time.deltaTime;
            } else {
                // If the button has been released, then the player is allowed to 'action' again.
                t = 0;
            }
        }
    }
}
