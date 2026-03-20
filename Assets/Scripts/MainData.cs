using Banzai.Json;
using PlayerData;
using System;
using System.Runtime.CompilerServices;

public class MainData : BaseUserHolder<MainData>
{
	private int _coins;

	public bool _isUnlock = true;

	protected sealed override string JSONName => "MainData";

	public bool IsUnlock => _isUnlock;

	public event Action<int, int> CoinsChangedEvent;

	public override void ParseData()
	{
        _coins = JsonUtils.GetInt(_userjObject, "Coins");
		if (!_isUnlock)
		{
			_isUnlock = JsonUtils.GetBool(_userjObject, "Unlock", false);
		}

    }

	public override void SaveData()
	{
		_userjObject["Coins"] = _coins;
		_userjObject["Unlock"] = _isUnlock;
		UserDataManager.Instance.SaveUserDate();
	}

	public void SetCoins(int newValue)
	{
		CoinsChangedEvent?.Invoke(_coins, newValue);
        _coins = newValue;
        SaveData();
	}

	public void AddCoins(int value)
	{
		CoinsChangedEvent?.Invoke(_coins, value + _coins);
        _coins += value;
        SaveData();
	}

	public int GetCoins()
	{
		return _coins;
	}

	public void Unlock()
	{
		_isUnlock = true;
		_coins += UserDataManager.Instance.GameStats.CalcUnlockBonus();
		SaveData();
	}

	public void SetUlock(bool value)
	{
		_isUnlock = value;
	}

	public void Migrate(PlayerDataComponent playerData)
	{
		_coins = (int)playerData.Wallet.Coins;
		_isUnlock = playerData.User.Settings.IsUnlock;
		SaveData();
	}
}
