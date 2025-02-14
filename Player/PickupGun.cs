using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupGun : MonoBehaviour
{
    [SerializeReference] private GameObject gunPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") == true)
        {
            transform.SetParent(gunPosition.transform, true);
            transform.position = gunPosition.transform.position;
            transform.Rotate(0, 0, -80, Space.Self);
            transform.localScale = new Vector3(0.8f, 0.8f, 1);

            CursorController.GetInstance().playerHanGun = true;
        }
    }
}
