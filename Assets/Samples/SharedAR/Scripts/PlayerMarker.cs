using Niantic.Lightship.AR.Samples;
using System.Diagnostics;
using Unity.Netcode;
using UnityEngine;

namespace Scenes.SharedAR.VpsColocalization
{
    public class PlayerMarker : NetworkBehaviour
    {
        private Transform _arCameraTransform;

        public override void OnNetworkSpawn()
        {
            transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            if (IsOwner)
            {
                if (Camera.main)
                {
                    _arCameraTransform = Camera.main.transform;
                }
            }
            if (IsServer)
            {
                // TODO: add some server specific logic?
            }

            base.OnNetworkSpawn();
        }

        public override void OnNetworkDespawn()
        {
            if (OwnerClientId == NetworkManager.ServerClientId)
            {
                UnityEngine.Debug.Log("Host disconnected!!");
                if (VpsColocalizationDemo.Instance)
                {
                    VpsColocalizationDemo.Instance.HandleHostDisconnected();
                }
            }
            base.OnNetworkDespawn();
        }

        void Update()
        {
            if (IsOwner)
            {
                if (_arCameraTransform)
                {
                    // Get local AR camera transform position and rotation
                    Vector3 pos = _arCameraTransform.position; // Correctly access the position
                    Quaternion rot = _arCameraTransform.rotation; // Correctly access the rotation

                    // Update world transform of the object to match the AR Camera's worldTransform
                    transform.SetPositionAndRotation(pos, rot);
                }
            }
        }
    }
}
