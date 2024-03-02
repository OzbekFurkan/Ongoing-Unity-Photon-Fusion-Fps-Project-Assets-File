using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace Interract
{
    public abstract class InterractListener : NetworkBehaviour
    {
        List<IInterractCallbacks> ListInterractCallbacks = new List<IInterractCallbacks>();

        public void Register(IInterractCallbacks interractCallbacks)
        {
            ListInterractCallbacks.Add(interractCallbacks);
        }
        public void Unregister(IInterractCallbacks interractCallbacks)
        {
            ListInterractCallbacks.Remove(interractCallbacks);
        }
        protected void SendPickupItemCallback<T>(T data) where T:ItemDataMono
        {
            foreach (var observer in ListInterractCallbacks)
            {
                observer.ItemPicked(data.itemId, data);
            }
        }
        protected void SendDropItemCallback<T>(T data) where T : ItemDataMono
        {
            foreach (var observer in ListInterractCallbacks)
            {
                observer.ItemDropped(data.itemId, data);
            }
        }
    }
}

