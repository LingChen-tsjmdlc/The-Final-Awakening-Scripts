using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCreatBollButton : MonoBehaviour
{
    [SerializeField] private GameObject triggerEventsObject;

    private bool isCreated;

    private void Update()
    {
        if (!isCreated)
        {
            isCreated = InputManager.GetInstance().ObjectInteraction;
            Debug.Log("isCreated: " + isCreated);
        }
    }

    private void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.CompareTag("Player") && isCreated)
        {
            isCreated = false;
            triggerEventsObject.GetComponent<BoolCreat>().Throw();
        }
    }

    private void OnTriggerStay2D(Collider2D obj)
    {
        if (obj.CompareTag("Player") && isCreated)
        {
            isCreated = false;
            triggerEventsObject.GetComponent<BoolCreat>().Throw();
        }
    }
}
