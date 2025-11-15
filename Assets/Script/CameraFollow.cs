using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       // ลากตัว Player มาใส่ช่องนี้
    public float smoothSpeed = 5f; // ความหน่วงของกล้อง (ยิ่งเยอะยิ่งตามติดเร็ว)

    void LateUpdate()
    {
        // ถ้าไม่มีเป้าหมาย (เช่น Player ตาย) ก็ไม่ต้องทำอะไร
        if (target == null) return;

        // หาตำแหน่งเป้าหมายที่กล้องควรจะอยู่ (เอาแค่ X กับ Y ของผู้เล่น)
        // สำคัญ: ต้องคงค่า Z เดิมของกล้องไว้ (ปกติ -10) ไม่งั้นภาพจะมืดหายไป
        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

        // ใช้ Lerp เพื่อให้กล้องค่อยๆ ขยับไปหาเป้าหมาย (ทำให้ภาพดูนุ่มนวล ไม่กระตุก)
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // ย้ายกล้องไปที่ตำแหน่งใหม่
        transform.position = smoothedPosition;
    }
}