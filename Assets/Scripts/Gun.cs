using UnityEngine;
using Mirror;
using System.Collections;
using TMPro;

public class Gun : NetworkBehaviour
{
    [SerializeField] Transform bulletStartPos;
    [SerializeField] Transform bulletDirection;
    [SerializeField] Transform bulletShellPos;
    [SerializeField] float _shootingDelay = 0.1f;
    [SerializeField] GameObject hitParticle;
    [SerializeField] GameObject missParticle;
    [SerializeField] GameObject muzzlefleshParticle;
    [SerializeField] GameObject shootSound;
    [SerializeField] PlayerUiManager uiManager;
    [SerializeField] GameObject aim;
    [SerializeField] GameObject bulletShell;
    [SerializeField] TMP_Text ammoText;
    [SerializeField] AudioSource playerAudio;
    [SerializeField] AudioClip reloadSound;
    AnimationManager animationManager;
    [SerializeField] GameObject bulletTrail;
    float shootingDistance = 20000f;
    private float _nextShootTime;
    const int maxAmmo = 30;
    int currentAmmo;
    float reloadTime = 1.5f;
    bool reloading = false;

    private void Start()
    {
        if (!isLocalPlayer) return;
        uiManager = GetComponent<PlayerUiManager>();
        animationManager = GetComponent<AnimationManager>();
        currentAmmo = maxAmmo;
        SetAmmoText();
    }

    private bool CheckAmmo()
    {
        if (currentAmmo <= 0) return false;
        else return true;
    }

    private void SetAmmoText()
    {
        if (!isLocalPlayer) return;
        ammoText.SetText(currentAmmo + "/" + maxAmmo);
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetMouseButton(0) && CheckShootDelay() && CheckAmmo())
        {
            Shoot();
            _nextShootTime = Time.time + _shootingDelay;
        }
        Reload();

    }

    private void Reload()
    {
        if (reloading) return;
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo != maxAmmo
        || currentAmmo == 0 && Input.GetMouseButton(0)) StartCoroutine(Reloading());
    }

    IEnumerator Reloading()
    {
        reloading = true;
        animationManager.Reload();
        playerAudio.PlayOneShot(reloadSound);
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        SetAmmoText();
        reloading = false;
    }
    bool CheckShootDelay()
    {
        return Time.time >= _nextShootTime;
    }

    [Client]
    private void Shoot()
    {
        if (!isLocalPlayer) return;

        currentAmmo--;

        SetAmmoText();

        animationManager.FireAnimation();

        CmdBulletSound();

        CmdMuzzleFlashParticle();

        CmdBulletShellParticle();

        float damage = Random.Range(8, 12);

        RaycastHit hit;

        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out hit, shootingDistance))
        {
            aim.transform.position = hit.point; //virtual aim, just for animations 
            PlayerHealth target = hit.collider.gameObject.GetComponent<PlayerHealth>();
            SetBulletTrails(hit.point);
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

    private void SetBulletTrails(Vector3 hitPoint)
    {
        var trail = Instantiate(bulletTrail, bulletStartPos.position, Quaternion.identity);
        var line = trail.GetComponent<LineRenderer>();
        line.SetPosition(0, bulletStartPos.position);
        line.SetPosition(1, hitPoint);
    }

    [Command]
    private void CmdBulletShellParticle()
    {
        var newBulletShell = Instantiate(bulletShell, bulletShellPos.position, Quaternion.identity);

        newBulletShell.GetComponent<Rigidbody>().AddForce(Vector3.right, ForceMode.Impulse);

        NetworkServer.Spawn(newBulletShell);

        StartCoroutine(DestroyShells(newBulletShell));

    }
    [Server]
    IEnumerator DestroyShells(GameObject shell)
    {
        yield return new WaitForSeconds(1f);

        Destroy(shell);
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
    private void CmdDealDamage(Vector3 point, PlayerHealth player, float damage)
    {
        player.ChangeHealthValue(damage);

        var particle = Instantiate(hitParticle, point, Quaternion.identity);
        NetworkServer.Spawn(particle);
    }

    [Command]
    private void CmdMissParticle(Vector3 point)
    {

        var particle = Instantiate(missParticle, point, Quaternion.identity);
        NetworkServer.Spawn(particle);
    }


    [Command]
    private void CmdBulletSound()
    {
        var gunSound = Instantiate(shootSound, bulletStartPos.position, Quaternion.identity);
        NetworkServer.Spawn(gunSound);
    }

}