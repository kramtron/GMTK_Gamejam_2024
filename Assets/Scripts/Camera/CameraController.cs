using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;  // El objetivo que la c�mara seguir�

    [Header("Camera Settings")]
    public Vector3 offset = new Vector3(0, 0, -10f); // Desplazamiento de la c�mara con respecto al objetivo
    public float smoothSpeed = 0.125f; // Velocidad de suavizado para el seguimiento de la c�mara

    void LateUpdate()
    {
        // Si no hay objetivo asignado, salir del m�todo
        if (target == null) return;

        // Posici�n deseada basada en el objetivo y el desplazamiento
        Vector3 desiredPosition = target.position + offset;

        // Suavizar la transici�n hacia la posici�n deseada
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Asignar la nueva posici�n suavizada a la c�mara
        transform.position = smoothedPosition;
    }
}

