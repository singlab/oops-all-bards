using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadWriteCSV : MonoBehaviour
{
    // Variables to be changed
    string csv_name = "Data.csv";
    string menu = "Name,School,Level,Abilities";

    // Character template
    public class Character
    {
        public string name;
        public int level;
        public string school;
        public string[] abilities = {};
    }

    // Character list
    public List<Character> character_list = new List<Character>();

    // Get the full path
    private string FullPath()
    {
        return $"{Application.dataPath}/Scripts/Reader/{csv_name}";
    }

    // Create the CSV file if it does not exist
    private void CreateFile()
    {
        if(!File.Exists(FullPath()))
        {
            StreamWriter writer = new StreamWriter(FullPath(), true);
            writer.Write(menu);
            writer.Close();
        }
    }

    // Add a character to the CSV file
    private void AddToFile(string text)
    {
        CreateFile();
        StreamWriter writer = new StreamWriter(FullPath(), true);
        writer.Write(text);
        writer.Close();
    }

    // Convert a character back into the CSV format
    private string CharacterToString(Character my_character)
    {
        string summary = "\n" + my_character.name + ",";
        summary += my_character.school + ",";
        summary +=  my_character.level.ToString() + ",";
        int inner_length = my_character.abilities.Length;
        for (int i = 0; i < inner_length; i++)
        {
            string ability = my_character.abilities[i];
            summary += ability;
            if (i != inner_length - 1)
            {
                summary += "|";
            }
        }
        return summary;
    }

    // Re-write the CSV file
    public void ReWriteFile()
    {
        File.Delete(FullPath());
        string summary = "";
        int length = character_list.Count;
        for (int i = 0; i < length; i++)
        {
            var my_character = character_list[i];
            summary += CharacterToString(my_character);
        }
        AddToFile(summary);
    }

    // Read the CSV file
    public void ReadFile()
    {
        CreateFile();
        // Open the CSV file for reading
        StreamReader reader = new StreamReader(FullPath());
        if (reader.Peek() == -1)
        {
            Debug.LogError("FileIsEmpty...");
            return;
        }
        // Convert the strings into classes
        bool first = true;
        string[] column_names = menu.Split(",");
        int length = column_names.Length;
        while (true)
        {
            // Break out at the end
            string line = reader.ReadLine();
            if (line == null)
            {
                break;
            }
            // Skip if empy string
            if (line == "")
            {
                ReWriteFile();
                continue;
            }
            // Skip the first line
            if (first)
            {
                first = false;
                continue;
            }
            // Read the CSV file line by line
            string[] row_data = line.Split(",");
            {
                // Create the characters
                Character my_character = new Character();
                for (int i = 0; i < length; i++)
                {
                    string element = row_data[i];
                    string column_name = column_names[i];
                    string lower = column_name.ToLower();
                    if (lower == "name")
                    {
                        my_character.name = element;
                    }
                    else if (lower == "school")
                    {
                        my_character.school = element;
                    }
                    else if (lower == "level")
                    {
                        my_character.level = System.Convert.ToInt32(element);
                    }
                    else if (lower == "abilities" && element != "") {
                        my_character.abilities = element.Split("|");
                    }
                }
                character_list.Add(my_character);
            }
        }
        reader.Close();
    }

    // Add the example character to the CSV file
    public void AddExample()
    {
        Character my_character = new Character();
        my_character.name = "Example";
        my_character.school = "None";
        my_character.level = 0;
        character_list.Add(my_character);
        AddToFile(CharacterToString(my_character));
    }

    // Change Alera's level to 100
    public void ChangeLevel()
    {
        foreach (Character my_character in character_list)
        {
            if (my_character.name == "Alera")
            {
                my_character.level = 100;
                break;
            }
        }
        ReWriteFile();
    }

    // Print out the characters' information
    public void Printer()
    {
        foreach (Character my_character in character_list)
        {
            string summary = "\n Name: " + my_character.name;
            summary += "\n School: " + my_character.school;
            summary += "\n Level: " + my_character.level.ToString();
            if (my_character.abilities.Length > 0)
            {
                summary += "\n Abilities: (below)";
                foreach (string ability in my_character.abilities)
                {
                    summary += "\n * " + ability;
                }
            }
            Debug.Log(summary);
        }
    }

    // Reset the character list
    public void Reset()
    {
        character_list.Clear();
    }
}
