using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Genetics : MonoBehaviour
{
    public Text EpochsText;
    public int epochs = 0;
    public GameObject prefab;
    public static int PlaneAlive;

    public int poblacion = 30;
    public float probDeMutacion = .05f;

    public int mejoresCromosomas = 7;
    public int peoresCromosomas = 1;
    public int cromosomasParaMutar = 20;
    public int mutacionesporCromosoma = 5;

    private List<GameObject> Drone;
    private List<GameObject> newerDrone;

    public GeneticsSaveLoad saveLoad;

    // Start is called before the first frame update
    void Start()
    {
        PlaneAlive = poblacion;
        Drone = new List<GameObject>();
        newerDrone = new List<GameObject>();
        for (int i = 0; i < poblacion; i++)
        {
            GameObject newObject = Instantiate(prefab) as GameObject;
            Drone.Add(newObject);
        }
        //Load();
    }

    public void Load()
    {
        if (saveLoad.jsonFile == null)
        {
            for (int i = 0; i < poblacion; i++)
            {
                GameObject newObject = Instantiate(prefab) as GameObject;
                Drone.Add(newObject);
            }
        }
        else
        {
            List<GameObject> DroneNew = new List<GameObject>();
            SaveData Data = saveLoad.Load();
            for (int i = 0; i < poblacion; i++)
            {
                GameObject newObject = Instantiate(prefab) as GameObject;

                if (Data.drones[i] != null)
                {
                    newObject.GetComponent<IA>().Initialize();
                    newObject.GetComponent<IA>().biases = Data.drones[i].biases;
                    newObject.GetComponent<IA>().pesos = Data.drones[i].pesos;
                }
                Drone.Add(newObject);
            }

            epochs = Data.epoch;
        }
    }

    // Update is called once per frame
    void Update()
    {
        EpochsText.text = "GENERATION: " + epochs.ToString();
        if (PlaneAlive <= 0 || Input.GetKeyDown("space"))
        {
            saveLoad.Save(epochs, Drone);
            NextEpoch();
            DeleteDrone();
            PlaneAlive = poblacion;
            epochs++;
        }
    }

    void DeleteDrone()
    {
        for (int i = 0; i < Drone.Count; i++)
        {
            Destroy(Drone[i]);
        }
        Drone.Clear();
        Drone = newerDrone;
    }

    void NextEpoch()
    {
        Drone.Sort((x, y) => y.GetComponent<IA>().score.CompareTo(x.GetComponent<IA>().score)); // Ordenar de mayor a menor
        List<GameObject> DroneNew = new List<GameObject>();

        // Copiar los mejores cromosomas
        for (int i = 0; i < mejoresCromosomas; i++)
        {
            DroneNew.Add(Copy(Drone[Drone.Count - 1 - i]));
        }

        // Copiar los peores cromosomas (si es necesario)
        for (int i = 0; i < peoresCromosomas; i++)
        {
            DroneNew.Add(Copy(Drone[i]));
        }

        int k = mejoresCromosomas + peoresCromosomas;

        // Crear nuevos cromosomas por cruce
        while (k < poblacion)
        {
            int n1 = UnityEngine.Random.Range(0, DroneNew.Count); // Asegúrate de que el rango es válido
            int n2 = UnityEngine.Random.Range(0, DroneNew.Count); // Asegúrate de que el rango es válido
            DroneNew.Add(Cross(DroneNew[n1], DroneNew[n2]));
            k++;
        }

        // Mutar algunos cromosomas
        for (int i = 0; i < cromosomasParaMutar; i++)
        {
            int n1 = UnityEngine.Random.Range(0, DroneNew.Count);
            IA iaN = DroneNew[n1].GetComponent<IA>();

            // Mutar los bias
            for (int j = 0; j < iaN.biases.Length; j++)
            {
                iaN.biases[j].Mutate(mutacionesporCromosoma);
            }

            // Mutar los pesos
            for (int j = 0; j < iaN.pesos.Length; j++)
            {
                iaN.pesos[j].Mutate(mutacionesporCromosoma);
            }
        }

        newerDrone = DroneNew; // Reemplazar la generación anterior
    }

    GameObject Cross(GameObject g1, GameObject g2)
    {
        GameObject newObject = Instantiate(prefab) as GameObject;
        GameObject r = newObject;
        r.GetComponent<IA>().Initialize();
        IA ia1 = g1.GetComponent<IA>();
        IA ia2 = g2.GetComponent<IA>();

        for (int i = 0; i < ia1.biases.Length; i++)
        {
            r.GetComponent<IA>().biases[i] = Matriz.SinglePointCross(ia1.biases[i], ia2.biases[i]);
        }

        for (int i = 0; i < ia1.pesos.Length; i++)
        {
            r.GetComponent<IA>().pesos[i] = Matriz.SinglePointCross(ia1.pesos[i], ia2.pesos[i]);
        }
        return r;
    }

    GameObject Copy(GameObject c)
    {
        GameObject newObject = Instantiate(prefab) as GameObject;
        GameObject r = newObject;
        r.GetComponent<IA>().Initialize();
        IA ia1 = c.GetComponent<IA>();

        for (int i = 0; i < ia1.biases.Length; i++)
        {
            r.GetComponent<IA>().biases[i] = ia1.biases[i];
        }

        for (int i = 0; i < ia1.pesos.Length; i++)
        {
            r.GetComponent<IA>().pesos[i] = ia1.pesos[i];
        }
        return r;
    }
}
