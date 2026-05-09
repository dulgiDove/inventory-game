using UnityEngine;

public interface IInteractable
{
    void Interact();

    bool CanInteract() => true;

    string GetInteractionPrompt() => "E를 눌러 상호작용";
}