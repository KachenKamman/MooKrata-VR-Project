using UnityEngine;
using UnityEngine.UI;

public class Flashlight : MonoBehaviour
{
    [Header("settings")]
    [SerializeField] private GameObject flashlight;
    [SerializeField] private float battery = 100f;
    [SerializeField] private float batteryDepletionSpeed = 0.7f;
    [SerializeField] private KeyCode switchKey = KeyCode.Q;
    private bool on;
    [Header("ui")]
    [SerializeField] private Text batteryText;

    private void Update()
    {
        if (Input.GetKeyUp(switchKey))
            on = !on;
        if (on && battery > 0) 
            battery -= batteryDepletionSpeed * Time.deltaTime;
        else if (battery < 0.01f)
            on = false;

        batteryText.text =$"Battery:{(int)battery}";

        flashlight.SetActive(on);
    }

}
