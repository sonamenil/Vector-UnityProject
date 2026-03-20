using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using UnityEngine.EventSystems;

namespace UI
{
	public class LobbyScreenView : ScreenViewWithCommonPayload<LobbyScreen>
	{
		//no filthy ads in my game
		private class ConfigData
		{
			public string Hash;

			public string Url;

			public string Link;

			private ConfigData()
			{
			}

			public ConfigData(string xmlData)
			{
			}

			public bool IsEmpty()
			{
				return false;
			}

			public ConfigData(string hash, string url, string link)
			{
			}

			public void SaveConfig(string text, string path)
			{
			}

			public static ConfigData LoadConfig(string path)
			{
				return null;
			}

			public bool Equals(ConfigData data)
			{
				return false;
			}
		}

		public UnityEngine.UI.Button PlayButton;

		public UnityEngine.UI.Button StoreButton;

		public UnityEngine.UI.Button StatsButton;

		public UnityEngine.UI.Button FollowUsFacebookButton;

		public UnityEngine.UI.Button FollowUsTwitterButton;

		public UnityEngine.UI.Button FollowUsTikTokButton;

		public UnityEngine.UI.Button GetFreeButton;

		public UnityEngine.UI.Button MoreGamesButton;

		public UnityEngine.UI.Button CreditsButton;

		public UnityEngine.UI.Button OptionsButton;

		public UnityEngine.UI.Button BuyCoinsButton;

		public StarsLobbyView StarsLobbyView;

		public UnityEngine.UI.Button ADSButton;

		public Image ADSImage;

		private static string _adsPath;

		private static string _adsConfig;

		private static readonly string _adsUrl;

		private string _adsLink;

		public LobbyScreenView() : base()
		{

		}

		public override void Init(LobbyScreen lobbyScreen)
		{
			ADSButton.gameObject.SetActive(false);
            GetFreeButton.gameObject.SetActive(false);
            BuyCoinsButton.gameObject.SetActive(false);
			PlayButton.onClick.AddListener(new UnityAction(lobbyScreen.PlayButton.PressedAction));
            StoreButton.onClick.AddListener(new UnityAction(lobbyScreen.StoreButton.PressedAction));
            StatsButton.onClick.AddListener(new UnityAction(lobbyScreen.StatsButton.PressedAction));
            FollowUsFacebookButton.onClick.AddListener(new UnityAction(lobbyScreen.FollowUsFacebookButton.PressedAction));
            FollowUsTwitterButton.onClick.AddListener(new UnityAction(lobbyScreen.FollowUsTwitterButton.PressedAction));
            FollowUsTikTokButton.onClick.AddListener(new UnityAction(lobbyScreen.FollowUsTikTokButton.PressedAction));
            GetFreeButton.onClick.AddListener(new UnityAction(() => Game.Instance.ScreenManager.Show<GetFreeScreen>(true, true)));
            MoreGamesButton.onClick.AddListener(new UnityAction(lobbyScreen.MoreGamesButton.PressedAction));
            CreditsButton.onClick.AddListener(new UnityAction(lobbyScreen.CreditsButton.PressedAction));
            OptionsButton.onClick.AddListener(new UnityAction(lobbyScreen.OptionsButton.PressedAction));
            BuyCoinsButton.onClick.AddListener(new UnityAction(() =>
			{
				Game.Instance.ScreenManager.Show<BuyCoinsScreen>(true, true);
				SoundsManager.Instance.PlaySounds(SoundType.ui_click);
			}));
        }

        public void Start()
		{
		}

		public override void Back()
		{
			var title = LocalizationManager.Instance.GetTranslation("msg_quit_tit");
			var message = LocalizationManager.Instance.GetTranslation("msg_quit");
			var ok = LocalizationManager.Instance.GetTranslation("ctrl_ok");
			var cancel = LocalizationManager.Instance.GetTranslation("ctrl_cancel");

			Action<bool> action = new Action<bool>((b) =>
			{
				if (b)
				{
					Game.Instance.ScreenManager.ClosePopup();
					Utils.Utils.QuitGame();
				}
				else
				{
                    Game.Instance.ScreenManager.ClosePopup();
                }
            });

			Game.Instance.ScreenManager.Popup<AlertPopup, AlertPayloadData>(new AlertPayloadData(title, message, ok, cancel, action));
		}

		public override void PreShow(CommonPayloadData payload)
		{
			var gameStats = UserDataManager.Instance.GameStats;

            foreach (Transform child in StarsLobbyView.Content.transform)
            {
                Destroy(child.gameObject);
            }

            int count = 0;

            foreach (var location in LocationManager.Instance.locations.Keys)
            {
                foreach (var mode in LocationManager.Instance.locations[location].Keys)
                {
                    var starsView = Instantiate(Resources.Load<StarsLobbyItemView>("StarsItem"), StarsLobbyView.Content.transform);

                    starsView.Gradient.SetActive(count % 2 == 0);

                    string locationID = string.Format("%item_{0}%", location);
                    string modeID = string.Format("(%item_mode_{0}%)", mode == LocationModeType.Hunter ? "hunter" : "story");

                    starsView.LocationInfo.text = locationID + " " + modeID;
                    starsView.Count.text = gameStats.GetLocationStars(mode, location).ToString();

                    count++;
                }
            }
        }

		public override void SetSelectedGO()
		{
            EventSystem.current.SetSelectedGameObject(PlayButton.gameObject);
		}

		private void setADS(Texture2D tex)
		{
		}

		private Texture2D LoadPNGFromFile(string filePath)
		{
			return null;
		}

		private IEnumerator DownloadImage(string url)
		{
			return null;
		}

		private IEnumerator DownloadConfig(string url)
		{
			return null;
		}
	}
}
