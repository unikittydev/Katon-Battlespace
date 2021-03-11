using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public enum GameType
    {
        [Tooltip("������ ������������ (����� ������������), �� ������� � ������� ����� �� �������������.")]
        VoxelDamageOnly,
        [Tooltip("������ ������������ (����� ������������), ������� � ������� ����� �������������.")]
        VoxelPartialDestruction,
        [Tooltip("������ ������������ ��� ����� ����� �����.")]
        VoxelFullDestruction
    }

    public class GameSettings : MonoBehaviour
    {
        private static GameSettings instance;

        [SerializeField]
        [Tooltip("����� ����.")]
        private GameType _gameType;

        /// <summary>
        /// ����� ����.
        /// </summary>
        public static GameType gameType => instance._gameType;

        /// <summary>
        /// ���������� �� ������ ��� ������?
        /// </summary>
        public static bool processCollisions => gameType == GameType.VoxelFullDestruction;

        /// <summary>
        /// ����������� �� ������� � ������� �����?
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
        /// ������������� ����� ���� �� � ������.
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
