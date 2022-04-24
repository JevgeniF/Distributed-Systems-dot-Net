﻿namespace Base.Domain;

public class LangStr : Dictionary<string, string>
{
    private const string DefaultCulture = "en";

    public LangStr(string value) : this(value, Thread.CurrentThread.CurrentUICulture.Name)
    {
    }

    public LangStr()
    {
    }

    public LangStr(string value, string culture)
    {
        this[GetCultureName(culture)] = value;
    }

    private string GetCultureName(string culture)
    {
        return culture.Split("-")[0];
    }

    public void SetTranslation(string value)
    {
        this[GetCultureName(Thread.CurrentThread.CurrentUICulture.Name)] = value;
    }

    public string? Translate(string? culture = null)
    {
        // if there is exact match
        if (Count == 0) return null;
        culture = culture?.Trim() ?? Thread.CurrentThread.CurrentUICulture.Name;
        culture = GetCultureName(culture);

        if (ContainsKey(culture)) return this[culture];

        var neutralCulture = culture.Split("-")[0];
        if (ContainsKey(neutralCulture)) return this[neutralCulture];

        if (ContainsKey(DefaultCulture)) return this[DefaultCulture];

        return null;
    }

    public override string ToString()
    {
        return Translate() ?? "???";
    }

    public static implicit operator string(LangStr? l)
    {
        return l?.ToString() ?? "null";
    }

    public static implicit operator LangStr(string s)
    {
        return new(s);
    }
}