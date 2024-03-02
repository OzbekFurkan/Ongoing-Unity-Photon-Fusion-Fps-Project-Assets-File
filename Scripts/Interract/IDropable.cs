using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Interract
{
    public interface IDropable
    {
        public TDataMono DropItem<TDataMono>() where TDataMono : ItemDataMono, ICloneable;
    }
}
