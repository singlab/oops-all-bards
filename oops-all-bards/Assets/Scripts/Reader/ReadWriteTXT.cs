using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadWriteTXT : MonoBehaviour
{
    // Code

    string txt_name = "Data.txt";
    string folder_name = "Reader";

    private string FullPath()
    {
        return $"{Application.dataPath}/Scripts/{folder_name}/{txt_name}";
    }

    Dictionary<string, Person> person_map = new Dictionary<string, Person>();

    private void PersonFromJson(string json)
    {
        Person my_person = new Person();
        my_person = JsonUtility.FromJson<Person>(json);
        person_map.Add(my_person.name, my_person);
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

    public void Reload()
    {
        ReadFile();
    }

    private Person CreatePerson(string name, int level, string school, string[] abilities)
    {
        int count = 0;
        foreach (string person in person_map.Keys)
        {
            string token = person.Split("|")[0];
            if (token == name)
            {
                count++;
            }
        }
        if (count > 0)
        {
            name = $"{name}|{count + 1}";
        }
        Person my_person = new Person();
        my_person.name = name;
        my_person.level = level;
        my_person.school = school;
        my_person.abilities = abilities;
        person_map.Add(my_person.name, my_person);
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
        string text = $"Name: {name}\nLevel: {my_person.level}";
        text += AbilitiesToString(my_person.abilities);
        Debug.Log(text);
    }

    public void TestReading()
    {
        ReadFile();
        foreach (Person my_person in person_map.Values)
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

    public void ChangeAleraLevel()
    {
        int level = 1;
        if (person_map["Alera"].level < 100)
        {
            level = 100;
        }
        EditPlayerInFile(person_map["Alera"], level);
        PrintPerson(person_map["Alera"]);
    }

    public void Reset()
    {
        person_map.Clear();
    }
}
