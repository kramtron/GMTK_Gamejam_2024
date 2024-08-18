using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;  // El objetivo que la cámara seguirá

    [Header("Camera Settings")]
    public Vector3 offset = new Vector3(0, 0, -10f); // Desplazamiento de la cámara con respecto al objetivo
    public float smoothSpeed = 0.125f; // Velocidad de suavizado para el seguimiento de la cámara

    void LateUpdate()
    {
        // Si no hay objetivo asignado, salir del método
        if (target == null) return;

        // Posición deseada basada en el objetivo y el desplazamiento
        Vector3 desiredPosition = target.position + offset;

        // Suavizar la transición hacia la posición deseada
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Asignar la nueva posición suavizada a la cámara
        transform.position = smoothedPosition;
    }
}

