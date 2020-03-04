using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeginnerToMaster {
    public class HeaderTest : MonoBehaviour {
        [Header("Socket GUI")]
        public float SocketConnectedOpacity = 1f;
        public float SocketCurveModifierMaxValue = 1f;
        public float SocketCurveModifierMinValue = -1f;
        public float SocketDividerHeight = 1f;
        public float SocketDividerOpacity = 0.3f;
        public float SocketHeight = 24f;
        public float SocketNotConnectedOpacity = 0.5f;
        public float SocketVerticalSpacing;

        [Header("Connection GUI")]
        public float ConnectionPointHeight = 16f;
        public float ConnectionPointOffsetFromLeftMargin = -2f;
        public float ConnectionPointOffsetFromRightMargin = 2f;
        public float ConnectionPointWidth = 16f;

        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
    }
}


