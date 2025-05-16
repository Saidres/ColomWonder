using UnityEngine;
using UnityEngine.EventSystems;
using HeneGames.DialogueSystem;

public class CustomButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public PlayerController playerController; // Reference to the PlayerController
    public string action; // Action to perform (e.g., "MoveLeft", "MoveRight", "Jump", "Attack")

    private void OnEnable()
    {
        DialogueManager.OnDialogueStart += DisableButton;
        DialogueManager.OnDialogueEnd += EnableButton;
    }

    private void OnDisable()
    {
        DialogueManager.OnDialogueStart -= DisableButton;
        DialogueManager.OnDialogueEnd -= EnableButton;
    }

    private void DisableButton()
    {
        // Disable the button or its functionality
        GetComponent<CanvasGroup>().alpha = 0.1f; // Make it semi-transparent
        GetComponent<CanvasGroup>().interactable = false;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    private void EnableButton()
    {
        // Enable the button or its functionality
        GetComponent<CanvasGroup>().alpha = 1f; // Make it fully opaque
        GetComponent<CanvasGroup>().interactable = true;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"Pointer Down: {action}");
        if (playerController != null)
        {
            if (action == "MoveLeft")
                playerController.OnMoveLeftButtonDown();
            else if (action == "MoveRight")
                playerController.OnMoveRightButtonDown();
            else if (action == "Jump")
                playerController.OnJumpButtonDown();
            else if (action == "Attack")
                playerController.OnAttackButtonDown();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log($"Pointer Up: {action}");
        if (playerController != null && (action == "MoveLeft" || action == "MoveRight"))
        {
            playerController.OnMoveButtonUp();
        }
    }
}