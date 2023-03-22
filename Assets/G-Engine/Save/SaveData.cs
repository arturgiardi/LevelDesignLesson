using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    protected Dictionary<string, float> _floats = new Dictionary<string, float>();
    protected Dictionary<string, int> _integers = new Dictionary<string, int>();
    protected Dictionary<string, string> _strings = new Dictionary<string, string>();


    public void SetFloat(string uniqueName, float value) => _floats.Add(uniqueName, value);
    public void SetInt(string uniqueName, int value) => _integers.Add(uniqueName, value);
    public void SetString(string uniqueName, string value) => _strings.Add(uniqueName, value);
    public void SetBool(string uniqueName, bool value) => _integers.Add(uniqueName, value ? 1 : 0);
    public float GetFloat(string uniqueName)
    {
        if (_floats.ContainsKey(uniqueName))
            return _floats[uniqueName];
        else
            return 0;
    }

    public int GetInt(string uniqueName)
    {
        if (_integers.ContainsKey(uniqueName))
            return _integers[uniqueName];
        else
            return 0;
    }

    public string GetString(string uniqueName)
    {
        if (_strings.ContainsKey(uniqueName))
            return _strings[uniqueName];
        else
            return string.Empty;
    }

    public bool GetBool(string uniqueName)
    {
        return GetInt(uniqueName) != 0;
    }

    public virtual void SetVector3(string uniqueName, Vector3 vector)
    {
        SetFloat(string.Concat(uniqueName, ".x"), vector.x);
        SetFloat(string.Concat(uniqueName, ".y"), vector.y);
        SetFloat(string.Concat(uniqueName, ".z"), vector.z);
    }

    public virtual Vector3 GetVector3(string uniqueName)
    {
        return new Vector3(GetFloat(string.Concat(uniqueName, ".x")),
            GetFloat(string.Concat(uniqueName, ".y")),
            GetFloat(string.Concat(uniqueName, ".z")));
    }

    public void SetColor(string uniqueName, Color color)
    {
        SetFloat(string.Concat(uniqueName, ".r"), color.r);
        SetFloat(string.Concat(uniqueName, ".g"), color.g);
        SetFloat(string.Concat(uniqueName, ".b"), color.b);
        SetFloat(string.Concat(uniqueName, ".a"), color.a);
    }

    public Color GetColor(string uniqueName)
    {
        return new Color(GetFloat(string.Concat(uniqueName, ".r")),
            GetFloat(string.Concat(uniqueName, ".g")),
            GetFloat(string.Concat(uniqueName, ".b")),
            GetFloat(string.Concat(uniqueName, ".a")));
    }
}
