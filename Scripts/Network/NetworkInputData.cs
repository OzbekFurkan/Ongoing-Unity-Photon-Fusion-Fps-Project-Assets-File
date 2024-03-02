using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace Network
{
    public struct NetworkInputData : INetworkInput
    {
        public Vector2 movementInput;
        public Vector3 aimForwardVector;
        public NetworkBool isJumpPressed;
        public NetworkBool isFireButtonPressed;
        public NetworkBool isDropButtonPressed;
        public NetworkBool isPickUpButtonPressed;
    }
}
