using UnityEngine;

public class AutoDestroyEffect : MonoBehaviour
{
    public float lifeTime = 2f;

    void Start()
    {
        // สั่งทำลาย "ตัวเอง" (GameObject ที่แปะสคริปต์นี้)
        // หลังจากเวลาผ่านไป lifeTime วินาที
        Destroy(gameObject, lifeTime);
    }
}