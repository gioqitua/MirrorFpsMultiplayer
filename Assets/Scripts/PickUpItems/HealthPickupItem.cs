using System;
using Mirror;
using UnityEngine;

public class HealthPickupItem : PickUpItem
{
    float healthToAdd = 30f;

    
    private void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<Player>();
        if (player)
        {
            if (player.isLocalPlayer)
            {
                var PlayerHealth = player.GetComponent<PlayerHealth>();
                NewMethod(PlayerHealth);
                Debug.Log("you pickup item");
                NetworkServer.Destroy(this.gameObject);

            }
        }
    }
    [Command]
    private void NewMethod(PlayerHealth PlayerHealth)
    {
        PlayerHealth.ChangeHealthValue(-healthToAdd);
    }
}
