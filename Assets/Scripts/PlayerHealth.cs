using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System;

public class PlayerHealth : NetworkBehaviour
{
    private const float maxhealth = 100f;
    [SyncVar][SerializeField] public float health = 100f;
    [SerializeField] Slider healthBarSlider;
    int score = 0;

    public override void OnStartClient()
    {
        base.OnStartClient();

        SetHealthSlider();
    }
    [ClientRpc]
    private void SetHealthSlider()
    {
        healthBarSlider.value = health / maxhealth;
    }

    [ClientRpc]
    internal void ChangeHealthValue(float damage)
    {
        health -= damage;
        SetHealthSlider();
        if (health <= 0)
        {
            PlayerDie(this.gameObject);
        }
    }
    [Server]
    private void PlayerDie(GameObject player)
    {
        health = maxhealth;
        SetHealthSlider();
        var newPos = NetworkManager.singleton.GetStartPosition();
        player.transform.position = newPos.position;
    }
}