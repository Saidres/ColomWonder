using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraController : MonoBehaviour
{
    public Transform objetivo; // The target (e.g., player)
    public float velocidadCamara = 0.025f; // Camera smoothing speed
    public Vector3 desplazamiento; // Offset from the target

    [Header("Look Ahead Settings")]
    public float lookAheadDistance = 2f; // Distance to look ahead based on movement
    public float lookAheadSpeed = 0.1f; // Speed of the look-ahead adjustment

    private Vector3 currentLookAhead; // Current look-ahead offset
    private Vector3 lastTargetPosition; // Last position of the target

    private void Start()
    {
        // Initialize the last target position
        lastTargetPosition = objetivo.position;
    }

    private void LateUpdate()
    {
        // Calculate the target's movement direction
        Vector3 targetMovement = objetivo.position - lastTargetPosition;

        // Update the look-ahead offset based on movement direction
        currentLookAhead = Vector3.Lerp(currentLookAhead, targetMovement.normalized * lookAheadDistance, lookAheadSpeed);

        // Calculate the desired camera position with look-ahead
        Vector3 posicionDeseada = objetivo.position + desplazamiento + currentLookAhead;

        // Smoothly interpolate the camera's position
        Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, velocidadCamara);

        // Update the camera's position
        transform.position = posicionSuavizada;

        // Update the last target position
        lastTargetPosition = objetivo.position;
    }
}
