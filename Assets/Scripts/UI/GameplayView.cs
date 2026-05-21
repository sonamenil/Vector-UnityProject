using Nekki.Vector.Core.Gadgets;
using Nekki.Vector.GUI.Scenes.Run;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace UI
{
    [View("GameplayScreenView")]
    public class GameplayView : ScreenViewWithCommonPayload<GameplayScreen>
    {
        public Text Caption;

        public UnityEngine.UI.Button ButtonPause;

        public UnityEngine.UI.Button ButtonReplay;

        public GameObject Gadgets;

        public UnityEngine.UI.Button UseGadgetsButton;

        public Text GadgetsCount;

        public FPSMeter FPSMeter;

        public RunStats RunStats;

        public Image GadgetIcon;

		private bool _stepFrame;

        public override void Init(GameplayScreen screen)
        {
			if (Game.Instance.Snail)
			{
				ButtonPause.transform.parent.gameObject.SetActive(false);
				ButtonReplay.transform.parent.gameObject.SetActive(false);

				bool showGadgetUI = Game.Instance.SnailSett.ShowUI;
				Gadgets.SetActive(showGadgetUI);

				FPSMeter.gameObject.SetActive(Game.Instance.SnailSett.ShowStats);
				RunStats.gameObject.SetActive(Game.Instance.SnailSett.ShowStats);

				if (!Game.Instance.SnailSett.ShowUI)
				{
					gameObject.SetActive(false);
				}
			}
            ButtonPause.onClick.AddListener(() =>
            {
                if (Game.Instance.Snail)
                {
                    LevelMainController.current.pauseRender = !LevelMainController.current.pauseRender;
                    return;
                }
                if (!LevelMainController.current.CanPauseOrReload)
                {
                    return;
                }
                Game.Instance.ScreenManager.Show<GameplayPauseScreen>(false, false);
                LevelMainController.current.pauseRender = true;
            });
            ButtonReplay.onClick.AddListener(() =>
            {
                if (!Game.Instance.Snail && !LevelMainController.current.CanPauseOrReload)
                {
                    return;
                }
                LevelMainController.current.ReloadButton();
            });
            UseGadgetsButton.onClick.AddListener(() =>
            {
                if (UserDataManager.RuntimeInfo.IsHunterMode)
                {
                    return;
                }
                if (!GadgetUtils.HasAnyEquippedGadget())
                {
                    return;
                }
				var gadget = GadgetDatabase.All.FirstOrDefault();

				if (gadget != null)
				{
					LevelMainController.current.controllerGadgets.ActivateGadget(gadget);
				}
            });
            UserDataManager.Instance.ShopData.Updated += InventoryOnUpdated;
        }

		private void Update()
		{
			if (!Game.Instance.Snail)
				return;

			/*if (Keyboard.current.numpad6Key.wasPressedThisFrame)
			{
				StartCoroutine(StepOneFrame());
			}

			if (Keyboard.current.numpad4Key.wasPressedThisFrame)
			{
				Debug.Log("Previous frame requires snapshot system");
			}
			*/

			if (Keyboard.current.zKey.wasPressedThisFrame)
			{
				UseGadgetsButton.onClick.Invoke();
			}
		}

		// returns unity internal log spam
		/*
		private bool _isSteppingFrame;

		private IEnumerator StepOneFrame()
		{
			if (Keyboard.current.numpad6Key.wasPressedThisFrame)
			{
				Debug.Log("STEP FRAME");
				StartCoroutine(StepOneFrame());
			}

			if (_isSteppingFrame)
				yield break;

			_isSteppingFrame = true;

			LevelMainController.current.pauseRender = false;

			// allow exactly one update/render pass
			yield return null;

			LevelMainController.current.pauseRender = true;

			_isSteppingFrame = false;
		}*/

        public override void PreShow(CommonPayloadData payload)
        {
            Caption.text = "You are playing " + string.Format("{0}-", UserDataManager.RuntimeInfo.CurentLocationType) + string.Format("{0}", UserDataManager.RuntimeInfo.CurrentStory + 1);
            InventoryOnUpdated(UserDataManager.Instance.ShopData);
            
            actions.Gameplay.Restart.performed += _ => ButtonReplay.onClick.Invoke();

			actions.Gameplay.Gadget.performed += _ =>
			{
				if (UserDataManager.RuntimeInfo.IsHunterMode)
					return;

				if (!GadgetUtils.HasAnyEquippedGadget())
					return;

				var gadget = GadgetDatabase.All.FirstOrDefault();

				if (gadget != null)
				{
					LevelMainController.current.controllerGadgets.ActivateGadget(gadget);
				}
			};
        }

		private void InventoryOnUpdated(ShopData inventory)
		{
			if (UserDataManager.RuntimeInfo.IsHunterMode)
			{
				Gadgets.SetActive(false);
				return;
			}

			int totalCount = GadgetUtils.GetTotalCharges();

			if (!GadgetUtils.HasAnyEquippedGadget() || totalCount < 1)
			{
				Gadgets.SetActive(false);

				// optional: clear stale text
				// GadgetsCount.text = "0";

				return;
			}

			// in snail mode, only show if UI enabled
			if (Game.Instance.Snail)
			{
				Gadgets.SetActive(Game.Instance.SnailSett.ShowUI);
			}
			else
			{
				Gadgets.SetActive(true);
			}

			GadgetsCount.text = totalCount.ToString();
		}

        public override void Back()
        {
            ButtonPause.onClick?.Invoke();
        }
    }
}
