using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;


namespace Interract
{
    public abstract class ItemDataMono : NetworkBehaviour
    {
        [SerializeField] ItemDataSettings itemDataSettings;

        [HideInInspector] public ItemId itemId;
        [HideInInspector] public GameObject hasOwnerPrefab;
        [HideInInspector] public GameObject hasNoOwnerPrefab;
        

        protected void SetPrefabDatas()
        {
            hasOwnerPrefab = itemDataSettings.hasOwnerPrefab;
            hasNoOwnerPrefab = itemDataSettings.hasNoOwnerPrefab;
            itemId = itemDataSettings.itemId;
        }

    }
}

