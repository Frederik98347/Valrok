using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valrok.Audio
{
    public class MicrophonePermission
    {
        public bool CanUse { get; private set; }

        public MicrophonePermission()
        {
            var authType = UserAuthorization.Microphone;
            var auth = Application.HasUserAuthorization(authType);

            CanUse = auth;

            if (!CanUse)
            {
                var request = RequestUserAuthorization(authType);
                request.completed += (operation) =>
                {
                    CanUse = true;
                };
            }
        }

        private AsyncOperation RequestUserAuthorization(UserAuthorization request)
        {
            return Application.RequestUserAuthorization(request);
        }
    }
}