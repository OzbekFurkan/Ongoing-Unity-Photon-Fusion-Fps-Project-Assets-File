using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Network;
using System;
using System.Reflection;

namespace Interract
{
    public class PlayerInterractManager : InterractListener
    {

        List<LagCompensatedHit> detectedInfo = new List<LagCompensatedHit>();

        LayerMask layerMask;

        public override void Spawned()
        {

        }

        public override void FixedUpdateNetwork()
        {

            //Get the input from the network
            if (GetInput(out NetworkInputData networkInputData))
            {
                
                if (networkInputData.isDropButtonPressed)
                {
                    HandleDrop();
                }

                if (networkInputData.isPickUpButtonPressed)
                {
                    HandlePickup();
                }

            }

        }

        private void HandleDrop()
        {
            IDropable droppedChildItem = GetComponentInChildren<IDropable>();
            IDropable droppedParentItem = GetComponentInParent<IDropable>();
            if (droppedParentItem != null)
            {
                ParentDropComponent parentDropComponent = droppedParentItem as ParentDropComponent;
                ItemDataMono itemDataMono = parentDropComponent.gameObject.GetComponent<ItemDataMono>();

                Type type = itemDataMono.GetType();

                MethodInfo methodInfo = typeof(ParentDropComponent).GetMethod("DropItem").MakeGenericMethod(type);

                methodInfo.Invoke(droppedParentItem, null);
            }
            else if (droppedChildItem != null)
            {
                
                ChildDropComponent childDropComponent = droppedChildItem as ChildDropComponent;
                ItemDataMono itemDataMono = childDropComponent.gameObject.GetComponent<ItemDataMono>();

                Type type = itemDataMono.GetType();

                MethodInfo methodInfo = typeof(ChildDropComponent).GetMethod("DropItem").MakeGenericMethod(type);

                object ret = methodInfo.Invoke(droppedChildItem, null);

                dynamic returnResult = ret;

                SendDropItemCallback(returnResult);

            }
        }

        private void HandlePickup()
        {
            //when player is interract with external items like machinegun, he shouldn't be able to pick up anything
            if (GetComponent<PlayerDataMono>().hasInterract)
                return;

            layerMask = LayerMask.GetMask("Pickupable");
            Runner.LagCompensation.OverlapSphere(transform.position, 3, Object.InputAuthority, detectedInfo, layerMask, HitOptions.IncludePhysX);
            if (detectedInfo != null)
            {
                Debug.Log(detectedInfo.Count);
                foreach (var info in detectedInfo)
                {
                    if (info.Collider == null)
                        continue;

                    info.Collider.transform.root.gameObject.TryGetComponent<IPickupable>(out var grabbedItem);
                    Debug.Log("interface alindi " + info.Collider.transform.root.name);
                    if (grabbedItem != null)
                    {
                        Debug.Log("item alindi");
                        ParentPickupComponent parentPickupComponent = grabbedItem as ParentPickupComponent;
                        if(parentPickupComponent!=null)
                        {
                            ItemDataMono itemDataMono = parentPickupComponent.gameObject.GetComponent<ItemDataMono>();
                            Type type = itemDataMono.GetType();
                            MethodInfo methodInfo = typeof(ParentPickupComponent).GetMethod("PickUpItem").MakeGenericMethod(type);
                            methodInfo.Invoke(grabbedItem, new object[] {Object.InputAuthority, gameObject});

                            break;
                        }

                        ChildPickupComponent childPickupComponent = grabbedItem as ChildPickupComponent;
                        if(childPickupComponent!=null)
                        {
                            ItemDataMono itemDataMono = childPickupComponent.gameObject.GetComponent<ItemDataMono>();
                            Type type = itemDataMono.GetType();
                            MethodInfo methodInfo = typeof(ChildPickupComponent).GetMethod("PickUpItem").MakeGenericMethod(type);
                            object ret = methodInfo.Invoke(grabbedItem, new object[] { Object.InputAuthority, gameObject });
                            dynamic returnResult = ret;
                            SendPickupItemCallback(returnResult);

                            break;
                        }
                        break;
                    }
                    Debug.Log("bos gecti");
                }
            }
        }



    }
}

