using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valrok.Audio
{
    public class AudioManager : MonoBehaviour
    {
        float masterVolume = 1f;

        [SerializeField] AudioSource efxSource;
        public AudioSource EfxSource { get => efxSource; }

        [SerializeField] AudioSource musicSource;
        public AudioSource MusicSource { get => musicSource; }

        [SerializeField] float lowPitchRange = .95f;
        [SerializeField] float highPitchRange = 1.05f;
        [Space]
        [SerializeField]
        AudioClip clickSound;

        public static AudioManager instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);

            if (PlayerPrefs.HasKey("Music"))
            {
                var music = PlayerPrefs.GetInt("Music") == 1 ? true : false;
                MusicSource.mute = music;
            }

            if (PlayerPrefs.HasKey("Sound"))
            {
                var sound = PlayerPrefs.GetInt("Sound") == 1 ? true : false;
                EfxSource.mute = sound;
            }
        }

        private void Start()
        {
            efxSource.volume = .75f;
            musicSource.volume = .45f;
        }

        public void PlaySound(AudioClip clip)
        {
            efxSource.clip = clip;

            float randomPitch = Random.Range(lowPitchRange, highPitchRange);
            efxSource.pitch = randomPitch;

            efxSource.Play();
        }

        public void PlayMusic(AudioClip clip)
        {
            musicSource.clip = clip;
            musicSource.volume *= masterVolume;
            musicSource.Play();
        }

        public void PlayRandomSound(params AudioClip[] clips)
        {
            var randomSound = clips[Random.Range(0, clips.Length)];
            PlaySound(randomSound);
        }

        public void PlayClickSound(float volume = .75f)
        {
            if (clickSound)
                efxSource.volume = volume * masterVolume;
            PlaySound(clickSound);
        }

        public void PlayClickSound(AudioClip sound, float volume = .75f)
        {
            efxSource.volume = volume * masterVolume;
            PlaySound(sound);
        }
    }
}