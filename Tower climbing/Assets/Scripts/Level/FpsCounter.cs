using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Level {

    class FpsCounter : MonoBehaviour {

        [SerializeField]
        [Tooltip("Flag that determines if FPS counter should be displayed")]
        private bool display = true;

        [SerializeField]
        [Tooltip("FPS text field")]
        private Text fpsText;

        private const string FPS_COUNTER = "FpsCounter";
        private float TimeCounter = 0.0f;

        private void Start() {
            if(fpsText == null) {
                throw new Exception("Please provide FPS Counter Text Field");
            }

            if(display) {
                fpsText.enabled = true;
            } else {
                fpsText.enabled = false;
            }
        }

        void Update() {
            if(fpsText == null) {
                return;
            }

            TimeCounter += (Time.deltaTime - TimeCounter) * 0.1f;
            fpsText.text = string.Format("FPS: {0:0.}", 1.0f / TimeCounter);
        }
    }
}
