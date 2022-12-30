using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valrok.Audio {
    public class Microphone
    {
        public MicrophonePermission Permission { get; private set; }

        public Microphone(AudioSource audioSource)
        {
            Permission = new MicrophonePermission();

            //audioSource.clip = Microphone.star
        }
    }
}
