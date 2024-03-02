using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Utilitiy;
using Interract;

namespace Inventory
{
    public class InventoryManager : NetworkBehaviour, IInterractCallbacks
    {
        private PlayerDataMono playerDataMono;
        GameObject weaponHolder;
        public GameObject StartingWeapon;
        private PlayerInterractManager PIM;

        //makinali tufekten cikinca envanter geri gelmiyor. Yorum satiri olan yer silah yoksa default silahtan koyar.
        [SerializeField] List<GameObject> weaponPrefabs;

        public override void Spawned()
        {
            playerDataMono = GetComponent<PlayerDataMono>();
            PIM = GetComponent<PlayerInterractManager>();
            PIM.Register(this);
            weaponHolder = transform.GetChild(1).GetChild(0).GetChild(1).gameObject;
            InitializeInventoryItems();
        }


        public void InitializeInventoryItems()
        {
            if (!Object.HasStateAuthority)
                return;

            /*if (playerDataMono.inventoryItems.Count==0)
            {
                var spawnedWeapon = Runner.Spawn(StartingWeapon, transform.position, Quaternion.identity, Object.InputAuthority);
                spawnedWeapon.transform.SetParent(transform.GetChild(1).GetChild(0).GetChild(1));
                spawnedWeapon.transform.localPosition = Utils.GetWeaponPositon();

                playerDataMono.inventoryItems.Add(spawnedWeapon.gameObject.GetComponent<WeaponDataMono>().weaponId
                    , spawnedWeapon.gameObject.GetComponent<WeaponDataMono>());

                return;
            }*/
            foreach (KeyValuePair<ItemId, object> kvp in playerDataMono.inventoryItems)
            {
                if (weaponPrefabs[((int)kvp.Key)] == null)
                    break;

                var newItem = Runner.Spawn(weaponPrefabs[((int)kvp.Key)], transform.position, transform.rotation, Object.InputAuthority);
                SetItemTransform(newItem.gameObject);
                SetItemData(newItem.gameObject, (dynamic)kvp.Value);
            }
        }

        private void SetItemTransform(GameObject newItem)
        {
            newItem.transform.SetParent(weaponHolder.transform);
            newItem.transform.localPosition = Utils.GetWeaponPositon();
            newItem.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
        private void SetItemData(GameObject newItem, WeaponDataMono data)
        {
            WeaponDataMono originalWeaponDataMono = newItem.GetComponent<WeaponDataMono>();
            originalWeaponDataMono.ammo = data.ammo;
            originalWeaponDataMono.fullAmmo = data.fullAmmo;
        }

        public void ItemDropped<T>(ItemId itemId, T data) where T:ItemDataMono
        {
            playerDataMono.inventoryItems.Remove(itemId);
        }

        public void ItemPicked<T>(ItemId itemId, T data) where T:ItemDataMono
        {
            playerDataMono.inventoryItems.Add(itemId, data);
        }

    }
}
