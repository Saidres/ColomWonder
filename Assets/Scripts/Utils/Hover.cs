using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Hover : MonoBehaviour
{
    public float hoverHeight = 0.5f; // Height of the hover effect
    public float hoverDuration = 1.0f; // Duration of the hover effect
    public float hoverDelay = 0.5f; // Delay before starting the hover effect

    private Vector3 originalLocalPosition; // Store the local position instead of world position

    void Start()
    {
        originalLocalPosition = transform.localPosition; // Use local position
        StartHover();
    }

    void StartHover()
    {
        transform.DOLocalMoveY(originalLocalPosition.y + hoverHeight, hoverDuration) // Use DOLocalMoveY for local position
            .SetEase(Ease.InOutSine)
            .SetDelay(hoverDelay)
            .SetLoops(-1, LoopType.Yoyo);
    }
}

