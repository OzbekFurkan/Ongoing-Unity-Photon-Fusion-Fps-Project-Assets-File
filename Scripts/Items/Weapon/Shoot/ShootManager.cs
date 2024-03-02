using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Player;
using Interract;

namespace Item
{
    public class ShootManager : NetworkBehaviour
    {
        ChangeDetector changeDetector;

        [Networked]
        public bool isFiring { get; set; }

        [SerializeField] private WeaponShootSettings weaponShootSettings;
        [SerializeField] private WeaponDataMono weaponDataMono;
        public Transform aimPoint;
        public ParticleSystem fireParticleSystem;

        float lastTimeFired = 0;

        public override void Spawned()
        {
            changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
        }

        public override void Render()
        {
            foreach (var change in changeDetector.DetectChanges(this, out var prev, out var current))
            {
                switch (change)
                {
                    case nameof(isFiring):
                        var fireReader = GetPropertyReader<bool>(nameof(isFiring));
                        var (isFiringOld, isFiringCurrent) = fireReader.Read(prev, current);
                        OnFireChanged(isFiringOld, isFiringCurrent);
                        break;
                }
            }
        }


        public void Fire(Vector3 aimForwardVector)
        {
            //Limit fire rate
            if (Time.time - lastTimeFired < 0.15f)
                return;

            StartCoroutine(FireEffectCO());

            Runner.LagCompensation.Raycast(aimPoint.position, aimForwardVector, 100, Object.InputAuthority,
                out var hitinfo, weaponShootSettings.collisionLayers, HitOptions.IncludePhysX);

            float hitDistance = 100;
            bool isHitOtherPlayer = false;

            if (hitinfo.Distance > 0)
                hitDistance = hitinfo.Distance;

            if (hitinfo.Hitbox != null)
            {
                Debug.Log($"{Time.time} {transform.name} hit hitbox {hitinfo.Hitbox.transform.root.name}");

                if (Object.HasStateAuthority)
                    hitinfo.Hitbox.transform.root.GetComponent<HPHandler>().OnTakeDamage();

                isHitOtherPlayer = true;

            }
            else if (hitinfo.Collider != null)
            {
                Debug.Log($"{Time.time} {transform.name} hit PhysX collider {hitinfo.Collider.transform.name}");
            }

            //Debug
            if (isHitOtherPlayer)
                Debug.DrawRay(aimPoint.position, aimForwardVector * hitDistance, Color.red, 1);
            else Debug.DrawRay(aimPoint.position, aimForwardVector * hitDistance, Color.green, 1);

            lastTimeFired = Time.time;
        }

        IEnumerator FireEffectCO()
        {
            isFiring = true;
            weaponDataMono.ammo--;
            fireParticleSystem.Play();

            yield return new WaitForSeconds(0.09f);

            isFiring = false;
        }


        public void OnFireChanged(bool isFiringOld, bool isFiringCurrent)
        {
            //Debug.Log($"{Time.time} OnFireChanged value {changed.Behaviour.isFiring}");

            if (isFiringCurrent && !isFiringOld)
                OnFireRemote();

        }

        void OnFireRemote()
        {
            if (!Object.HasInputAuthority)
                fireParticleSystem.Play();
        }

    }
}

