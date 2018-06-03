using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Environment.Tiles {

    [RequireComponent(typeof(SpriteRenderer))]
    public class Tower : MonoBehaviour{

        public Vector2 TowerStartPosition {
            get {
                return towerStartPosition;
            }
        }

        public int TowerNumber {
            get {
                return towerNumber;
            }
        }

        public GameObject BuffsParent {
            get {
                return buffsParent;
            }
        }

        public GameObject ObstaclesParent {
            get {
                return obstaclesParent;
            }
        }

        [SerializeField]
        [Tooltip("Tower height on awake. This will help with random respawning things.")]
        [Range(1, 5)]
        private int towerNumber;

        [SerializeField]
        [Tooltip("Parent game object of buffs")]
        private GameObject buffsParent;

        [SerializeField]
        [Tooltip("Parent game object of obstacles")]
        private GameObject obstaclesParent;

        private Vector2 towerStartPosition;

        private void Awake() {
            towerStartPosition = transform.position;
        }
    }
}
