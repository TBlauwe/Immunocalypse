using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public static class PersistentData {

    [System.Serializable]
    public static class Level
    {
        public static GameObject selectedLevel;
        public static GameObject selectedDifficulty;
    }

    /**
	public static void Save () {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create (Application.persistentDataPath + "/save");
        bf.Serialize(file, currentLevel);
        file.Close();
	}
	
	public static void Load () {
		
	}
    **/
}
