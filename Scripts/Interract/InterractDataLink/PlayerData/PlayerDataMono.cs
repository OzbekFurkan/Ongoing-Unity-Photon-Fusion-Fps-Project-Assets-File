using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace Interract
{
    public class PlayerDataMono : ItemDataMono
    {
        [SerializeField] PlayerDataSettings playerDataSettings;
        [HideInInspector] public Dictionary<ItemId, object> inventoryItems;
        [HideInInspector] public Team team;
        [HideInInspector] public byte HP;
        [HideInInspector] public bool hasInterract;

        public override void Spawned()
        {
            SetPrefabDatas();
            inventoryItems = new Dictionary<ItemId, object>();
            team = playerDataSettings.team;
            HP = playerDataSettings.HP;
            hasInterract = playerDataSettings.hasInterract;
        }

        public override void FixedUpdateNetwork()
        {

        }

        public override void Render()
        {

        }


    }
}

