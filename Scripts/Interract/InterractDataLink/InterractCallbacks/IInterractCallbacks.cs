using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interract
{
    public interface IInterractCallbacks
    {

        public void ItemPicked<T>(ItemId itemId, T data) where T:ItemDataMono;

        public void ItemDropped<T>(ItemId itemId, T data) where T:ItemDataMono;

    }

}
