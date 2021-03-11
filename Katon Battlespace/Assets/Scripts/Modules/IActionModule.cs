using static UnityEngine.InputSystem.InputAction;

namespace Game.Modules
{
    /// <summary> ��������� ��� �������, ������ ������� ������� �� ����� ������������. </summary>
    public interface IActionModule : System.IDisposable
    {
        /// <summary> �����, ������������ ��� ������ ������� �����. </summary>
        void OnPerformInput(CallbackContext ctx);

        /// <summary> �����, ������������ ��� ������ ������� ������ �����. </summary>
        void OnCancelInput(CallbackContext ctx);
    }
}
