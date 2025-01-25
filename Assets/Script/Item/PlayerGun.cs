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
    [SerializeField]
    TMPro.TMP_Text ammoUI;
    [SerializeField]
    int maxAmmoPerClip = 8;
    int currentAmmoCount; 

    float fireTimer;
    private void Awake()
    {
        soundGenerator = GetComponent<SoundGenerator>();
        currentAmmoCount = maxAmmoPerClip;
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
            if (fireTimer <= 0 && currentAmmoCount > 0)
            {
                fireTimer = 60 * (1 / firingRate);
                Instantiate(bulletPrefab, bulletOrigin.transform);
                soundGenerator.PlaySoundOnce(0, true);
                currentAmmoCount--;
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

    public bool IsRightHanded()
    {
        return true;
    }
    public void Reload()
    {
        currentAmmoCount = maxAmmoPerClip;
        PlayerInventory.Instance.RemoveItem("Ammo");
    }
}
