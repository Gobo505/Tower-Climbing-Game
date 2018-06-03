using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Environment.Tiles {

    public class TowerColumn : MonoBehaviour {

        public Vector2 TowerStartPosition {
            get {
                return towerStartPosition;
            }
        }

        private Vector2 towerStartPosition;

        private void Awake() {
            towerStartPosition = transform.position;
        }
    }
}
