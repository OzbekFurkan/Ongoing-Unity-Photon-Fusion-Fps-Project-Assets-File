using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;

namespace Interract
{
    public class ParentPickupComponent : NetworkBehaviour, IPickupable
    {
        private GameObject hasOwnerWeapon;
        private GameObject hasInterractPlayer;

        public TDataMono PickUpItem<TDataMono>(PlayerRef newOwner, GameObject ownerPlayerObject) where TDataMono : ItemDataMono, ICloneable
        {
            if (!Object.HasStateAuthority)
                return null;

            hasOwnerWeapon = GetComponent<TDataMono>().hasOwnerPrefab;
            var spawnedCloneParent = Runner.Spawn(hasOwnerWeapon, transform.position, transform.rotation, newOwner);
            TDataMono originalParentData = GetComponent<TDataMono>();
            TDataMono cloneParentData = spawnedCloneParent.gameObject.GetComponent<TDataMono>();
            cloneParentData = originalParentData.Clone() as TDataMono;


            hasInterractPlayer = ownerPlayerObject.GetComponent<PlayerDataMono>().hasNoOwnerPrefab;
            var spawnedClonePlayer = Runner.Spawn(hasInterractPlayer,
                new Vector3(transform.localPosition.x, transform.localPosition.y + 1, transform.localPosition.z),
                Quaternion.identity, newOwner);
            spawnedClonePlayer.gameObject.transform.SetParent(spawnedCloneParent.gameObject.transform);
            spawnedClonePlayer.transform.localPosition += new Vector3(0, 0, -1.7f);
            PlayerDataMono originalPlayerData = ownerPlayerObject.GetComponent<PlayerDataMono>();
            PlayerDataMono clonePlayerData = spawnedClonePlayer.GetComponent<PlayerDataMono>();
            InitializeClonePlayerData(originalPlayerData, clonePlayerData);


            Runner.Despawn(ownerPlayerObject.GetComponent<NetworkObject>());
            Runner.Despawn(Object);

            return originalParentData;
        }

        private void InitializeClonePlayerData(PlayerDataMono from, PlayerDataMono to)
        {
            if (to == null) return;

            to.inventoryItems = from.inventoryItems;
            to.team = from.team;
            to.HP = from.HP;
            to.hasInterract = true;

        }
    }
}
