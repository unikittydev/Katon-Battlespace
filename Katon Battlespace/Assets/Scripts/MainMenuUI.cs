using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField]
        private SingleShipLoader spawner;
        [SerializeField]
        private ShipListLoader loader;

        public void OnSingleStart()
        {
            SingleShipLoader.shipName = loader.loadShipName;
            StartCoroutine(LoadSceneCoroutine("Scene"));
        }

        public void OnVoxelEditorStart()
        {
            StartCoroutine(LoadSceneCoroutine("Ship Editor"));
        }

        public void OnExit()
        {
            Application.Quit();
        }

        public static IEnumerator LoadSceneCoroutine(string sceneName)
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
            while (!op.isDone)
                yield return null;
        }
    }
}
