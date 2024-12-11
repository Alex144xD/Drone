using UnityEngine;

public class CambiarCamara : MonoBehaviour
{
    public Camera[] camaras; // Array de cámaras a alternar
    private int camaraActivaIndex = 0; // Índice de la cámara activa

    void Start()
    {
        // Asegurarnos de que solo una cámara esté activa al inicio
        ActivarCamara(camaraActivaIndex);
    }

    void Update()
    {
        // Detectar si se presiona la tecla "C"
        if (Input.GetKeyDown(KeyCode.C))
        {
            CambiarACamaraSiguiente();
        }
    }

    void CambiarACamaraSiguiente()
    {
        // Desactivar la cámara actual
        camaras[camaraActivaIndex].gameObject.SetActive(false);

        // Incrementar el índice de la cámara activa
        camaraActivaIndex = (camaraActivaIndex + 1) % camaras.Length;

        // Activar la siguiente cámara
        ActivarCamara(camaraActivaIndex);
    }

    void ActivarCamara(int index)
    {
        // Asegurarnos de que la cámara especificada esté activa
        camaras[index].gameObject.SetActive(true);
    }
}
