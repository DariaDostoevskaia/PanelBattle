using LegoBattaleRoyal.ScriptableObjects.PlayerData;
using UnityEngine;

public class PlayerFactory : MonoBehaviour
{
    public Player CreatePlayer(PlayerData playerData = null)
    {
        var player = new Player();
        return player;
    }

    public PlayerData CreatePlayerData(Player player)
    {
        var playerData = new PlayerData();
        return playerData;
    }
}