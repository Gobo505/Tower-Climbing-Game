using Assets.Scripts.Listeners;
using Assets.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Players {

    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour, IPlayerDeathListener, IPlayerRespawnListener, IPlayerDamageListener {

        public bool IsInvulnerable {
            get {
                return isInvulnerable;
            }
        }

        public bool IsAlive {
            get {
                return Lives > 0;
            }
        }

        public int Lives { get; private set; }

        [SerializeField]
        [Tooltip("Player animator")]
        private Animator animator;

        [SerializeField]
        [Tooltip("Sound that play on player death")]
        private AudioClip deathSound;

        [SerializeField]
        [Tooltip("Sound that play on player getting damage")]
        private AudioClip damageSound;

        [SerializeField]
        [Tooltip("Determines how long player will be invulnerable after getting hit")]
        private float invulnerableTime = 2.5f;

        [SerializeField]
        [Tooltip("Flag that determines if player can be damaged. Should be used only for testing purposes")]
        private bool isPlayerImmortal = false;

        private new Rigidbody2D rigidbody;
        private int startLives = 3;
        private bool isInvulnerable = false;

        void Awake() {
            ResetPlayerLives();
            rigidbody = GetComponent<Rigidbody2D>();
        }

        public void OnPlayerDeath() {
            rigidbody.gravityScale = 30;
            rigidbody.AddForce(new Vector2(Random.Range(-25, 25f), Random.Range(50, 75)));
            if(deathSound != null) {
                AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position);
            }
            animator.SetBool("isDead", true);
            Debug.Log("Odegraj animacje smierci");
        }

        public void OnPlayerRespawn() {
            ResetRigidbodyAndTransform();
            ResetPlayerLives();

            animator.SetBool("isDead", false);
            Debug.Log("Przywróć normalną animację");
        }

        private void ResetRigidbodyAndTransform() {
            rigidbody.gravityScale = 0;
            rigidbody.velocity = new Vector3(0f, 0f, 0f);
            rigidbody.angularVelocity = 0;

            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            transform.position = new Vector3(0f, 0f, 0f);
        }

        private void ResetPlayerLives() {
            Lives = startLives;
        }

        public void OnPlayerDamage() {
            if(isPlayerImmortal) {
                return;
            }

            Lives -= 1;
            if(Lives <= 0) {
                LevelManager.Instance.KillPlayer();
                return;
            }

            if(damageSound != null) {
                AudioSource.PlayClipAtPoint(damageSound, Camera.main.transform.position);
            }
            StartCoroutine(MakePlayerTemporaryInvulnerable());
        }

        private IEnumerator MakePlayerTemporaryInvulnerable() {
            isInvulnerable = true;
            animator.SetBool("isInvulnerable", true);
            yield return new WaitForSeconds(invulnerableTime);
            isInvulnerable = false;
            animator.SetBool("isInvulnerable", false);
        }
    }
}
