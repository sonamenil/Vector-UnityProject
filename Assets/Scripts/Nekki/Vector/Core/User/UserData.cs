using Nekki.Vector.Core.Animation;
using System.Collections.Generic;
using UnityEngine;
using AnimationInfo = Nekki.Vector.Core.Animation.AnimationInfo;

namespace Nekki.Vector.Core.User
{
	public class UserData
	{
		private string _UserId;

		private Color _Color;

		private int _Gender;

		private List<AnimationInfo> _AvailableAnimation = new List<AnimationInfo>();

		private string _Name;

		private List<string> _Skins = new List<string>();

		private List<string> _Stocks = new List<string>();

		private bool _IsSelf;

		private List<string> _Arrests = new List<string>();

		private List<string> _Murders = new List<string>();

		private List<string> _Respawns = new List<string>();

		private List<string> _Births = new List<string>();

		private string _BirthSpawn;

		private int _AI;

		private bool _IsTrick;

		private bool _IsItem;

		private bool _IsVictory;

		private bool _IsLost;

		private float _StartTime;

		private float _LiveTime;

		private bool _isIcon;

		public string UserId => _UserId;

		public Color Color
		{
			get
			{
				return _Color;
			}
			set
			{
				_Color = value;
			}
		}

		public string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				_Name = value;
			}
		}

		public List<string> Skins
		{
			get
			{
				return _Skins;
			}
			set
			{
				_Skins = value;
			}
		}

		public List<string> Stocks
		{
			get
			{
				return _Stocks;
			}
			set
			{
				_Stocks = value;
			}
		}

		public bool IsSelf
		{
			get
			{
				return _IsSelf;
			}
			set
			{
				_IsSelf = value;
			}
		}

		public bool IsBot => _IsSelf == false;

		public List<string> Arrests
		{
			get
			{
				return _Arrests;
			}
			set
			{
				_Arrests = value;
			}
		}

		public List<string> Murders
		{
			get
			{
				return _Murders;
			}
			set
			{
				_Murders = value;
			}
		}

		public List<string> Respawns
		{
			get
			{
				return _Respawns;
			}
			set
			{
				_Respawns = value;
			}
		}

		public List<string> Births
		{
			get
			{
				return _Births;
			}
			set
			{
				_Births = value;
			}
		}

		public string BirthSpawn
		{
			get
			{
				return _BirthSpawn;
			}
			set
			{
				_BirthSpawn = value;
			}
		}

		public int AI
		{
			get
			{
				return _AI;
			}
			set
			{
				_AI = value;
			}
		}

		public bool IsTrick
		{
			get
			{
				return _IsTrick;
			}
			set
			{
				_IsTrick = value;
			}
		}

		public bool IsItem
		{
			get
			{
				return _IsItem;
			}
			set
			{
				_IsItem = value;
			}
		}

		public bool IsVictory
		{
			get
			{
				return _IsVictory;
			}
			set
			{
				_IsVictory = value;
			}
		}

		public bool IsLost
		{
			get
			{
				return _IsLost;
			}
			set
			{
				_IsLost = value;
			}
		}

		public float StartTime
		{
			get
			{
				return _StartTime;
			}
			set
			{
				_StartTime = value;
			}
		}

		public float LiveTime
		{
			get
			{
				return _LiveTime;
			}
			set
			{
				_LiveTime = value * 60;
			}
		}

		public bool isIcon
		{
			get
			{
				return _isIcon;
			}
			set
			{
				_isIcon = value;
			}
		}

		public UserData(string userID)
		{
			_UserId = userID;
		}

		public void Init()
		{
			SetModeles();
			SetAnimation();
		}

		private void SetModeles()
		{
			var list = new List<string>();
			list.Add("0.xml");

			foreach (var stock in _Stocks)
			{
				list.Add(stock + ".xml");
			}

			foreach (var skin in _Skins)
			{
				list.Add(skin + ".xml");
			}

			if (IsSelf)
			{
                foreach (var item in AbstractManager<StoreManager>.Instance.GetItems(StoreItemType.Gear))
                {
                    if (Game.IsItemEquipped(item.Id))
                    {
                        list.Add(item.Id + ".xml");
                    }
                }
            }

			_Skins = list;
		}

		public void SetAnimation()
		{
			_AvailableAnimation.Clear();
			foreach (var anim in Animations.ToList())
			{
				_AvailableAnimation.Add(anim);
			}
		}

		public void AddTrick(int id)
		{
			var animation = Animation(id);
			if (animation != null)
			{
				var list = Animations.ToList();
				for (int i = 0; i < list.Count; i++)
				{
					if (animation.Id == list[i].Id)
					{
						_AvailableAnimation.Add(list[i]);
					}
				}
			}
		}

		public AnimationInfo Animation(string name)
		{
            for (int i = 0; i < _AvailableAnimation.Count; i++)
            {
                if (_AvailableAnimation[i].Name == name)
                {
                    return _AvailableAnimation[i];
                }
            }
            return null;
        }

		public AnimationInfo Animation(int id)
		{
			return null;
		}
	}
}
