using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System;
using System.Collections;

public class PlayerHealth : NetworkBehaviour
{
    private const float maxhealth = 100f;
    [SyncVar(hook = "SyncHealth")][SerializeField] public float syncHealth = 100f;
    [SerializeField] Slider healthBarSlider;
    private void Start()
    {
        healthBarSlider.value = syncHealth / maxhealth;
    }
    void SyncHealth(float oldValue, float newValue)
    {
        healthBarSlider.value = newValue / maxhealth;
    }

    [Server]
    internal void ChangeHealthValue(float damage)
    {
        syncHealth -= damage;

        if (syncHealth <= 0)
        {
            PlayerDie(gameObject);
        }
    }
    [Server]
    private void PlayerDie(GameObject player)
    {
        StartCoroutine(SpawnPlayer(player, 2f)); 
    }
    [Server]
    IEnumerator SpawnPlayer(GameObject _player, float delay)
    {

        var newPos = NetworkManager.singleton.GetStartPosition();

        _player.transform.position = newPos.position; 

        yield return new WaitForSeconds(delay); 

        syncHealth = maxhealth;

    }
}