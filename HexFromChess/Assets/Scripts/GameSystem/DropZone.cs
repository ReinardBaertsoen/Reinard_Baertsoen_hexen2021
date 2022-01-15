using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace DAE.GameSystem
{
    public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {

        public void OnPointerEnter(PointerEventData eventData)
        {
            //Debug.Log("OnPointerEnter");
            if (eventData.pointerDrag == null)
                return;

            Card d = eventData.pointerDrag.GetComponent<Card>();
            if (d != null)
            {
                d._placeholderParent = this.transform;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //Debug.Log("OnPointerExit");
            if (eventData.pointerDrag == null)
                return;

            Card d = eventData.pointerDrag.GetComponent<Card>();
            if (d != null && d._placeholderParent == this.transform)
            {
                d._placeholderParent = d._parentToReturnTo;
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            //Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);
            Card d = eventData.pointerDrag.GetComponent<Card>();
            if (d != null)
            {
                d._parentToReturnTo = this.transform;
            }

        }
    }
}