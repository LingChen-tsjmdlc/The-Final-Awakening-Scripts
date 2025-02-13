using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator animator;
    public int doorId;
    public string animaTriggerName;


    private void Awake()
    {
        //animator = GetComponent<Animator>();
        Debug.Log("当前的动画器是 " +  gameObject.name + " 的");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 检查碰撞的物体是否带有"Key"标签
        if (collision.CompareTag("Key"))
        {
            // 获取所有带有"Key"标签的物体
            GameObject[] keys = GameObject.FindGameObjectsWithTag("Key");

            // 遍历所有带有"Key"标签的物体
            foreach (GameObject key in keys)
            {
                // 获取物体上的DoorKeyCollisionDetector脚本
                DoorKeyCollisionDetector keyScript = key.GetComponent<DoorKeyCollisionDetector>();

                // 如果脚本存在，并且keyId与doorId相等
                if (keyScript != null && keyScript.keyId == doorId)
                {
                    Debug.Log("当前的钥匙ID：" + keyScript.keyId + ",当前门的ID：" + doorId + ",当前的触发器名字：" + animaTriggerName);
                    Open();
                    Destroy(keyScript.gameObject);
                    Invoke(nameof(DestroyDoor), 2f);
                }
            }
        }
    }

    [ContextMenu("点击开门")]
    public void Open()
    {
        animator.SetTrigger(animaTriggerName);
    }

    private void DestroyDoor()
    {
        Destroy(this.gameObject);
    }
}
