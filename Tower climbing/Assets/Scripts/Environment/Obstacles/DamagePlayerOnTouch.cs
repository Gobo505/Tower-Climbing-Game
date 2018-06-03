using Assets.Scripts.Managers;
using Assets.Scripts.Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Environment.Obstacles {

    public class DamagePlayerOnTouch : MonoBehaviour {

        private void OnTriggerEnter2D(Collider2D collision) {
            if(collision.GetComponent<Player>()) {
                LevelManager.Instance.DamagePlayer();
            }
        }
    }
}