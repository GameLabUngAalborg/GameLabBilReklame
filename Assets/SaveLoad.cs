using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;

public class SaveLoad : MonoBehaviour
{

    private List<ScoreInfo> savedGames = new List<ScoreInfo>();

    [SerializeField]
    private UnityEngine.UI.InputField _name;

    public GameObject InputPanel;
    public List<UnityEngine.UI.Text> ScoreTexts = new List<UnityEngine.UI.Text>();

    public bool LoadOnStart;


    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if(LoadOnStart)
        {
            Load();
        }
    }

    public void SubmitScore()
    {
        //add score yourself instead of 1337
        NewScore(_name.text, 1337);

        for(int i = 0; i < ScoreTexts.Count; i++)
        {
            if (i == savedGames.Count)
            {
                //don't do more
                break;
            }
            else {
                ScoreTexts[i].text = savedGames[i].name + " - " + savedGames[i].points;
            }
        }

        InputPanel.SetActive(false);
        Save();
    }

    void NewScore(string name, float score)
    {
        ScoreInfo i = new ScoreInfo();
        i.name = name;
        i.points = score;

        savedGames.Add(i);
        savedGames.Sort(SortByScore);
    }

    //it's static so we can call it from anywhere
    public  void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        //Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
        print("Saved file to " + Application.persistentDataPath + "/savedGames.gd");
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd"); //you can call it anything you want
        bf.Serialize(file, savedGames);
        file.Close();
    }

    public  void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            print("Loaded file from " + Application.persistentDataPath + "/savedGames.gd");
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            savedGames = (List<ScoreInfo>)bf.Deserialize(file);
            file.Close();
        }
    }

    private static int SortByScore(ScoreInfo o1, ScoreInfo o2)
    {
        // variable.sort(sortbyscore);
        return o1.points.CompareTo(o2.points);
    }
}

[System.Serializable]
public class ScoreInfo
{
    public string name;
    public float points;

}