using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Valrok.SceneManagement
{
    public class SceneSwitcher : MonoBehaviour
    {
        [SerializeField] string scene;
        [SerializeField] bool async = true;

        public void Switch()
        {
            if (scene == "")
            {
                return;
            }

            if (async)
            {
                SceneManger.instance.LoadSceneAsync(scene);
            }
            else
            {
                SceneManger.instance.LoadScene(scene);
            }
        }
    }
}
