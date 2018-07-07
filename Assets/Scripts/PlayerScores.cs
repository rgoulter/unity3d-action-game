using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScores : MonoBehaviour {
    // Global static variables, hardcoded for two teams. <33
    static int team0Points = 15;
    static int team1Points = 15;


    // Costs of things we can buy.
    public static int botTankCost = 5;
    public static int uncapturedBaseCost = 30;

    // Cost of ammo to buy.
    // 'Ammo' is only rockets here.
    public static int buyRocketCost = 5;
    public static int buyRocketAmount = 10;

    public static int buyHealthCost = 10;
    public static int buyHealthAmount = 100;

    // Bonuses awarded for killing things.
    public static int killTowerBonus = 4;
    public static int killBotTankBonus = 3;
    public static int killMechBonus = 20;
    public static int killBotJetBonus = 40;

    // Give a bonus for a player killing something? (as opposed to bots..).
    public static float killedByPlayerBonus = 0.5f; // extra 50% bonus

    public static int GetTeamScore (int team) {
        return (team == 0) ? team0Points : team1Points;
    }

    public static void AddTeamScore (int team, int delta) {
        if (team == 0) {
            team0Points += delta;
        } else {
            team1Points += delta;
        }
    }
}
