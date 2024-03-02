using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;

namespace Interract
{
    public class ParentDropComponent : NetworkBehaviour, IDropable
    {
        private GameObject hasNoOwnerWeapon;
        private GameObject hasNoInterractPlayer;

        public TDataMono DropItem<TDataMono>() where TDataMono : ItemDataMono, ICloneable
        {
            if (!Object.HasStateAuthority)
                return null;

            hasNoOwnerWeapon = GetComponent<TDataMono>().hasNoOwnerPrefab;
            var spawnedCloneParent = Runner.Spawn(hasNoOwnerWeapon, transform.position, transform.rotation, PlayerRef.None);
            TDataMono originalParentData = GetComponent<TDataMono>();
            TDataMono cloneParentData = spawnedCloneParent.gameObject.GetComponent<TDataMono>();
            cloneParentData = originalParentData.Clone() as TDataMono;

            hasNoInterractPlayer = transform.GetChild(4).gameObject.GetComponent<PlayerDataMono>().hasOwnerPrefab;
            var spawnedClonePlayer = Runner.Spawn(hasNoInterractPlayer,
                new Vector3(transform.position.x, transform.position.y + 1, transform.position.z - 2), Quaternion.Euler(0, 0, 0), Object.InputAuthority);
            PlayerDataMono originalPlayerData = transform.GetChild(4).gameObject.GetComponent<PlayerDataMono>();
            PlayerDataMono clonePlayerData = spawnedClonePlayer.GetComponent<PlayerDataMono>();
            InitializeClonePlayerData(originalPlayerData, clonePlayerData);
            Runner.Despawn(transform.GetChild(4).gameObject.GetComponent<NetworkObject>());
            Runner.Despawn(Object);

            return originalParentData;
        }

       

        private void InitializeClonePlayerData(PlayerDataMono from, PlayerDataMono to)
        {
            if (to == null) return;

            to.inventoryItems = from.inventoryItems;
            to.team = from.team;
            to.HP = from.HP;
            to.hasInterract = false;

        }

    }
}

