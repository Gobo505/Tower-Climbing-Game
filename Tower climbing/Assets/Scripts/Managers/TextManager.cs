using Assets.Scripts.Level;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Managers {

    public class TextManager : SingletonComponent<TextManager> {

        public static TextManager Instance {
            get { return ((TextManager)_Instance); }
            set { _Instance = value; }
        }

        public string FormatNumber(int score) {
            string scoreString = score.ToString();

            StringBuilder stringBuilder = new StringBuilder();
            for(int i = 0; i < scoreString.Length; i++) {
                if(i != 0 && i % 3 == 0) {
                    stringBuilder.Insert(0, ',');
                }
                stringBuilder.Insert(0, scoreString[scoreString.Length - (i + 1)]);
            }

            return stringBuilder.ToString();
        }
    }
}