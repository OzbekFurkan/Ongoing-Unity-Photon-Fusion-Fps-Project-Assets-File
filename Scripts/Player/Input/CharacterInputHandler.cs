using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Network;

namespace Player
{
    public class CharacterInputHandler : MonoBehaviour
    {
        [SerializeField] private InputSettings inputSettings;

        Vector2 moveInputVector = Vector2.zero;
        Vector2 viewInputVector = Vector2.zero;
        bool isJumpButtonPressed = false;
        bool isFireButtonPressed = false;
        bool isDropButtonPressed = false;
        bool isPickUpButtonPressed = false;

        //Other components
        LocalCameraHandler localCameraHandler;

        private void Awake()
        {
            localCameraHandler = GetComponentInChildren<LocalCameraHandler>();
        }

        // Start is called before the first frame update
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (!GetComponent<NetworkObject>().HasInputAuthority)
                return;

            //View input
            viewInputVector.x = Input.GetAxis("Mouse X");
            viewInputVector.y = Input.GetAxis("Mouse Y") * -1; //Invert the mouse look

            //Move input
            moveInputVector.x = Input.GetAxis("Horizontal");
            moveInputVector.y = Input.GetAxis("Vertical");

            //Jump
            if (Input.GetKeyDown(inputSettings.jumpKey))
                isJumpButtonPressed = true;

            //Fire
            if (Input.GetKeyDown(inputSettings.shootKey))
                isFireButtonPressed = true;

            if (Input.GetKeyDown(inputSettings.dropkey))
                isDropButtonPressed = true;

            if (Input.GetKeyDown(inputSettings.pickupKey))
                isPickUpButtonPressed = true;

            //Set view
            localCameraHandler.SetViewInputVector(viewInputVector);

        }

        public NetworkInputData GetNetworkInput()
        {
            NetworkInputData networkInputData = new NetworkInputData();

            //Aim data
            networkInputData.aimForwardVector = localCameraHandler.transform.forward;

            //Move data
            networkInputData.movementInput = moveInputVector;

            //Jump data
            networkInputData.isJumpPressed = isJumpButtonPressed;

            //Fire data
            networkInputData.isFireButtonPressed = isFireButtonPressed;

            networkInputData.isDropButtonPressed = isDropButtonPressed;

            networkInputData.isPickUpButtonPressed = isPickUpButtonPressed;

            //Reset variables now that we have read their states
            isJumpButtonPressed = false;
            isFireButtonPressed = false;
            isDropButtonPressed = false;
            isPickUpButtonPressed = false;

            return networkInputData;
        }
    }
}

