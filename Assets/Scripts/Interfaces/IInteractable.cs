using UnityEngine;

public interface IInteractable
{
    // 상호작용 메서드 (필수)
    void Interact();

    // 상호작용 가능 여부 (선택)
    bool CanInteract() => true;

    // 상호작용 프롬프트 텍스트 (선택)
    string GetInteractionPrompt() => "E를 눌러 상호작용";
}