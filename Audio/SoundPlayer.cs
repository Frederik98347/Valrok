using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valrok.Audio
{
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] AudioClip[] clips;

        /// <summary>
        /// Plays the clip with the clipID provided from the audioclips Array
        /// </summary>
        /// <param name="clip"></param>
        public void PlaySound(int clip)
        {
            AudioManager.instance.PlaySound(clips[clip]);
        }

        /// <summary>
        /// Plays a randomClip from the audioclips array
        /// </summary>
        public void PlayRandomSound()
        {
            AudioManager.instance.PlayRandomSound(clips);
        }
    }
}
