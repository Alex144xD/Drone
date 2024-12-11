using UnityEngine;

public class TimeControl : MonoBehaviour
{
    // Factor de aceleración del tiempo
    public float fastForwardSpeed = 2.0f; // Velocidad acelerada (2x)
    private float normalSpeed = 1.0f; // Velocidad normal (1x)
    private bool isFastForwarding = false; // Para controlar el estado

    // Método que alterna la velocidad del tiempo
    public void ToggleTimeSpeed()
    {
        if (isFastForwarding)
        {
            Time.timeScale = normalSpeed; // Volver a la velocidad normal
            isFastForwarding = false;
        }
        else
        {
            Time.timeScale = fastForwardSpeed; // Acelerar el tiempo
            isFastForwarding = true;
        }
    }
}

