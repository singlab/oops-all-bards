using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadWriteTXT : MonoBehaviour
{
    // Code

    int biggest_id = -1;
    string txt_name = "Data.txt";
    string folder_name = "JSON";

    List<Person> person_list = new List<Person>();

    private string FullPath()
    {
        return $"{Application.dataPath}/Scripts/{folder_name}/{txt_name}";
    }

    private void PersonFromJson(string json)
    {
        Person my_person = new Person();
        my_person = JsonUtility.FromJson<Person>(json);
        person_list.Add(my_person);
        biggest_id = my_person.id;
    }

    public void ReadFile()
    {
        if(!File.Exists(FullPath()))
        {
            AddToFile("");
            Debug.LogError($"No {txt_name} was found! Creating one now...");
            return;
        }
        StreamReader reader = new StreamReader(FullPath());
        if (reader.Peek() == -1)
        {
            Debug.LogError($"{txt_name} is empty...");
            reader.Close();
            return;
        }
        while (true)
        {
            string line = reader.ReadLine();
            if (line == null)
            {
                break;
            }
            else if (line == "")
            {
                continue;
            }
            try
            {
                PersonFromJson(line);
            }
            catch (Exception)
            {
                Debug.LogError($"Could not convert {line} into a character!");
                break;
            }
        }
        reader.Close();
    }

    private void AddToFile(string text)
    {
        StreamWriter writer = new StreamWriter(FullPath(), true);
        writer.Write(text);
        writer.Close();
    }

    private void AddPersonToFile(Person my_person)
    {
        string json = JsonUtility.ToJson(my_person);
        AddToFile(json + "\n");
    }

    // Testing

    public void Reset()
    {
        biggest_id = -1;
        person_list.Clear();
    }

    public void Reload()
    {
        ReadFile();
    }

    private Person CreatePerson(string name, int level, string school, string[] abilities)
    {
        biggest_id += 1;
        Person my_person = new Person();
        my_person.id = biggest_id;
        my_person.name = name;
        my_person.level = level;
        my_person.school = school;
        my_person.abilities = abilities;
        person_list.Add(my_person);
        return my_person;
    }

    private string AbilitiesToString(string[] abilities)
    {
        int length = abilities.Length;
        if (length == 0)
        {
            return "";
        }
        string text = "\n";
        for (int i = 0; i < length; i++)
        {
            string ability = abilities[i];
            text += $"* {ability}";
            if (i != length - 1)
            {
                text += "\n";
            }
        }
        return text;
    }

    private void PrintPerson(Person my_person)
    {
        string name = my_person.name.Split("|")[0];
        string text = $"Name: {name}\nID: {my_person.id}\nLevel: {my_person.level}";
        text += AbilitiesToString(my_person.abilities);
        Debug.Log(text);
    }

    public void TestReading()
    {
        ReadFile();
        foreach (Person my_person in person_list)
        {
            PrintPerson(my_person);
        }
    }

    public void TestWriting()
    {
        string[] names = {"Alera", "Runt", "Mudd"};
        string[] abilities = {"Placeholder 1", "Placeholder 2", "Placeholder 3"};
        foreach (string name in names)
        {
            Person my_person = CreatePerson(name, 1, "N/A", abilities);
            PrintPerson(my_person);
            AddPersonToFile(my_person);
        }
    }

    private void EditPlayerInFile(Person my_person, int level)
    {
        int count = 0;
        string json = JsonUtility.ToJson(my_person);
        StreamReader reader = new StreamReader(FullPath());
        while (true)
        {
            string line = reader.ReadLine();
            if (line == null || json == line)
            {
                break;
            }
            count++;
        }
        reader.Close();
        my_person.level = level;
        string[] lines = File.ReadAllLines(FullPath());
        lines[count] = JsonUtility.ToJson(my_person);
        File.WriteAllLines(FullPath(), lines);
    }

    public void ChangeMuddLevel()
    {
        int id = 2;
        foreach (Person my_person in person_list)
        {
            if (my_person.id == id && my_person.level < 100)
            {
                EditPlayerInFile(my_person, 100);
                PrintPerson(my_person);
                break;
            }
        }
    }
}
