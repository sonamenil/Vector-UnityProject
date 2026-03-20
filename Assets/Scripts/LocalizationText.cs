using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationText : MonoBehaviour
{
    protected Text _text;

    private List<string> _keys = new List<string>();

    private StringBuilder _origText;

    private void Awake()
    {
        _text = GetComponent<Text>();
        if (_text != null)
        {
            var builder = new StringBuilder();
            bool flag = false;
            for (int i = 0; i < _text.text.Length; i++)
            {
                char value = _text.text[i];

                if (value == '%')
                {
                    if (!flag)
                    {
                        flag = true;
                        builder.Clear();
                    }
                    else
                    {
                        _keys.Add(builder.ToString());
                        builder.Clear();
                        flag = false;
                    }
                }
                else if (flag)
                {
                    builder.Append(value);
                }
            }

        }
        var builder1 = new StringBuilder(_text.text);
        _origText = builder1;
        OnLocaleUpdated(true);
        LocalizationManager.Instance.UpdateEvent += OnLocaleUpdated;
    }

    private void OnDestroy()
    {
        if (LocalizationManager.IsInited)
        {
            LocalizationManager.Instance.UpdateEvent -= OnLocaleUpdated;
        }
    }

    private void OnLocaleUpdated(bool updateFont)
    {
        if (updateFont)
        {
            _text.font = LocalizationManager.Instance.CurrentFont;
        }
        if (_text == null || _keys.Count < 1)
        {
            return;
        }
        var builder = new StringBuilder(_origText.ToString());
        foreach (var key in _keys)
        {
            var translation = LocalizationManager.Instance.GetTranslation(key);
            var val = string.Concat("%", key, "%");
            builder.Replace(val, translation);
            SetText(builder);
        }

    }

    protected virtual void SetText(StringBuilder value)
    {
        _text.text = value.ToString();
    }
}
