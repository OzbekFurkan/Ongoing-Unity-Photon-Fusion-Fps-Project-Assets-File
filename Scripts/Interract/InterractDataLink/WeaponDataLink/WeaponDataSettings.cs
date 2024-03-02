using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interract
{
    public enum Slot { Pistol, Rifle, Bomb, Other }
    
    [CreateAssetMenu(menuName = "RAW/Interract/Weapon/WeaponDataSettings")]
    public class WeaponDataSettings : ItemDataSettings
    {
        public int ammo;
        public int fullAmmo;
        public Slot slot;
        
    }
}
