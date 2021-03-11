using static UnityEngine.InputSystem.InputAction;

namespace Game.Modules
{
    /// <summary> Интерфейс для модулей, работа которых зависит от ввода пользователя. </summary>
    public interface IActionModule : System.IDisposable
    {
        /// <summary> Метод, вызывающийся при вызове события ввода. </summary>
        void OnPerformInput(CallbackContext ctx);

        /// <summary> Метод, вызывающийся при вызове события отмены ввода. </summary>
        void OnCancelInput(CallbackContext ctx);
    }
}
