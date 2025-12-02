using UnityEngine;
using System.Collections;
public class AutoOpenLid : MonoBehaviour
{
    [Header("Settings")]
    public float openAngle = -90f;
    public float smoothSpeed = 2f;
    public Vector3 rotationAxis = new Vector3(1, 0, 0);

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        // คำนวณค่าตำแหน่งเตรียมไว้
        closedRotation = transform.localRotation;
        openRotation = Quaternion.Euler(rotationAxis * openAngle) * closedRotation;
    }

    void Update()
    {
        // ถ้า isOpen เป็น true ฝากล่องจะค่อยๆ หมุน
        if (isOpen)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, openRotation, Time.deltaTime * smoothSpeed);
        }
    }

    // -----------------------------------------------------------
    // ฟังก์ชันนี้คือตัวสำคัญ! ต้องเป็น public void ถึงจะโผล่ในปุ่มกด
    // -----------------------------------------------------------
    public void OpenTheBox()
    {
        isOpen = true;
        Debug.Log("Box Opened via Select!");
    }
}
