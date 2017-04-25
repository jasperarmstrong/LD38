using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DisableShootingWhenMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
     public void OnPointerEnter(PointerEventData eventData) {
         GameManager.canShoot = false;
     }
 
     public void OnPointerExit(PointerEventData eventData) {
         GameManager.canShoot = true;
     }
}
