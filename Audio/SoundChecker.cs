using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valrok.Audio
{
    public class SoundChecker : MonoBehaviour
    {
        [SerializeField] AudioSource audioSource;

        // Start is called before the first frame update
        void Start()
        {
            if (AudioManager.instance.EfxSource.mute)
            {
                audioSource.mute = true;
            }
        }

        private void OnValidate()
        {
            if (!audioSource) audioSource = GetComponent<AudioSource>();
        }
    }
}