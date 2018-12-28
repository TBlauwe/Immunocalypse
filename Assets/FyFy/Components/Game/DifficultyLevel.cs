﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DifficultyLevel : MonoBehaviour {
    public string           title;                  // ID
    [TextArea(3,10)]
    public string           description;
    public ELevel           scene;                  // Which scene should be loaded
    public bool             victory;                // Used to unlocked children

    public Text             Title;                  // UI STUFF
    public Text             Descrition;             // UI STUFF
}