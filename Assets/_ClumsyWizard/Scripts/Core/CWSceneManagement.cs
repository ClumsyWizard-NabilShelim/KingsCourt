using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace ClumsyWizard.Utilities
{
    public interface ISceneLoadEvent
    {
        public void OnSceneLoadTriggered(Action onComplete);
    }

    public abstract class CWSceneManagement : Persistant<CWSceneManagement>, ISceneLoadEvent
    {
        private bool loading;
        public string CurrentLevel => UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        //Level Load Cleanup
        private ISceneLoadEvent[] sceneLoadEvents;
        private int currentSceneLoadEventIndex;
        private string sceneToLoad = string.Empty;

        protected virtual void Start()
        {

        }

        public void Load(string sceneName = "")
        {
            if (loading)
                return;

            loading = true;
            StartCoroutine(LoadScene(sceneName));
        }

        protected abstract void OnLoadTriggered();

        private IEnumerator LoadScene(string sceneName)
        {
            if (sceneName == "")
            {
                int buildIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1;
                sceneToLoad = UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(buildIndex).name;
            }
            else
            {
                sceneToLoad = sceneName;
            }
            OnLoadTriggered();
            yield return new WaitForSeconds(1.0f);
            CleanupScene();
        }

        //Scene load cleanup Callbacks
        private void CleanupScene()
        {
            sceneLoadEvents = FindObjectsOfType<MonoBehaviour>().OfType<ISceneLoadEvent>().ToArray();
            currentSceneLoadEventIndex = 0;

            sceneLoadEvents[0].OnSceneLoadTriggered(OnNextObjectCleanup);
        }
        private void OnNextObjectCleanup()
        {
            currentSceneLoadEventIndex++;
            if (currentSceneLoadEventIndex >= sceneLoadEvents.Length)
            {
                StartCoroutine(FinishLoadingScene());
            }
            else
            {
                sceneLoadEvents[currentSceneLoadEventIndex].OnSceneLoadTriggered(OnNextObjectCleanup);
            }
        }

        private IEnumerator FinishLoadingScene()
        {
            AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneToLoad);

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);
                LoadingProgress(progress);
                yield return null;
            }

            OnFinishLoadingScene();
            loading = false;
        }
        protected abstract void LoadingProgress(float progress);
        protected abstract void OnFinishLoadingScene();
    }
}