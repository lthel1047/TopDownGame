using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform muzzle;
    public Projectile projectile;
    public float msBetweenShots = 100;
    public float muzzleVelocity = 35;

    public Transform shell;
    public Transform shellEjection;
    Muzzleflash muzzleflash;

    void Start(){
        muzzleflash = GetComponent<Muzzleflash>();
    }
    float nextShootTime;

    public void Shoot(){
        if(Time.time > nextShootTime){
        nextShootTime = Time.time + msBetweenShots / 1000;
        Projectile newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation) as Projectile;
        newProjectile.SetSpeed(muzzleVelocity);

        Instantiate(shell, shellEjection.position, shellEjection.rotation);
        muzzleflash.Activate();
        }
    }
}
