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
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();

                if (interactable is { CanInteract: true })
                {
                    _currentInteractable = interactable;
                    return;
                }
            }

            ClearCurrentInteractable();
        }

        private void HandleInteractionInput()
        {
            if (Input.GetKeyDown(KeyCode.E) && _currentInteractable != null)
            {
                _currentInteractable.Interact();
            }
        }

        private void ClearCurrentInteractable()
        {
            _currentInteractable = null;

        }
    }
}