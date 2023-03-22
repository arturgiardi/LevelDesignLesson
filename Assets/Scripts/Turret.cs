using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [field: SerializeField] private float ShotCooldown { get; set; }
    [field: SerializeField] private Transform BulletSpawnPoint { get; set; }
    [field: SerializeField] private Bullet Bullet { get; set; }

    private float elapsedTime;
    private bool isOn = true;
    
    void Update()
    {
        if(!isOn)
            return;

        elapsedTime += Time.deltaTime;
        if(elapsedTime >= ShotCooldown)
        {
            Shot();
            elapsedTime = 0;
        }
    }

    private void Shot()
    {
        var bullet = Instantiate(Bullet, BulletSpawnPoint.position, Quaternion.identity);
        Vector2 direction = (Vector2)(BulletSpawnPoint.position - transform.position).normalized;
        bullet.Shot(direction);
    }

    public void TurnOn() => isOn = true;
    public void TurnOff() => isOn = false;
}
