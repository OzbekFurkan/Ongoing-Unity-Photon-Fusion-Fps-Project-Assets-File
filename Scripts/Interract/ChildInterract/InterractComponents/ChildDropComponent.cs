using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;

namespace Interract
{
    public class ChildDropComponent : NetworkBehaviour, IDropable
    {
        private GameObject hasNoOwnerWeapon;
        private Rigidbody cloneItemRigidbody;

        

        public TDataMono DropItem<TDataMono>() where TDataMono:ItemDataMono, ICloneable
        {
            if (!Object.HasStateAuthority)
                return null;

            hasNoOwnerWeapon = GetComponent<TDataMono>().hasNoOwnerPrefab;
            var spawnedCloneChild = Runner.Spawn(hasNoOwnerWeapon, transform.position, transform.rotation, PlayerRef.None);
            TDataMono originalChildData = GetComponent<TDataMono>();
            TDataMono cloneChildData = spawnedCloneChild.gameObject.GetComponent<TDataMono>();
            //data clonu yapıp refereansı verdik ama bu clone metodundan dönen obje artık component değil. Olmadı yani.
            cloneChildData = originalChildData.Clone() as TDataMono;
            
            ThrowCloneWeapon(spawnedCloneChild.gameObject);
            Runner.Despawn(Object);

            return originalChildData;
        }

        private void ThrowCloneWeapon(GameObject spawnedCloneChild)
        {
            cloneItemRigidbody = spawnedCloneChild.GetComponent<Rigidbody>();
            cloneItemRigidbody.AddForce(spawnedCloneChild.transform.forward, ForceMode.Impulse);
        }

    }
}

