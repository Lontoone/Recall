using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public class Save_LoadSystem : MonoBehaviour {
    public static void SaveSongData(SongClass song) {

        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/songRecord";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
           
        }
        path += "/" + song.SongName + ".rec";
        //string path = Application.persistentDataPath + "/songRecord/" +song.SongName+ ".rec";

        FileStream stream = new FileStream(path,
                         FileMode.Create,
                         FileAccess.Write, FileShare.None);

        SongClass data = new SongClass(song);

        stream.Seek(0, SeekOrigin.Begin);

        formatter.Serialize(stream, data);
        stream.Close();
        stream.Dispose();
        
    }
    
    public static SongClass LoadSongData(string _songName) {
        string path = Application.persistentDataPath + "/songRecord/" + _songName + ".rec";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open,
                          FileAccess.Read,
                          FileShare.Read);
            stream.Position = 0;

            SongClass data = new SongClass();

            stream.Seek(0, SeekOrigin.Begin);

            data = formatter.Deserialize(stream) as SongClass;
            stream.Close();
          

            return data;
        }
        else {
            Debug.Log("Load Error");
            return null;
        }
    }

    //Node :-------------
    public static void SaveNodeData(List<CustumeEditNode.Node> _nodeData, string name)
    {

        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/CustumeEditNode";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);

        }
        path += "/" + name+ ".node";
      
        FileStream stream = new FileStream(path,
                         FileMode.Create,
                         FileAccess.Write, FileShare.None);

        //CustumeEditNode.Node data = new CustumeEditNode.Node();

        stream.Seek(0, SeekOrigin.Begin);

        formatter.Serialize(stream, _nodeData);
        stream.Close();
        stream.Dispose();

    }

    public static List <CustumeEditNode.Node> LoadNodeData(string _songName)
    {
        string path = Application.persistentDataPath + "/CustumeEditNode/" + _songName + ".node";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open,
                          FileAccess.Read,
                          FileShare.Read);
            stream.Position = 0;

            List<CustumeEditNode.Node> data = new List<CustumeEditNode.Node>();

            stream.Seek(0, SeekOrigin.Begin);

            data = formatter.Deserialize(stream) as List<CustumeEditNode.Node>;
            stream.Close();


            return data;
        }
        else
        {
            Debug.Log("<color=red>Load Error</color>");
            return null;
        }
    }

    //--------------------

    //PlayerPerence---------------------
    public static void SavePlayerPreferenceData(GM.PlayerPreference _playerPreference)
    {

        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/PlayerData";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);

        }
        path += "/" + "PlayerPreference" + ".Pre";

        FileStream stream = new FileStream(path,
                         FileMode.Create,
                         FileAccess.Write, FileShare.None);

        stream.Seek(0, SeekOrigin.Begin);

        formatter.Serialize(stream, _playerPreference);
        stream.Close();
        stream.Dispose();

    }
    public static GM.PlayerPreference LoadPlayerPreferenceData()
    {
        string path = Application.persistentDataPath + "/PlayerData/" + "PlayerPreference" + ".Pre";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open,
                          FileAccess.Read,
                          FileShare.Read);
            //stream.Position = 0;

            GM.PlayerPreference data = new GM.PlayerPreference();

            //stream.Seek(0, SeekOrigin.Begin);

            data = formatter.Deserialize(stream) as GM.PlayerPreference;
            stream.Close();
         

            return data;
        }
        else
        {
            Debug.Log("<color=red>Load Error</color>");
            return null;
        }
    }
    //----------------------------------

    //PlayerData------------------------
    public static void SavePlayerData(GM.PlayerData _playerData)
    {

        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/PlayerData";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);

        }
        path += "/" + "PlayerData" + ".data";

        FileStream stream = new FileStream(path,
                         FileMode.Create,
                         FileAccess.Write, FileShare.None);

        stream.Seek(0, SeekOrigin.Begin);

        formatter.Serialize(stream, _playerData);
        stream.Close();
        stream.Dispose();

    }
    public static GM.PlayerData LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/PlayerData/" + "PlayerData" + ".data";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open,
                          FileAccess.Read,
                          FileShare.Read);
            //stream.Position = 0;

            GM.PlayerData data = new GM.PlayerData();

            //stream.Seek(0, SeekOrigin.Begin);

            data = formatter.Deserialize(stream) as GM.PlayerData;
            stream.Close();


            return data;
        }
        else
        {
            Debug.Log("<color=red>Load Error</color>");
            return null;
        }
    }
    //----------------------------------


    //Time-------------------------
    public static void SaveTimeData(TimeManager.TimeData _time)
    {

        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/TimeData";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);

        }
        path += "/" + "Time" + ".time";
        
        FileStream stream = new FileStream(path,
                         FileMode.Create,
                         FileAccess.Write, FileShare.None);

        TimeManager.TimeData data = _time;

        stream.Seek(0, SeekOrigin.Begin);

        formatter.Serialize(stream, data);
        stream.Close();
        stream.Dispose();

    }
    public static TimeManager.TimeData LoadTimeData()
    {
        string path = Application.persistentDataPath + "/TimeData/" + "Time" + ".time";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open,
                          FileAccess.Read,
                          FileShare.Read);
            stream.Position = 0;

            TimeManager.TimeData data = new TimeManager.TimeData();

            stream.Seek(0, SeekOrigin.Begin);

            data = (TimeManager.TimeData)formatter.Deserialize(stream);
            stream.Close();


            return data;
        }
        else
        {
            Debug.Log("Load Error");
            return null;
        }
    }

    //-----------------------------

    //Mission------------------------
    public static void SaveMissionData(List<MissionManager.MissionData> missions) {

        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/Mission";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);

        }
        path += "/" + "MissionData" + ".mis";

        FileStream stream = new FileStream(path,
                         FileMode.Create,
                         FileAccess.Write, FileShare.None);

        //CustumeEditNode.Node data = new CustumeEditNode.Node();

        stream.Seek(0, SeekOrigin.Begin);

        formatter.Serialize(stream, missions);
        stream.Close();
        stream.Dispose();

    }

    public static List<MissionManager.MissionData> LoadMissinData()
    {
        string path = Application.persistentDataPath + "/Mission/" + "MissionData" + ".mis";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open,
                          FileAccess.Read,
                          FileShare.Read);
            stream.Position = 0;

            List<MissionManager.MissionData> data = new List<MissionManager.MissionData>();

            stream.Seek(0, SeekOrigin.Begin);

            data = formatter.Deserialize(stream) as List<MissionManager.MissionData>;
            stream.Close();


            return data;
        }
        else
        {
            Debug.Log("<color=red>Load Error</color>");
            return null;
        }
    }
    //-------------------------------


    //StoryProecss--------------------
    public static void SaveStoryProcess(StoryManager.StoryProcess _process) {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/StoryProcess";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);

        }
        path += "/" + "Process" + ".pro";

        FileStream stream = new FileStream(path,
                         FileMode.Create,
                         FileAccess.Write, FileShare.None);


        stream.Seek(0, SeekOrigin.Begin);

        formatter.Serialize(stream, _process);
        stream.Close();
        stream.Dispose();
    }

    public static StoryManager.StoryProcess LoadStoryProcess()
    {
        string path = Application.persistentDataPath + "/StoryProcess/" + "Process" + ".pro";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open,
                          FileAccess.Read,
                          FileShare.Read);
            stream.Position = 0;

            StoryManager.StoryProcess data = new StoryManager.StoryProcess();

            stream.Seek(0, SeekOrigin.Begin);

            data = formatter.Deserialize(stream) as StoryManager.StoryProcess;
            stream.Close();


            return data;
        }
        else
        {
            Debug.Log("<color=red>Load Error</color>");
            return null;
        }
    }
    //---------------------------------

}
