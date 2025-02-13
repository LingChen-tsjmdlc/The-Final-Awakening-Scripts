using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorKeyCollisionDetector : MonoBehaviour
{
    public int keyId;
    public Vector2 offset; // 定义一个二维向量来表示新钥匙的偏移量
    public Vector2 discardOffset; // 定义一个二维向量来表示丢弃旧钥匙的偏移量

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 检查碰撞的物体是否是玩家
        if (collision.CompareTag("Player"))
        {
            // 查找玩家身上是否已经有带有"Key"标签的物体
            Transform existingKey = collision.transform.Find("Key");

            if (existingKey != null)
            {
                // 如果已经有一个钥匙，则替换它
                existingKey.SetParent(null); // 取消当前钥匙的父物体关系
                existingKey.gameObject.SetActive(true); // 确保旧钥匙是激活的
                existingKey.position = collision.transform.position + (Vector3)discardOffset; // 将旧钥匙移到丢弃偏移量的位置
            }

            // 将当前钥匙设置为玩家的子物体，并设置偏移量
            transform.SetParent(collision.transform, false);
            transform.localPosition = offset;
            transform.tag = "Key"; // 确保当前钥匙的标签为"Key"
        }
    }
}
