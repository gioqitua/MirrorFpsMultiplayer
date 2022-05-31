using UnityEngine;
using Mirror;
using TMPro;
using System.Collections;

public class LaserGun : NetworkBehaviour
{

    [SerializeField] Transform bulletStartPos;
    [SerializeField] float _shootingDelay = 0.3f;
    [SerializeField] GameObject hitParticle;
    [SerializeField] GameObject missParticle;
    [SerializeField] GameObject shootSound;
    [SerializeField] PlayerUiManager uiManager;

    float shootingDistance = 200f;
    private float damage = 10f;
    private float _nextShootTime;

    private void Start()
    {
        uiManager = GetComponent<PlayerUiManager>();
    }

    private void Update()
    {
        if (isLocalPlayer && Input.GetMouseButton(0) && CheckIfCanSHoot())
        {
            Shoot();
            _nextShootTime = Time.time + _shootingDelay;
        }
    }


    bool CheckIfCanSHoot()
    {
        return Time.time >= _nextShootTime;
    }

    [Client]
    private void Shoot()
    {
        RpcBulletSound();

        uiManager.SetOutGoingDmgText(damage);
        
        RaycastHit hit;

        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out hit, shootingDistance))
        {
            PlayerHealth target = hit.transform.GetComponent<PlayerHealth>();

            if (target) //if hit 
            {
                CmdHitSomeone(hit.point, target);
            }
            else    //if miss 
            {
                CmdMiss(hit.point);
            }
        }
    }
    [Command]
    private void CmdMiss(Vector3 point)
    {
        var particle = Instantiate(missParticle, point, Quaternion.identity);
        NetworkServer.Spawn(particle);
    }

    [Command]
    private void CmdHitSomeone(Vector3 point, PlayerHealth target)
    {
        var particle = Instantiate(hitParticle, point, Quaternion.identity);
        NetworkServer.Spawn(particle);
        target.ChangeHealthValue(damage);
    }

    [Command]
    private void RpcBulletSound()
    {
        var gunSound = Instantiate(shootSound, bulletStartPos.position, Quaternion.identity);
        NetworkServer.Spawn(gunSound);
    }

}