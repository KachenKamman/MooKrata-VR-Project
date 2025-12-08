using UnityEngine;
using UnityEngine.SceneManagement; // <<--- เพิ่มบรรทัดนี้
using System.Collections;

public class SceneChanger : MonoBehaviour
{
    [Header("Settings")]
    // เปลี่ยนจากมุมเปิด เป็นชื่อ Scene เป้าหมาย
    public string targetSceneName = "END";

    // ตัวแปรอื่นๆ ที่ไม่จำเป็นสำหรับการเปิด Scene สามารถลบออกได้
    // private bool isOpen = false; 
    // private Quaternion closedRotation;
    // private Quaternion openRotation;

    void Start()
    {
        // โค้ดเกี่ยวกับการคำนวณการหมุนไม่จำเป็นแล้ว
    }

    void Update()
    {
        // โค้ดเกี่ยวกับการหมุนไม่จำเป็นแล้ว
    }

    // -----------------------------------------------------------
    // ฟังก์ชันนี้คือตัวสำคัญ! ต้องเป็น public void ถึงจะโผล่ในปุ่มกด
    // -----------------------------------------------------------
    public void NEWSCENE()
    {
        // ใช้ SceneManager.LoadScene() เพื่อโหลด Scene ใหม่
        SceneManager.LoadScene(targetSceneName);
        Debug.Log("Attempting to load Scene: " + targetSceneName);
    }
}