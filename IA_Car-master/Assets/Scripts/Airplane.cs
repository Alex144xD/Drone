using UnityEngine;

public class Airplane : MonoBehaviour
{
    Vector3 pos;
    Vector3 forward;
    Vector3 left;
    Vector3 right;
    Vector3 up;
    Vector3 down;

    public float ForwardDistance = 0;
    public float LeftDistance = 0;
    public float RightDistance = 0;
    public float UpDistance = 0;
    public float DownDistance = 0;

    public float RayLength = 50f; // Longitud de los rayos para el avión

    private IA iaScript; // Referencia al script IA

    private void Start()
    {
        // Buscar el script IA en el mismo GameObject
        iaScript = GetComponent<IA>();
        if (iaScript == null)
        {
            Debug.LogError("El script IA no está presente en el objeto.");
        }
    }

    private void Update()
    {
        // Direcciones relativas al avión
        forward = transform.TransformDirection(Vector3.forward) * RayLength;
        left = transform.TransformDirection(Vector3.left) * RayLength;
        right = transform.TransformDirection(Vector3.right) * RayLength;
        up = transform.TransformDirection(Vector3.up) * RayLength;
        down = transform.TransformDirection(Vector3.down) * RayLength;

        // Ajustar la posición de origen del rayo ligeramente encima del avión
        pos = transform.position + new Vector3(0, 2, 0);
        if (iaScript.Getlost() == false)
        {
            // Dibujar rayos para visualizar en la escena
            Debug.DrawRay(pos, forward, Color.red);
            Debug.DrawRay(pos, left, Color.blue);
            Debug.DrawRay(pos, right, Color.green);
            Debug.DrawRay(pos, up, Color.yellow);
            Debug.DrawRay(pos, down, Color.magenta);
        }
       
    }

    private void FixedUpdate()
    {
        RaycastHit hit;

        // Restablecer las distancias a un valor máximo
        ForwardDistance = LeftDistance = RightDistance = UpDistance = DownDistance = 1;

        // Realizar los raycasts en cada dirección
        if (Physics.Raycast(pos, forward, out hit, RayLength))
        {
            ForwardDistance = hit.distance / RayLength;
        }
        if (Physics.Raycast(pos, left, out hit, RayLength))
        {
            LeftDistance = hit.distance / RayLength;
        }
        if (Physics.Raycast(pos, right, out hit, RayLength))
        {
            RightDistance = hit.distance / RayLength;
        }
        if (Physics.Raycast(pos, up, out hit, RayLength))
        {
            UpDistance = hit.distance / RayLength;
        }
        if (Physics.Raycast(pos, down, out hit, RayLength))
        {
            DownDistance = hit.distance / RayLength;
        }

       
    }
}
