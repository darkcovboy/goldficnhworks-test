using UnityEngine;

namespace Game.Scripts.Test
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private float _interactionDistance = 3f;
        [SerializeField] private LayerMask _interactionLayerMask = -1;
        private Camera _playerCamera;
        private IInteractable _currentInteractable;

        private void Start()
        {
            _playerCamera = GetComponentInChildren<Camera>();
        }

        private void Update()
        {
            HandleRaycast();
            HandleInteractionInput();
        }

        private void HandleRaycast()
        {
            Ray ray = new Ray(_playerCamera.transform.position, _playerCamera.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, _interactionDistance, _interactionLayerMask))
            {
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    _currentInteractable = interactable;
                    return;
                }
            }

            _currentInteractable.Release();
            _currentInteractable = null;
        }

        private void HandleInteractionInput()
        {
            if (_currentInteractable == null)
                return;

            if (Input.GetKeyDown(KeyCode.E))
            {
                _currentInteractable.Interact();
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                _currentInteractable.Release();
            }
        }
    }
}