using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

namespace Valrok.SceneManagement
{
    public class SceneManger : MonoBehaviour
    {
        /// <summary>Static reference to the instance of our Scenemanger</summary>
        public static SceneManger instance;

        [SerializeField] GameObject LoadingScreen;
        [SerializeField] Slider loadingSlider;

        private void Awake()
        {
            // If the instance reference has not been set, yet, 
            if (instance == null)
            {
                // Set this instance as the instance reference.
                instance = this;
            }
            else if (instance != this)
            {
                // If the instance reference has already been set, and this is not the
                // the instance reference, destroy this game object.
                Destroy(gameObject);
            }

            // Do not destroy this object, when we load a new scene.
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if (LoadingScreen != null)
            {
                LoadingScreen.gameObject.SetActive(false);
            }
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void LoadSceneAsync(string sceneName)
        {
            StartCoroutine(LoadLevelWithLoadingAsync(sceneName));
        }

        public void LoadScene(int sceneANumber)
        {
            SceneManager.LoadScene(sceneANumber);
        }

        public void LoadGameMenuScene()
        {
            SceneManager.LoadScene(0);
        }

        public void LoadThisLevelAgain()
        {
            Scene scene = SceneManager.GetActiveScene();
            StartCoroutine(LoadLevelWithLoadingAsync(scene.name, true));
        }

        public void LoadCurrentSelectedLevelWithRewardedAd(string sceneName)
        {
            bool canShowAds = AdsManager.Instance.CanShowRewardedAd(AdsManager.AdType.RETRY_BOOST);
            StartCoroutine(ReplayLevelWithRewardedAds(sceneName, canShowAds));
        }

        IEnumerator LoadLevelWithLoadingAsync(string sceneName, bool allowSceneActivation = true)
        {
            LoadingScreen.gameObject.SetActive(true);

            yield return null;

            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = allowSceneActivation;

            Time.timeScale = 0f;

            while (!operation.isDone)
            {
                if (allowSceneActivation)
                {
                    float progress = Mathf.Clamp01(operation.progress / .9f);
                    loadingSlider.value = progress;
                }
                else
                {
                    if (operation.progress >= 0.9f)
                    {
                        operation.allowSceneActivation = true;
                        loadingSlider.value = 1f;
                    }
                }

                yield return null;
            }

            Time.timeScale = 1f;

            yield return new WaitForSeconds(.1f);

            LoadingScreen.gameObject.SetActive(false);
            yield return null;
        }

        IEnumerator ReplayLevelWithRewardedAds(string sceneName, bool canShowAd)
        {
            LoadingScreen.gameObject.SetActive(true);
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = true;

            if (canShowAd)
            {
                AdsManager.Instance.ShowRewardedAd(AdsManager.AdType.RETRY_BOOST);
                Time.timeScale = 0f;
            }

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / .9f);
                loadingSlider.value = progress;
                yield return null;
            }

            while (AdsManager.Instance.AdPlaying)
            {
                yield return null;
            }

            Time.timeScale = 1f;

            yield return new WaitForSeconds(.1f);

            LoadingScreen.gameObject.SetActive(false);
            yield return null;
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}