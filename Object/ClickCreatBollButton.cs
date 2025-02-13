using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCreatBollButton : MonoBehaviour
{
    [SerializeField] private GameObject triggerEventsObject;

    private void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                triggerEventsObject.GetComponent<BoolCreat>().Throw();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D obj)
    {
        if (obj.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                triggerEventsObject.GetComponent<BoolCreat>().Throw();
            }
        }
    }
}
