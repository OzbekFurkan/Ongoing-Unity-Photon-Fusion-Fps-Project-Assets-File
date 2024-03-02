using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    [CreateAssetMenu(menuName = "RAW/Shoot/ShootSettings")]
    public class WeaponShootSettings : ScriptableObject
    {

        [Header("shoot")]
        public LayerMask collisionLayers;

    }
}

