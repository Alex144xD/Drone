using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GeneticsSaveLoad : MonoBehaviour
{
    string rute;
    string filePath;
    string fileName;
    SaveData SaveP;

    public string jsonFile;

    DateTime date;
    int year;
    int month;
    int day;

    public void Date()
    {
        date = DateTime.Now;
        year = date.Year;
        month = date.Month;
        day = date.Day;
    }

    public void Save(int newEpoch, List<GameObject> Drone)
    {
        if (jsonFile == null)
            CreateFile();
        else
            fileName = $"{jsonFile}.json";

        List<Data> savedDrones = new List<Data>();
        int droneNum = 0;
        float hScore = 0;

        foreach (GameObject _D in Drone)
        {
            IA ia = _D.GetComponent<IA>();
            savedDrones.Add(new Data(droneNum, ia.biases, ia.pesos));
            hScore += ia.score;
            droneNum++;
        }

        float heuristicScore = hScore / Drone.Count;
        SaveP = new SaveData(newEpoch, heuristicScore, savedDrones);

        rute = Application.streamingAssetsPath + "/" + fileName;
        string json = JsonUtility.ToJson(SaveP, true);
        print(json);
        System.IO.File.WriteAllText(rute, json);
    }

    public SaveData Load()
    {
        fileName = $"{jsonFile}.json";
        rute = Application.streamingAssetsPath + "/" + fileName;
        string json = System.IO.File.ReadAllText(rute);
        SaveP = JsonUtility.FromJson<SaveData>(json);
        return SaveP;
    }

    public bool CheckExisting()
    {
        filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        return File.Exists(filePath);
    }

    public void CreateFile()
    {
        int version = 0;
        Date();
        fileName = $"{day}_{month}_{year}_({version}).json";
        bool exist = CheckExisting();

        do
        {
            version++;
            fileName = $"{day}_{month}_{year}_({version}).json";
            exist = CheckExisting();
        }
        while (exist == true);
    }
}

[System.Serializable]
public class SaveData
{
    public int epoch;         // Generación
    public float heuristic;
    public List<Data> drones;

    public SaveData(int _epoch, float _heuristic, List<Data> _drones)
    {
        epoch = _epoch;
        heuristic = _heuristic;
        drones = _drones;
    }
}

[System.Serializable]
public class Data
{
    public int drone;         // Índice del dron
    public Matriz[] biases;  // Biases del dron
    public Matriz[] pesos;   // Pesos del dron

    public Data(int _drone, Matriz[] _biases, Matriz[] _pesos)
    {
        drone = _drone;
        biases = _biases;
        pesos = _pesos;
    }
}
