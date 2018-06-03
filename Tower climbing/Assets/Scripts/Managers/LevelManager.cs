using Assets.Scripts.Level;
using Assets.Scripts.Listeners;
using Assets.Scripts.Players;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Managers {

    public class LevelManager : SingletonComponent<LevelManager> {

        public static LevelManager Instance {
            get { return ((LevelManager)_Instance); }
            set { _Instance = value; }
        }

        private Player player;
        private IEnumerable<IPlayerDeathListener> onPlayerDeathListeners;
        private IEnumerable<IPlayerRespawnListener> onPlayerRespawnListeners;
        private IEnumerable<IPlayerDamageListener> onPlayerDamageListeners;
        private bool paused = false;

        public void Start() {
            player = FindObjectOfType<Player>();
            onPlayerDeathListeners = FindObjectsOfType<MonoBehaviour>().OfType<IPlayerDeathListener>();
            onPlayerRespawnListeners = FindObjectsOfType<MonoBehaviour>().OfType<IPlayerRespawnListener>();
            onPlayerDamageListeners = FindObjectsOfType<MonoBehaviour>().OfType<IPlayerDamageListener>();
        }

        public void Pause() {
            if(paused) {
                Time.timeScale = 1;
            } else {
                Time.timeScale = 0;
            }

            paused = !paused;
            Debug.Log("Dorób pause menu");
        }

        public void DamagePlayer() {
            if(!player.IsAlive || player.IsInvulnerable) {
                return;
            }

            foreach(IPlayerDamageListener listener in GetSortedOnPlayerDamageListeners()) {
                listener.OnPlayerDamage();
            }
        }

        public void KillPlayer() {
            foreach(IPlayerDeathListener listener in GetSortedOnPlayerDeathListeners()) {
                listener.OnPlayerDeath();
            }
        }

        public void RestartLevel() {

            foreach(IPlayerRespawnListener listener in GetSortedOnPlayerRespawnListeners()) {
                listener.OnPlayerRespawn();
            }
        }

        #region Listeners Sorting
        private IOrderedEnumerable<IPlayerDamageListener> GetSortedOnPlayerDamageListeners() {
            return onPlayerDamageListeners.OrderBy(listener => {
                return PlayerListenerComparator(listener);
            });
        }

        private IOrderedEnumerable<IPlayerDeathListener> GetSortedOnPlayerDeathListeners() {
            return onPlayerDeathListeners.OrderBy(listener => {
                return PlayerListenerComparator(listener);
            });
        }

        private IOrderedEnumerable<IPlayerRespawnListener> GetSortedOnPlayerRespawnListeners() {
            return onPlayerRespawnListeners.OrderBy(listener => {
                return PlayerListenerComparator(listener);
            });
        }

        private static int PlayerListenerComparator(Listener listener) {
            if(listener.GetType() == typeof(Player)) {
                return -1;
            } else {
                return 1;
            }
        }
        #endregion Listeners Sorting
    }
}