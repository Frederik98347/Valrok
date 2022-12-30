using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valrok.Console
{
    public class Logger : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.unityLogger.logEnabled = true;
#else
                     Debug.unityLogger.logEnabled = false;
#endif
        }
    }
}
