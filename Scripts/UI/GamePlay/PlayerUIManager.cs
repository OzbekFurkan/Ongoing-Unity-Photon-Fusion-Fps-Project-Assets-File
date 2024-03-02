using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
using Interract;

namespace GameUI
{
    public class PlayerUIManager : NetworkBehaviour
    {
        [Header("Weapon Data UI")]
        [SerializeField] TextMeshProUGUI ammoText;


        // Update is called once per frame
        void Update()
        {
            if (!Object.HasInputAuthority)
            {
                transform.GetChild(1).GetChild(0).GetChild(0).gameObject.SetActive(false);
                return;
            }

            Transform testWeaponholder = transform.GetChild(1).GetChild(0).GetChild(1);
            GameObject testWeapon = testWeaponholder.gameObject;

            if (testWeaponholder.childCount > 0)
                testWeapon = testWeaponholder.GetChild(0).gameObject;

            testWeapon.TryGetComponent<WeaponDataMono>(out var weaponData);
            if (weaponData != null && weaponData.isActiveAndEnabled)
            {
                int ammoData = weaponData.ammo;
                ammoText.text = ammoData + "";
            }
            else
            {
                ammoText.text = "";
            }
        }
    }
}

