#pragma strict

// Global static variables, hardcoded for two teams. <33
static var team0Points : int = 15;
static var team1Points : int = 15;


// Costs of things we can buy.
static var botTankCost : int = 5;
static var uncapturedBaseCost : int = 30;

// Cost of ammo to buy.
// 'Ammo' is only rockets here.
static var buyRocketCost : int = 5;
static var buyRocketAmount : int = 10;

static var buyHealthCost : int = 10;
static var buyHealthAmount : int = 100;

// Bonuses awarded for killing things.
static var killTowerBonus : int = 4;
static var killBotTankBonus : int = 3;
static var killMechBonus : int = 20;
static var killBotJetBonus : int= 40;

// Give a bonus for a player killing something? (as opposed to bots..).
static var killedByPlayerBonus : float = 0.5; // extra 50% bonus

static function GetTeamScore (team : int) {
    return (team == 0) ? team0Points : team1Points;
}

static function AddTeamScore (team : int, delta : int) {
    if (team == 0) {
        team0Points += delta;
    } else {
        team1Points += delta;
    }
}
