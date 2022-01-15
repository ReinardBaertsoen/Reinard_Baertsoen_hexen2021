using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using DAE.HexSystem;

namespace DAE.GameSystem
{
	public class CardEventArgs: EventArgs
    {
		public Card Card { get; }
		public CardEventArgs(Card card) => Card = card;
    }
	public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, ICard
	{

		public Transform _parentToReturnTo = null;
		public Transform _placeholderParent = null;

		GameObject _placeholder = null;

		[SerializeField] public CardType _cardType;
		public CardType StoredCardType => _cardType;


		public void OnBeginDrag(PointerEventData eventData)
		{
			/*Debug.Log("OnBeginDrag");*/

			_placeholder = new GameObject();
			_placeholder.transform.SetParent(this.transform.parent);
			LayoutElement le = _placeholder.AddComponent<LayoutElement>();
			le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
			le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
			le.flexibleWidth = 0;
			le.flexibleHeight = 0;

			_placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

			_parentToReturnTo = this.transform.parent;
			_placeholderParent = _parentToReturnTo;
			this.transform.SetParent(this.transform.parent.parent);

			GetComponent<CanvasGroup>().blocksRaycasts = false;

			OnBeginDragging(this, new CardEventArgs(this));
		}

		public void OnDrag(PointerEventData eventData)
		{
			//Debug.Log ("OnDrag");

			this.transform.position = eventData.position;

			if (_placeholder.transform.parent != _placeholderParent)
				_placeholder.transform.SetParent(_placeholderParent);

			int newSiblingIndex = _placeholderParent.childCount;

			for (int i = 0; i < _placeholderParent.childCount; i++)
			{
				if (this.transform.position.x < _placeholderParent.GetChild(i).position.x)
				{

					newSiblingIndex = i;

					if (_placeholder.transform.GetSiblingIndex() < newSiblingIndex)
						newSiblingIndex--;

					break;
				}
			}

			_placeholder.transform.SetSiblingIndex(newSiblingIndex);

		}

		public void OnEndDrag(PointerEventData eventData)
		{
			/*Debug.Log("OnEndDrag");*/
			this.transform.SetParent(_parentToReturnTo);
			this.transform.SetSiblingIndex(_placeholder.transform.GetSiblingIndex());
			GetComponent<CanvasGroup>().blocksRaycasts = true;

			Destroy(_placeholder);
		}

        public void Used()
        {
			/*Debug.Log("OnEndDrag");*/
			this.transform.SetParent(_parentToReturnTo);
			this.transform.SetSiblingIndex(_placeholder.transform.GetSiblingIndex());
			GetComponent<CanvasGroup>().blocksRaycasts = true;

			Destroy(_placeholder);
            Destroy(this.gameObject);
        }

		public event EventHandler<CardEventArgs> Clicked;
		public event EventHandler<CardEventArgs> BeginDrag;
		public event EventHandler<CardEventArgs> Dragging;
		public event EventHandler<CardEventArgs> EndDrag;
		public event EventHandler<CardEventArgs> IDrop;

		protected virtual void OnBeginDragging(object s, CardEventArgs e)
        {
			var handler = BeginDrag;
			handler?.Invoke(this, e);
        }
    }

}
