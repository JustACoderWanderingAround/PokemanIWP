using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PlayerGun : UsableItem
{
    [SerializeField]
    GameObject bulletPrefab;
    [SerializeField]
    float firingRate;
    [SerializeField]
    GameObject bulletOrigin;

    float fireTimer;

    public override bool PrimaryUse()
    {
        return TryShoot();
    }
    public override bool SecondaryUse()
    {
        return false;
    }
    bool TryShoot()
    {
        if (bulletPrefab != null)
        {
            if (fireTimer <= 0)
            {
                fireTimer = 60 * (1 / firingRate);
                Instantiate(bulletPrefab, bulletOrigin.transform);
                return true;
            }
        }
        return false;
    }

    private void Update()
    {
        if (fireTimer > 0)
            fireTimer -= Time.deltaTime;
    }
}
