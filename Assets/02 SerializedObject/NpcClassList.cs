using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeginnerToMaster.Exmaple {

    public class NpcClassList : MonoBehaviour {
        public List<Npc> _npcList = new List<Npc>();

        // Start is called before the first frame update
        void Start() {
            foreach (var npc in _npcList) {
                Debug.Log("npc : " + npc._npcId);
            }
        }

        // Update is called once per frame
        void Update() {

        }
    }
}