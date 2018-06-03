using Assets.Scripts.Common;
using Assets.Scripts.Level;
using Assets.Scripts.Listeners;
using Assets.Scripts.Players;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Managers {
    public class LevelUIManager : SingletonComponent<LevelUIManager>, IPlayerDeathListener, IPlayerRespawnListener, IPlayerDamageListener {

        public static LevelUIManager Instance {
            get { return ((LevelUIManager)_Instance); }
            set { _Instance = value; }
        }
        
        private static readonly string BEST_HEIGHT = "PlayerBestHeight";

        [SerializeField]
        [Tooltip("Coins text field that shows when player is alive")]
        private Text coinsInGameText;

        [SerializeField]
        [Tooltip("Coins text field that shows when player is dead")]
        private Text coinsDeathScreenText;

        [SerializeField]
        [Tooltip("Coins total multiplier text field that shows when player is dead")]
        private Text coinsTotalMultipierText;

        [SerializeField]
        [Tooltip("Coins after calculation by multiplier text field that shows when player is dead")]
        private Text coinsAfterCalculationText;

        [SerializeField]
        [Tooltip("Coins total text field that shows when player is dead")]
        private Text coinsTotalText;

        [SerializeField]
        [Tooltip("Player height text field")]
        private Text playerHeightText;

        [SerializeField]
        [Tooltip("Player lives text field")]
        private Text playerLivesText;

        private int coins = 0;
        private float height = 0f;
        private float coinsMultiplier = 1;
        private Player player;

        private void Start() {
            player = FindObjectOfType<Player>();
            ResetPlayerUI();
            StartCoroutine(ChangePlayerHeight());
        }

        private IEnumerator ChangePlayerHeight() {
            yield return new WaitForSeconds(0.5f);

            height += Math.Max(TowerManager.Instance.TowerMovingDownSpeed / 10f, 1);
            SetHeightIfBest();
            UpdatePlayerHeightText();
            StartCoroutine(ChangePlayerHeight());
        }

        private void SetHeightIfBest() {
            if(height > PlayerPrefs.GetInt(Constants.GAME_PLAYER_PREFS_NAME + BEST_HEIGHT, 0)) {
                PlayerPrefs.SetInt(Constants.GAME_PLAYER_PREFS_NAME + BEST_HEIGHT, (int)height);
            }
        }

        public void AddCoins(int coinsToAdd) {
            coins += (int)(coinsToAdd * coinsMultiplier);

            UpdatePlayerCoinsText(coinsInGameText);
        }

        public void ResetPlayerUI() {
            coins = 0;
            height = 0;
            UpdatePlayerCoinsText(coinsInGameText);
            UpdatePlayerHeightText();
            UpdatePlayerLivesText();
        }

        #region Change Text
        private void UpdatePlayerCoinsText(Text coinsTextField) {
            StringBuilder stringBuilder = new StringBuilder();
            coinsTextField.text = stringBuilder.Append(" ").Append(TextManager.Instance.FormatNumber(coins)).ToString();
        }

        private void UpdatePlayerHeightText() {
            StringBuilder stringBuilder = new StringBuilder();
            playerHeightText.text = stringBuilder.Append(" ").Append(TextManager.Instance.FormatNumber((int)height)).Append("m").ToString();
        }

        private void UpdatePlayerLivesText() {
            StringBuilder stringBuilder = new StringBuilder();
            playerLivesText.text = stringBuilder.Append(" ").Append(TextManager.Instance.FormatNumber(player.Lives)).ToString();
        }
        #endregion Change Text

        public void OnPlayerDeath() {
            StopAllCoroutines();
        }

        public void OnPlayerRespawn() {
            ResetPlayerUI();
            StartCoroutine(ChangePlayerHeight());
        }

        public void OnPlayerDamage() {
            UpdatePlayerLivesText();
        }
    }
}
