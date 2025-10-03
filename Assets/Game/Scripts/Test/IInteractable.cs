namespace Game.Scripts.Test
{
    public interface IInteractable
    {
        bool CanInteract { get; }
        void Interact();
    }
}