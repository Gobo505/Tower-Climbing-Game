using Assets.Scripts.Level;
using Assets.Scripts.Listeners;
using Assets.Scripts.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Managers {

    public class MenuManager : SingletonComponent<MenuManager>, IPlayerDeathListener, IPlayerRespawnListener {

        public static MenuManager Instance {
            get { return ((MenuManager)_Instance); }
            set { _Instance = value; }
        }

        [SerializeField]
        [Tooltip("Menu that will be displayed when player is alive")]
        private GameObject menuPlayerAlive;

        [SerializeField]
        [Tooltip("Menu that will be displayed when player is dead")]
        private GameObject menuPlayerDead;

        [SerializeField]
        [Tooltip("Menu that will be displayed when player is dead and before he touched the screen")]
        private GameObject menuPlayerDeadFirstScreen;

        [SerializeField]
        [Tooltip("Menu that will be displayed when player is dead and touched the screen")]
        private GameObject menuPlayerDeadSecondScreen;

        private Player player;

        void Start() {
            player = FindObjectOfType<Player>();

            menuPlayerAlive.SetActive(true);
            menuPlayerDead.SetActive(false);
        }

        public void Update() {
            if(!player.IsAlive && Input.touchCount > 0) {
                Touch touch = Input.GetTouch(0);
                if(touch.phase == TouchPhase.Began) {
                    ActivateSecondDeadScreen();
                }
            }
        }

        private void ActivateSecondDeadScreen() {
            menuPlayerDeadFirstScreen.SetActive(false);
            menuPlayerDeadSecondScreen.SetActive(true);
        }

        public void OnPlayerDeath() {
            menuPlayerAlive.SetActive(false);

            menuPlayerDeadFirstScreen.SetActive(true);
            menuPlayerDeadSecondScreen.SetActive(false);
            menuPlayerDead.SetActive(true);
        }

        public void OnPlayerRespawn() {
            menuPlayerAlive.SetActive(true);
            menuPlayerDead.SetActive(false);
        }
    }
}
