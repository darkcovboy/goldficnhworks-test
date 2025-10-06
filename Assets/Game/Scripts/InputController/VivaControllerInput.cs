using HTC.UnityPlugin.Vive;
using UnityEngine;

namespace Game.Scripts.InputController
{
    public class VivaControllerInput : MonoBehaviour
    {
        [SerializeField] private HandRole _handRole = HandRole.RightHand;
        [SerializeField] private float _rayLength = 3f;
        [SerializeField] private LayerMask _interactableMask = ~0;

        private IInteractable _currentTarget;

        private void Update()
        {
            HandleRaycast();
            HandleInput();
        }

        private void HandleRaycast()
        {
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, _rayLength, _interactableMask))
            {
                Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    _currentTarget = interactable;
                    return;
                }
            }

            _currentTarget = null;
        }

        private void HandleInput()
        {
            if (ViveInput.GetPress(_handRole, ControllerButton.Trigger))
            {
                if (_currentTarget != null)
                {
                    Debug.Log("Pressed Target");
                }
                _currentTarget?.Interact();
            }

            if (ViveInput.GetPressUp(_handRole, ControllerButton.Trigger))
            {
                _currentTarget?.Release();
            }
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            Vector3 start = transform.position;
            Vector3 end = start + transform.forward * _rayLength;

            Gizmos.DrawLine(start, end);
            Gizmos.DrawSphere(end, 0.01f);
        }
#endif

    }
}