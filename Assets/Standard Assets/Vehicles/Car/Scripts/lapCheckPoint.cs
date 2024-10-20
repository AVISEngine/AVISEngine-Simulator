using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;

public class lapCheckPoint : MonoBehaviour
{
    public int Index;
    public LanguageHandler languageHandler;
    private SQLiteConnection _connection;

    void Start()
    {
        GameObject gameObject = new GameObject("LanguageHandler");
        languageHandler = gameObject.AddComponent<LanguageHandler>();
        languageHandler.m_dictionary();
        kartLap car = GameObject.Find("Car Urban Tesla").GetComponent<kartLap>();

        car.scoresText.text = string.Format(languageHandler.dict["LapsAndCheckpoints"], car.lapNumber.ToString(), car.checkpointIndex.ToString(), car.totalCheckpoints.ToString());
        InitializeDatabase();
    }

    void InitializeDatabase()
    {
        string DatabaseName = "Scores.db";
        string filepath = Application.persistentDataPath + "/" + DatabaseName;
        _connection = new SQLiteConnection(filepath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        Debug.Log("Database initialized at: " + filepath);

        _connection.CreateTable<TeamScore>();
    }

    private void OnTriggerEnter(Collider other)
    {
        kartLap car = other.GetComponent<kartLap>();
        if (car != null)
        {
            bool checkpointPassed = false;

            if ((car.checkpointIndex == Index + 1 || car.checkpointIndex == Index - 1) && car.trueCheckpoint != Index && Index > car.trueCheckpoint)
            {
                car.checkpointIndex = Index;
                car.trueCheckpoint = Index;
                checkpointPassed = true;
            }
            else if (car.trueCheckpoint != Index && Index > car.trueCheckpoint)
            {
                car.checkpointIndex = car.checkpointIndex + 1;
                car.trueCheckpoint = Index;
                checkpointPassed = true;
            }

            if (checkpointPassed)
            {
                // Save checkpoint data
                SaveCheckpointData(car);
                
                // Update the UI
                UpdateScoresText(car);
            }

            // Debug logging
            Debug.Log($"Checkpoint {Index} triggered. Car checkpoint: {car.checkpointIndex}, True checkpoint: {car.trueCheckpoint}");
        }
    }

    void SaveCheckpointData(kartLap car)
    {
        float checkpointTime = Time.time - car.lastCheckpointTime;
        car.lastCheckpointTime = Time.time;
        
        var teamScore = new TeamScore
        {
            TeamName = car.teamName,
            StageName = car.stageName,
            LapNumber = car.lapNumber,
            CheckpointIndex = car.trueCheckpoint,
            CheckpointTime = checkpointTime,
            Timestamp = DateTime.Now
        };
        Debug.Log("Saving checkpoint data: " + teamScore.ToString());   
        _connection.Insert(teamScore);
    }

    private void UpdateScoresText(kartLap car)
    {
        if (car.scoresText != null)
        {
            car.scoresText.text = string.Format(languageHandler.dict["LapsAndCheckpoints"], car.lapNumber.ToString(), car.checkpointIndex.ToString(), car.totalCheckpoints.ToString());
        }
        else
        {
            Debug.LogWarning("scoresText is null on the kartLap component.");
        }
    }

    void OnDestroy()
    {
        if (_connection != null)
        {
            _connection.Close();
        }
    }
}

// Make sure to include this class definition in your project
public class TeamScore
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string TeamName { get; set; }
    public string StageName { get; set; }
    public int LapNumber { get; set; }
    public int CheckpointIndex { get; set; }
    public float CheckpointTime { get; set; }
    public DateTime Timestamp { get; set; }

    public override string ToString()
    {
        return $"Id: {Id}, TeamName: {TeamName}, StageName: {StageName}, LapNumber: {LapNumber}, CheckpointIndex: {CheckpointIndex}, CheckpointTime: {CheckpointTime}, Timestamp: {Timestamp}";
    }
}
