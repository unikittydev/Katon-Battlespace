using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Voxels.Editor
{
    public class VoxelWorldEditorUI : MonoBehaviour
    {
        private VoxelWorldEditor editor;

        [SerializeField]
        private TMP_InputField _shipName;
        public TMP_InputField shipName => _shipName;
        [SerializeField]
        private TMP_Text shipClass;
        [SerializeField]
        private Slider voxelCount;
        [SerializeField]
        private TMP_Text maxBounds;
        [SerializeField]
        private TMP_Text health;
        [SerializeField]
        private TMP_Text bounds;

        [SerializeField]
        private SplashMessage warningMessage;

        private void Start()
        {
            editor = GetComponent<VoxelWorldEditor>();

            voxelCount.maxValue = editor.maxVoxelCount;
            maxBounds.SetText($"{editor.maxSize.x}×{editor.maxSize.y}×{editor.maxSize.z}");
        }

        public void UpdateUI()
        {
            voxelCount.value = editor.voxelCount;
            health.SetText(editor.totalHealth.ToString());
            bounds.SetText($"{editor.size.x}×{editor.size.y}×{editor.size.z}");
        }

        public void OnVoxelEditorExit()
        {
            StartCoroutine(MainMenuUI.LoadSceneCoroutine("Menu"));
        }

        public void ShowWarningMessage(string text)
        {
            warningMessage.SetActive(text);
        }
    }
}
