using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public enum GameType
    {
        [Tooltip("Модель повреждается (кроме столкновений), но висящие в воздухе части не отсоединяются.")]
        VoxelDamageOnly,
        [Tooltip("Модель повреждается (кроме столкновений), висящие в воздухе части отсоединяются.")]
        VoxelPartialDestruction,
        [Tooltip("Модель повреждается при любых видах урона.")]
        VoxelFullDestruction
    }

    public class GameSettings : MonoBehaviour
    {
        private static GameSettings instance;

        [SerializeField]
        [Tooltip("Режим игры.")]
        private GameType _gameType;

        /// <summary>
        /// Режим игры.
        /// </summary>
        public static GameType gameType => instance._gameType;

        /// <summary>
        /// Повреждать ли модель при ударах?
        /// </summary>
        public static bool processCollisions => gameType == GameType.VoxelFullDestruction;

        /// <summary>
        /// Отсоединять ли висящие в воздухе части?
        /// </summary>
        public static bool destructWorld => gameType >= GameType.VoxelPartialDestruction;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(this);
        }

        /// <summary>
        /// Устанавливает режим игры до её начала.
        /// </summary>
        public static void SetGameMode(GameType type)
        {
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                Debug.LogError("It is not allowed to change game mode after game start");
                return;
            }
            instance._gameType = type;
        }
    }
}
