using UnityEngine;
using Mirror;
using UnityEngine.UI;

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
    public void ChangeHealthValue(float damage)
    {
        syncHealth -= damage;

        if (syncHealth < 0f)
        {
            RespawnPlayer(gameObject);
            syncHealth = maxhealth;
        }
    }
    [TargetRpc]
    void RespawnPlayer(GameObject _player)
    {
        var newPos = NetworkManager.singleton.GetStartPosition();
        _player.transform.position = newPos.position;

    }

}