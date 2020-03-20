using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JsonUtilityExample {

    [Serializable]
    public class InputDataEntry {
        public string _name;
        public int _age; 
    }
    
    [Serializable]
    public class InputData {
        public InputDataEntry[] _data;
    }
}