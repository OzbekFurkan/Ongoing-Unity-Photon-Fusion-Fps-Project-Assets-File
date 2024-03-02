using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;

namespace Interract
{
    public interface IPickupable
    {
        public TDataMono PickUpItem<TDataMono>(PlayerRef newOwner, GameObject parentPlayerObject) where TDataMono : ItemDataMono, ICloneable;
    }
}
