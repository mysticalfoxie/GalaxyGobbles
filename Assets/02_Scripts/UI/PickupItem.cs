// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class PickupItem : MonoBehaviour
// {
//     public Inventory _inventory;
//     public GameObject itemButton;
//     void Start()
//     {
//         _inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
//     }
//
//     private void OnTriggerEnter(Collider other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             for (int i = 0; i < _inventory.Slots.Length; i++)
//             {
//                 if (_inventory.isFull[i] == false)
//                 {
//                     _inventory.isFull[i] = true;
//                     Instantiate(itemButton, _inventory.Slots[i].transform, false);
//                     Destroy(gameObject);
//                     break;
//                 }
//             }
//         }
//     }
//
//     public void OnClickItem()
//     {
//             for (int i = 0; i < _inventory.Slots.Length; i++)
//             {
//                 if (_inventory.isFull[i] == false)
//                 {
//                     _inventory.isFull[i] = true;
//                     Instantiate(itemButton, _inventory.Slots[i].transform, false);
//                     Destroy(gameObject);
//                     break;
//                 }
//             }
//     }
// }
