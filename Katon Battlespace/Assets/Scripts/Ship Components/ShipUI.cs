using Game;
using UnityEngine;
using UnityEngine.UI;

public class ShipUI : MonoBehaviour
{
    [SerializeField]
    private Slider xVelocity;
    [SerializeField]
    private Slider yVelocity;
    [SerializeField]
    private Slider xAngVelocity;
    [SerializeField]
    private Slider yAngVelocity;

    [SerializeField]
    private Ship ship;
    [SerializeField]
    private Transform camTr;

    private void Update()
    {
        Vector3 vel = camTr.InverseTransformDirection(ship.component.rb.velocity);

        xVelocity.value = vel.x;
        yVelocity.value = vel.y;

        Vector3 angVel = camTr.InverseTransformDirection(ship.component.rb.angularVelocity);

        xAngVelocity.value = angVel.x;
        yAngVelocity.value = angVel.y;
    }

    public void OnGameExit()
    {
        StartCoroutine(MainMenuUI.LoadSceneCoroutine("Menu"));
    }
}
