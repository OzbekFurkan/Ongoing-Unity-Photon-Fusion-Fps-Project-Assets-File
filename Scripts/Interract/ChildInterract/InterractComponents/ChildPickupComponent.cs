using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;
using Utilitiy;

namespace Interract
{
    public class ChildPickupComponent : NetworkBehaviour, IPickupable
    {
        private GameObject hasOwnerWeapon;

        public TDataMono PickUpItem<TDataMono>(PlayerRef newOwner, GameObject parentPlayerObject) where TDataMono : ItemDataMono, ICloneable
        {
            if (!Object.HasStateAuthority)
                return null;

            hasOwnerWeapon = GetComponent<TDataMono>().hasOwnerPrefab;
            var spawnedCloneWeapon = Runner.Spawn(hasOwnerWeapon, transform.position, transform.rotation, newOwner);
            TDataMono originalChildData = GetComponent<TDataMono>();
            TDataMono cloneChildData = spawnedCloneWeapon.gameObject.GetComponent<TDataMono>();
            cloneChildData = originalChildData.Clone() as TDataMono;
            SetCloneWeaponTransformData(cloneChildData.gameObject, parentPlayerObject);
            Runner.Despawn(Object);

            return originalChildData;
        }

        private void SetCloneWeaponTransformData(GameObject cloneWeapon, GameObject parent)
        {
            cloneWeapon.transform.SetParent(parent.transform.GetChild(1).GetChild(0).GetChild(1));
            cloneWeapon.transform.localPosition = Utils.GetWeaponPositon();
            cloneWeapon.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }



    }
}
