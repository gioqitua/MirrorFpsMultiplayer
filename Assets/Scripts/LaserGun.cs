using UnityEngine;
using Mirror;

public class LaserGun : NetworkBehaviour
{
    [SerializeField] Transform bulletStartPos;
    [SerializeField] Transform bulletDirection;
    [SerializeField] float _shootingDelay = 0.1f;
    [SerializeField] GameObject hitParticle;
    [SerializeField] GameObject missParticle;
    [SerializeField] GameObject muzzlefleshParticle;
    [SerializeField] GameObject shootSound;
    [SerializeField] PlayerUiManager uiManager;
    [SerializeField] GameObject aim;
    float shootingDistance = 200f;
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
        CmdBulletSound();

        CmdMuzzleFlashParticle();

        float damage = Random.Range(8, 12);

        RaycastHit hit;

        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out hit, shootingDistance))
        {
            PlayerHealth target = hit.transform.GetComponent<PlayerHealth>();
            aim.transform.position = hit.point; //virtual aim, just for animations
            Debug.Log(hit.transform.gameObject.name);

            if (target) //if hit 
            {
                uiManager.CreateDmgText(damage, hit.point, this.gameObject.transform.position);

                if (isServer)
                {
                    DealDamage(damage, hit, target);

                }
                else
                {
                    CmdDealDamage(hit.point, target, damage);

                }
            }
            else    //if miss 
            {
                CmdMissParticle(hit.point);
            }
        }
    }
    [Command]
    private void CmdMuzzleFlashParticle()
    {

        var particle = Instantiate(muzzlefleshParticle, bulletStartPos.position, Quaternion.identity);

        NetworkServer.Spawn(particle);
    }
    [Server]
    private void MuzzleFlashParticle()
    {
        var particle = Instantiate(muzzlefleshParticle, bulletStartPos.position, Quaternion.identity);

        NetworkServer.Spawn(particle);
    }

    private void DealDamage(float damage, RaycastHit hit, PlayerHealth target)
    {

        target.ChangeHealthValue(damage);
        var particle = Instantiate(hitParticle, hit.point, Quaternion.identity);
        NetworkServer.Spawn(particle);
    }

    [Command]
    private void CmdMissParticle(Vector3 point)
    {

        var particle = Instantiate(missParticle, point, Quaternion.identity);
        NetworkServer.Spawn(particle);
    }

    [Command]
    private void CmdDealDamage(Vector3 point, PlayerHealth player, float damage)
    {
        player.ChangeHealthValue(damage);

        var particle = Instantiate(hitParticle, point, Quaternion.identity);
        NetworkServer.Spawn(particle);
    }

    [Command]
    private void CmdBulletSound()
    {
        var gunSound = Instantiate(shootSound, bulletStartPos.position, Quaternion.identity);
        NetworkServer.Spawn(gunSound);
    }

}