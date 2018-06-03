using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Environment.Obstacles {

    public class Obstacle : MonoBehaviour {

        [SerializeField]
        [Tooltip("Obstacle health points")]
        private int health;
    }
}
