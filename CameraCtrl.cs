using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public GameObject gameCamera;
    public Transform cameraCenter;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // 确保只有当玩家接触到触发器时才移动相机
        {
            // 设置相机的X和Y坐标为cameraCenter的坐标
            Vector3 newPosition = cameraCenter.position;
            // 修改Z坐标为-10
            newPosition.z = -10;
            // 应用新的位置到游戏相机
            gameCamera.transform.position = newPosition;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // 确保只有当玩家接触到触发器时才移动相机
        {
            // 设置相机的X和Y坐标为cameraCenter的坐标
            Vector3 newPosition = cameraCenter.position;
            // 修改Z坐标为-10
            newPosition.z = -10;
            // 应用新的位置到游戏相机
            gameCamera.transform.position = newPosition;
        }
    }
}
