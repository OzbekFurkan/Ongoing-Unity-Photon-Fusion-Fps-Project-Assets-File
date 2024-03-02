using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;
using Interract;

namespace Player
{
    public class HPHandler : NetworkBehaviour
    {
        ChangeDetector changeDetector;
        [SerializeField] PlayerDataMono playerDataMono;

        [Networked]
        byte HP { get; set; }

        byte startingHP = 5;

        [Networked]
        public bool isDead { get; set; }

        bool isInitialized = false;

        public Color uiOnHitColor;
        public Image uiOnHitImage;

        public MeshRenderer bodyMeshRenderer;
        Color defaultMeshBodyColor;

        public GameObject playerModel;
        public GameObject deathGameObjectPrefab;

        //Other components
        HitboxRoot hitboxRoot;
        CharacterMovementHandler characterMovementHandler;

        private void Awake()
        {
            characterMovementHandler = GetComponent<CharacterMovementHandler>();
            hitboxRoot = GetComponentInChildren<HitboxRoot>();
        }

        // Start is called before the first frame update
        void Start()
        {
            isDead = false;

            defaultMeshBodyColor = bodyMeshRenderer.material.color;

            isInitialized = true;
        }
        public override void Spawned()
        {
            changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
            HP = playerDataMono.HP;
        }

        public override void Render()
        {
            foreach (var change in changeDetector.DetectChanges(this, out var prev, out var current))
            {
                switch (change)
                {
                    case nameof(HP):
                        var hpReader = GetPropertyReader<byte>(nameof(HP));
                        var (oldHP, newHP) = hpReader.Read(prev, current);
                        OnHPChanged(oldHP, newHP);
                        break;

                    case nameof(isDead):
                        var stateReader = GetPropertyReader<bool>(nameof(isDead));
                        var (isDeadOld, isDeadCurrent) = stateReader.Read(prev, current);
                        OnStateChanged(isDeadOld, isDeadCurrent);
                        break;
                }
            }
        }

        //Function only called on the server
        public void OnTakeDamage()
        {
            //Only take damage while alive
            if (isDead)
                return;

            HP -= 1;
            playerDataMono.HP--;

            Debug.Log($"{Time.time} {transform.name} took damage got {HP} left ");

            //Player died
            if (HP <= 0)
            {
                Debug.Log($"{Time.time} {transform.name} died");

                StartCoroutine(ServerReviveCO());

                isDead = true;
            }
        }

        public void OnHPChanged(byte oldHP, byte newHP)
        {
            Debug.Log($"{Time.time} OnHPChanged value {newHP}");
            HP = newHP;
            //Check if the HP has been decreased
            if (newHP < oldHP)
                OnHPReduced();
        }

        private void OnHPReduced()
        {
            if (!isInitialized)
                return;

            StartCoroutine(OnHitCO());
        }

        IEnumerator OnHitCO()
        {
            bodyMeshRenderer.material.color = Color.white;

            if (Object.HasInputAuthority)
                uiOnHitImage.color = uiOnHitColor;

            yield return new WaitForSeconds(0.2f);

            bodyMeshRenderer.material.color = defaultMeshBodyColor;

            if (Object.HasInputAuthority && !isDead)
                uiOnHitImage.color = new Color(0, 0, 0, 0);
        }



        public void OnStateChanged(bool isDeadOld, bool isDeadCurrent)
        {
            Debug.Log($"{Time.time} OnStateChanged isDead {isDeadCurrent}");

            //Handle on death for the player. Also check if the player was dead but is now alive in that case revive the player.
            if (isDeadCurrent)
                OnDeath();
            else if (!isDeadCurrent && isDeadOld)
                OnRevive();
        }

        private void OnDeath()
        {
            Debug.Log($"{Time.time} OnDeath");

            playerModel.gameObject.SetActive(false);
            hitboxRoot.HitboxRootActive = false;
            characterMovementHandler.SetCharacterControllerEnabled(false);

            Instantiate(deathGameObjectPrefab, transform.position, Quaternion.identity);
        }

        private void OnRevive()
        {
            Debug.Log($"{Time.time} OnRevive");

            if (Object.HasInputAuthority)
                uiOnHitImage.color = new Color(0, 0, 0, 0);

            playerModel.gameObject.SetActive(true);
            hitboxRoot.HitboxRootActive = true;
            characterMovementHandler.SetCharacterControllerEnabled(true);
        }

        IEnumerator ServerReviveCO()
        {
            yield return new WaitForSeconds(2.0f);

            characterMovementHandler.RequestRespawn();
        }




        public void OnRespawned()
        {
            //Reset variables
            HP = startingHP;
            isDead = false;
        }

    }
}

