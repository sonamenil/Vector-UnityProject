using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
	public class BoostPopupView : ScreenView<BoostPopup, BoostPayloadData>
	{
		public UnityEngine.UI.Button QuitButton;

		public UnityEngine.UI.Button CancelButton;

		public Text Title;

		public Text ButtonOKText;

		public Text ButtonCancelText;

		public Action<bool> onResult;

		[SerializeField]
		private Text _line1;

		[SerializeField]
		private Text _line2_1;

		[SerializeField]
		private Image _line2_2;

		[SerializeField]
		private Text _line2_3;

		[SerializeField]
		private Text _line3_1;

		[SerializeField]
		private Image _line3_2;

		[SerializeField]
		private Text _line3_3;

		[SerializeField]
		private Text _goldNominal;

		[SerializeField]
		private Text _rubyNominal;

		public override void Init(BoostPopup popup)
		{
		}

		private IEnumerator Hide(ScreenManager screenManager)
		{
			return null;
		}

		public override void PreShow(BoostPayloadData payload)
		{
		}

		private void PreShowBoost()
		{
		}

		private void PreShowAfterBoost()
		{
		}

		public override void PostShow(BoostPayloadData payload)
		{
		}

		public override void SetSelectedGO()
		{
			EventSystem.current.SetSelectedGameObject(QuitButton.gameObject);
		}

		public override void Back()
		{
		}
	}
}
