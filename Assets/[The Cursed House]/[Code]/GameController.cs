using UnityEngine;
using UnityEngine.SceneManagement; // ใช้สำหรับการรีสตาร์ทฉาก

public class GameController : MonoBehaviour
{
    // ฟังก์ชันสำหรับ "หยุดเกม"
    public void StopGame()
    {
        Debug.Log("Game Over! - Stopping Time");
        Time.timeScale = 0f; // เลข 0 คือหยุดเวลา (เลข 1 คือเวลาเดินปกติ)
    }

    // (แถม) ฟังก์ชันสำหรับ "เริ่มเกมใหม่"
    public void RestartGame()
    {
        Time.timeScale = 1f; // ต้องคืนค่าเวลาก่อน ไม่งั้นเริ่มฉากใหม่เกมจะยังหยุดอยู่
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}