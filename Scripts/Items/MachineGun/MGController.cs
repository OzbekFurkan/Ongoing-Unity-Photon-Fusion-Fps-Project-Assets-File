using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Network;

namespace Item
{
    public class MGController : NetworkBehaviour
    {

        public override void FixedUpdateNetwork()
        {

            //Get the input from the network
            if (GetInput(out NetworkInputData networkInputData))
            {
                //Rotate the transform according to the client aim vector
                transform.forward = networkInputData.aimForwardVector;
            }

        }

    }
}
