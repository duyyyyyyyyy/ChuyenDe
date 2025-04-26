using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPrefab;

    public void PlayerJoined(PlayerRef player)
    {
        // Chỉ spawn một player cho LocalPlayer, không spawn lại cho các player khác
        if (player == Runner.LocalPlayer)
        {
            Runner.Spawn(PlayerPrefab, new Vector3(1, 1, 1), Quaternion.identity, Runner.LocalPlayer, (runner, obj) =>
            {
                var playerGun = obj.GetComponent<PlayerGun>();
                if (playerGun != null) playerGun.networkRunner = runner;
                var playerProperties = obj.GetComponent<PlayerProperties>();
                if(playerProperties != null)
                {
                    playerProperties.networkRunner = runner;
                    playerProperties.networkObject = obj;
                }    
            });
        }
    }
}
