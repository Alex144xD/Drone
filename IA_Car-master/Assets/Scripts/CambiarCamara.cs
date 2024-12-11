using UnityEngine;

public class CambiarCamara : MonoBehaviour
{
    public Camera[] camaras; // Array de c�maras a alternar
    private int camaraActivaIndex = 0; // �ndice de la c�mara activa

    void Start()
    {
        // Asegurarnos de que solo una c�mara est� activa al inicio
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
        // Desactivar la c�mara actual
        camaras[camaraActivaIndex].gameObject.SetActive(false);

        // Incrementar el �ndice de la c�mara activa
        camaraActivaIndex = (camaraActivaIndex + 1) % camaras.Length;

        // Activar la siguiente c�mara
        ActivarCamara(camaraActivaIndex);
    }

    void ActivarCamara(int index)
    {
        // Asegurarnos de que la c�mara especificada est� activa
        camaras[index].gameObject.SetActive(true);
    }
}
