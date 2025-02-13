using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingWaterLevels : MonoBehaviour
{
    [SerializeField] private GameObject closeSprite;
    [SerializeField] private GameObject openSprite;

    [SerializeField] private GameObject water2;
    [SerializeField] private GameObject water3;

    [SerializeField] private Transform water2EndPoistion;
    [SerializeField] private Transform water3EndPoistion;

    // 添加成员变量来控制移动速度
    [SerializeField] private float moveSpeed = 1f;

    private bool isFallDownWater;

    private void Start()
    {
        closeSprite.SetActive(true);
        openSprite.SetActive(false);
    }

    private void Update()
    {
        if (!isFallDownWater)
        {
            isFallDownWater = InputManager.GetInstance().ObjectInteraction;
            Debug.Log("isFallDownWater: " + isFallDownWater);
        }
    }

    // TODO: 有bug；按键不太灵敏
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.CompareTag("Player") && isFallDownWater)
        {
            isFallDownWater = false;

            closeSprite.SetActive(!closeSprite.activeSelf);
            openSprite.SetActive(!openSprite.activeSelf);

            FxAudioManager.GetInstance().PlaySound(FxAudioManager.GetInstance().switchPole);
            FxAudioManager.GetInstance().PlaySound(FxAudioManager.GetInstance().fallingWaterLevels);

            FallingWater();
        }
    }

    private void FallingWater()
    {
        StartCoroutine(MoveWater(water2, water2EndPoistion));
        StartCoroutine(MoveWater(water3, water3EndPoistion));
    }

    private IEnumerator MoveWater(GameObject waterObject, Transform endPosition)
    {
        Vector3 startPosition = waterObject.transform.position;
        float journeyLength = Mathf.Abs(endPosition.position.y - startPosition.y);
        float startTime = Time.time;

        while (waterObject.transform.position.y > endPosition.position.y)
        {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distCovered / journeyLength;
            waterObject.transform.position = new Vector3(
                waterObject.transform.position.x,
                Mathf.Lerp(startPosition.y, endPosition.position.y, fractionOfJourney),
                waterObject.transform.position.z
            );
            yield return null;
        }
    }
}
