using TMPro;
using UnityEngine;

namespace Game.Voxels.Editor
{
    [RequireComponent(typeof(TMP_Text))]
    public class SplashMessage : MonoBehaviour
    {
        private TMP_Text message;

        [SerializeField]
        private float activeTime = 3f;

        private void Awake()
        {
            message = GetComponent<TMP_Text>();
        }

        public void SetActive(string text)
        {
            gameObject.SetActive(true);
            message.SetText(text);

            CancelInvoke(nameof(SetInactive));
            Invoke(nameof(SetInactive), activeTime);
        }

        private void SetInactive()
        {
            gameObject.SetActive(false);
        }
    }
}
