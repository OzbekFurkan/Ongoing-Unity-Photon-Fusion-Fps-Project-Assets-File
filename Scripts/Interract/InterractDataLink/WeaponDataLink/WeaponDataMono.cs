using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;

namespace Interract
{
    [Serializable]
    public class WeaponDataMono : ItemDataMono, ICloneable
    {
        [SerializeField] WeaponDataSettings weaponDataSettings;

        private ChangeDetector changeDetector;

        [Networked]
        [HideInInspector] public int ammo { get; set; }

        [HideInInspector] public int fullAmmo;
        [HideInInspector] public Slot slot;
        
        


        public override void Spawned()
        {
            changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
            SetPrefabDatas();
            ammo = weaponDataSettings.ammo;
            fullAmmo = weaponDataSettings.fullAmmo;
            slot = weaponDataSettings.slot;
            
        }

        public override void Render()
        {
            foreach (var change in changeDetector.DetectChanges(this, out var previous, out var current))
            {
                switch (change)
                {
                    case nameof(ammo):
                        var reader = GetPropertyReader<int>(nameof(ammo));
                        int receivedAmmo = reader.Read(current);
                        OnAmmoChanged(receivedAmmo);
                        break;
                }
            }
        }

        public void OnAmmoChanged(int newAmmo)
        {
            ammo = newAmmo;
        }

        public object Clone()
        {
            WeaponDataMono weaponDataMono = new WeaponDataMono();
            weaponDataMono.ammo = this.ammo;
            weaponDataMono.fullAmmo = this.ammo;
            return weaponDataMono;
        }
    }
}

