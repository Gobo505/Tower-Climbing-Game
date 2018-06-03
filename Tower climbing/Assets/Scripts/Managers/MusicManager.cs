using Assets.Scripts.Level;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers {

    [RequireComponent(typeof(AudioSource))]
    public class MusicManager : SingletonComponent<MusicManager> {

        public static MusicManager Instance {
            get { return ((MusicManager)_Instance); }
            set { _Instance = value; }
        }

        [SerializeField]
        [Tooltip("Music that will be playing in the background")]
        private AudioClip newMusic;

        void Awake() {
            ContinueMusicBetweenScenes();

            DontDestroyOnLoad(transform.root.gameObject);
        }

        private void ContinueMusicBetweenScenes() {
            GameObject musicManager = GameObject.Find("MusicManager");

            AudioSource audioSource = musicManager.GetComponent<AudioSource>();
            if(audioSource.clip != newMusic) {
                audioSource.clip = newMusic;

                if(audioSource.clip != null) {
                    audioSource.Play();
                }
            }

            DontDestroyOnLoad(musicManager);
        }
    }
}
