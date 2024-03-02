using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interract
{
    public enum ItemId { Glock18, P2000, Ak47, M4A4, Granade, Smoke, MG, Plane, Car };
    public class ItemDataSettings : ScriptableObject
    {
        public GameObject hasOwnerPrefab;
        public GameObject hasNoOwnerPrefab;
        public ItemId itemId;
    }

}
