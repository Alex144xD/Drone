using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IA : MonoBehaviour
{
    public Text scoreText;
    public int capas = 2;
    public int neuronas = 10;
    public Matriz[] pesos;
    public Matriz[] biases;
    Matriz entradas;
    float forwardMovement;
    float verticalMovement;
    float rotation;
    public float score;
    bool lost = false;
    
   public bool Getlost()
   {
        return lost;
   }

    // ForFitness
    private Vector3 lastPosition;
    private float distanceTraveled = 0;
    float forwardMovementSum = 0;
    int forwardMovementCount = 0;

    // Physics
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody is missing on the object!");
            return;
        }

        Initialize();
        lastPosition = transform.position;
    }

    public void Initialize()
    {
        pesos = new Matriz[capas];
        biases = new Matriz[capas];
        entradas = new Matriz(1, 5); // 5 entradas: Forward, Right, Left, Up, Down

        for (int i = 0; i < capas; i++)
        {
            if (i == 0)
            {
                pesos[i] = new Matriz(5, neuronas); // Ajuste de entradas a 5
                pesos[i].RandomInitialize();
                biases[i] = new Matriz(1, neuronas);
                biases[i].RandomInitialize();
            }
            else if (i == capas - 1)
            {
                pesos[i] = new Matriz(neuronas, 4); // Salida: rotation, forwardMovement, verticalMovement, verticalDirection
                pesos[i].RandomInitialize();
                biases[i] = new Matriz(1, 4);
                biases[i].RandomInitialize();
            }
            else
            {
                pesos[i] = new Matriz(neuronas, neuronas);
                pesos[i].RandomInitialize();
                biases[i] = new Matriz(1, neuronas);
                biases[i].RandomInitialize();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!lost)
        {
            // Obtenemos las distancias de los rayos del script Airplane
            float FD = GetComponent<Airplane>().ForwardDistance;
            float RD = GetComponent<Airplane>().RightDistance;
            float LD = GetComponent<Airplane>().LeftDistance;
            float UD = GetComponent<Airplane>().UpDistance;
            float DD = GetComponent<Airplane>().DownDistance;

            // Establecer las entradas en la matriz
            entradas.SetAt(0, 0, FD);
            entradas.SetAt(0, 1, RD);
            entradas.SetAt(0, 2, LD);
            entradas.SetAt(0, 3, UD);
            entradas.SetAt(0, 4, DD);

            // Resolver las salidas de la red
            resolve();
           
            
            // Mover el avión según las salidas de la red
            rb.AddForce(transform.forward * forwardMovement, ForceMode.Acceleration);
            rb.AddForce(transform.up * verticalMovement, ForceMode.Acceleration);

            transform.Rotate(Vector3.up, (rotation * 90) * Time.deltaTime);

            // Actualizar métricas para la función de fitness
            distanceTraveled += Vector3.Distance(transform.position, lastPosition);
            lastPosition = transform.position;
            forwardMovementSum += forwardMovement;
            forwardMovementCount++;
            SetScore();


        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    void resolve()
    {
        Matriz result;
        result = Activation((entradas * pesos[0]) + biases[0]);
        for (int i = 1; i < capas; i++)
        {
            result = Activation((result * pesos[i]) + biases[i]);
        }
        ActivationLast(result);
    }

    Matriz Activation(Matriz m)
    {
        for (int i = 0; i < m.rows; i++)
        {
            for (int j = 0; j < m.columns; j++)
            {
                m.SetAt(i, j, (float)MathL.HyperbolicTangtent(m.GetAt(i, j)));
            }
        }
        return m;
    }

    void ActivationLast(Matriz m)
    {
        rotation = (float)MathL.HyperbolicTangtent(m.GetAt(0, 0)); // Rotación
        forwardMovement = MathL.Sigmoid(m.GetAt(0, 1)); // Movimiento hacia adelante
        float verticalDirection = (MathL.Sigmoid(m.GetAt(0, 3)) * 2 - 1); // Dirección vertical (-1 a 1)
        verticalMovement = verticalDirection * MathL.Sigmoid(m.GetAt(0, 2)); // Movimiento vertical modulado
    }

    void SetScore() // FitnessFunction
    {
        float FD = GetComponent<Airplane>().ForwardDistance;
        float RD = GetComponent<Airplane>().RightDistance;
        float LD = GetComponent<Airplane>().LeftDistance;
        float UD = GetComponent<Airplane>().UpDistance;
        float DD = GetComponent<Airplane>().DownDistance;
        float s = (FD + RD + LD + UD + DD) / 5;
        s += ((distanceTraveled * 8) + (forwardMovement));
        score += (float)Math.Pow(s, 2);
    }
  
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Wall")
        {
            lost = true;
            Genetics.PlaneAlive--; // Cambiar si "carsAlive" también necesita adaptarse
        }
     
    }
    public void UpdateDistances(float forward, float left, float right, float up, float down)
    {
        // Actualizar lógicas específicas basadas en las distancias recibidas
        Debug.Log($"Distancias actualizadas: Forward={forward}, Left={left}, Right={right}, Up={up}, Down={down}");
    }


}
