using Nekki.Vector.Core.Gadgets;
using Nekki.Vector.GUI.Scenes.Run;
using UnityEngine;
using UnityEngine.UI;

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


        public override void Init(GameplayScreen screen)
        {
            if (Game.Instance.Snail)
            {
                ButtonPause.transform.parent.gameObject.SetActive(false);
                ButtonReplay.transform.parent.gameObject.SetActive(false);
                Gadgets.SetActive(false);
                if (Game.Instance.SnailSett.ShowUI)
                {
                    FPSMeter.gameObject.SetActive(true);
                    RunStats.gameObject.SetActive(true);
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
                if (!UserDataManager.Instance.ShopData.IsEquippedGadgetNotEmpty())
                {
                    return;
                }
                LevelMainController.current.controllerGadgets.ActivateGadget(GadgetType.KillBot);
            });
            UserDataManager.Instance.ShopData.Updated += InventoryOnUpdated;
        }

        // private void Update()
        // {
        //     if (Input.GetKeyDown(KeyCode.R))
        //     {
        //         ButtonReplay.onClick.Invoke();
        //     }
        //     if (Input.GetKeyDown(KeyCode.Escape))
        //     {
        //         ButtonPause.onClick.Invoke();
        //     }
        //     if (!Game.Instance.Snail)
        //     {
        //         if (Input.GetKeyDown(KeyCode.Z))
        //         {
        //             UseGadgetsButton.onClick.Invoke();
        //         }
        //     }
        // }

        public override void PreShow(CommonPayloadData payload)
        {
            Caption.text = "You are playing " + string.Format("{0}-", UserDataManager.RuntimeInfo.CurentLocationType) + string.Format("{0}", UserDataManager.RuntimeInfo.CurrentStory + 1);
            InventoryOnUpdated(UserDataManager.Instance.ShopData);
            
            actions.Gameplay.Restart.performed += _ => ButtonReplay.onClick.Invoke();

            if (!Game.Instance.Snail)
            {
                actions.Gameplay.Gadget.performed += _ => UseGadgetsButton.onClick.Invoke();
            }
        }

        private void InventoryOnUpdated(ShopData inventory)
        {
            if (Game.Instance.Snail)
            {
                return;
            }
            var equipped = inventory.IsEquipped("GADGET_FORCEBLASTER");
            var count = inventory.GetCount("GADGET_FORCEBLASTER");
            if (count < 1 || !equipped || UserDataManager.RuntimeInfo.IsHunterMode)
            {
                Gadgets.SetActive(false);
                return;
            }
            Gadgets.SetActive(true);
            GadgetsCount.text = string.Format("{0}", count);
        }
        
        public override void Back()
        {
            ButtonPause.onClick?.Invoke();
        }
    }
}
