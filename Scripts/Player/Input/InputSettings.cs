using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(menuName = "RAW/Input/InputSettings")]
    public class InputSettings : ScriptableObject
    {
        [Header("Input")]
        public KeyCode jumpKey;
        public KeyCode shootKey;
        public KeyCode reloadKey;
        public KeyCode aimKey;
        public KeyCode dropkey;
        public KeyCode pickupKey;
    }
}

