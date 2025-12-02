using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [Header("ลาก Game Object ที่เป็นหน้าต่างจบเกมมาใส่ตรงนี้")]
    public GameObject gameOverPanel; // ตัวแปรที่เห็นในรูป Inspector

    // --- นี่คือฟังก์ชันที่คุณต้องการ ---
    public void EndGame()
    {
        // 1. แสดงข้อความใน Console (เพื่อเช็คว่าทำงาน)
        Debug.Log("จบเกม");

        // 2. หยุดเวลาในเกมทันที (หยุดการเคลื่อนไหวและฟิสิกส์ทั้งหมด)
        Time.timeScale = 0f;

        // 3. เปิดหน้าต่างข้อความ "จบเกม" (UI)
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("ลืมลาก Game Over Panel ใส่ในช่อง Inspector ครับ!");
        }
    }
}
