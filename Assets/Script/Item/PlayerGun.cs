using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PlayerGun : MonoBehaviour, UsableItem
{
    [SerializeField]
    GameObject bulletPrefab;
    [SerializeField]
    float firingRate;
    [SerializeField]
    GameObject bulletOrigin;
    SoundGenerator soundGenerator;

    float fireTimer;
    private void Awake()
    {
        soundGenerator = GetComponent<SoundGenerator>();
    }
    public bool PrimaryUse()
    {
        return TryShoot();
    }
    public bool SecondaryUse()
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
                soundGenerator.PlaySoundOnce(0, true);
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

    public bool IsPrimary()
    {
        return true;
    }
}
