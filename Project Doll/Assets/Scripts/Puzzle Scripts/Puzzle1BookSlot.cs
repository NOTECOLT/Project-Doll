using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Puzzle1BookSlot : DroppableSlot {
    private int _bookCount = 0;
    private List<GameObject> _bookSlots = new List<GameObject>();
    void Start() {
        foreach (Transform child in transform) {
            _bookSlots.Add(child.gameObject);
        }
    }
    public override void OnDrop(PointerEventData eventData) {
        if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<DraggableItem>().enableMovement) {
            eventData.pointerDrag.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, -90);
            eventData.pointerDrag.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            _bookCount += 1;

            // Makes book child of book slot transform
            eventData.pointerDrag.transform.SetParent(_bookSlots[_bookCount - 1].transform, false);
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
            
            EventFlagManager.Instance.FlagTickTrue("puzzle1/book" + _bookCount + "Drag");
        }
    }  
}
