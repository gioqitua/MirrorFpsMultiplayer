using System;
using Mirror;
using UnityEngine;

public class HealthPickupItem : PickUpItem
{
    float healthToAdd = 35f; 

    private void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<Player>();

        var PlayerHealth = player.GetComponent<PlayerHealth>();

        if (player.isServer)
        {
            PlayerHealth.ChangeHealthValue(-healthToAdd);

            NetworkServer.Destroy(this.gameObject);

            Debug.Log("you pickup item"); 

        }
        else if (player.isClientOnly)
        {
            AddHealth(PlayerHealth);
            Debug.Log("you pickup item");
        }

    }
    [Command]
    private void AddHealth(PlayerHealth PlayerHealth)
    {
        PlayerHealth.ChangeHealthValue(-healthToAdd);
        NetworkServer.Destroy(this.gameObject);
    }
}
