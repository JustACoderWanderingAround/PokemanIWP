using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
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
    TMP_Text ammoUI;
    [SerializeField]
    int maxAmmoPerClip = 8;
    int currentAmmoCount;
    int remAmmoLeft;

    float fireTimer;
    private void Awake()
    {
        soundGenerator = GetComponent<SoundGenerator>();
        currentAmmoCount = maxAmmoPerClip;
        if(ammoUI != null)
            ammoUI.text = string.Format("{0} / {1}", maxAmmoPerClip.ToString(), maxAmmoPerClip.ToString());
        remAmmoLeft = PlayerInventory.Instance.GetCount("Ammo");
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
                ammoUI.text = string.Format("{0} / {1}", currentAmmoCount.ToString(), remAmmoLeft.ToString());
                return true;    
            }
        }
        return false;
    }

    private void Update()
    {
        if (fireTimer > 0)
            fireTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.R))
            Reload();
    }

    public bool IsRightHanded()
    {
        return true;
    }
    public void Reload()
    {
        currentAmmoCount = maxAmmoPerClip;
        PlayerInventory.Instance.RemoveItem("Ammo", maxAmmoPerClip);
        remAmmoLeft = PlayerInventory.Instance.GetCount("Ammo");
        ammoUI.text = string.Format("{0} / {1}", currentAmmoCount.ToString(), remAmmoLeft.ToString());
    }
}
