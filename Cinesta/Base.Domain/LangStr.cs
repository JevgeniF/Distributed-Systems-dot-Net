namespace Base.Domain;

public class LangStr : Dictionary<string, string>
{
    private const string DefaultCulture = "en";

    public new string this[string key]
    {
        get {return base[key]; }
        set { base[key] = value; }
    }

    public LangStr(string value) : this(value, Thread.CurrentThread.CurrentUICulture.Name)
    {
    }

    public LangStr()
    {
    }

    public LangStr(string value, string culture)
    {
        this[culture] = value;
    }

    public void SetTranslation(string value)
    {
        this[Thread.CurrentThread.CurrentUICulture.Name] = value;
    }

    public string? Translate(string? culture = null)
    {
        // if there is exact match
        if (this.Count == 0) return null;
        culture = culture?.Trim() ?? Thread.CurrentThread.CurrentUICulture.Name;

        if (this.ContainsKey(culture))
        {
            return this[culture];
        }
        
        // if there is match without region
        var key = this.Keys.FirstOrDefault(s => culture.StartsWith(s));
        if (key != null)
        {
            return this[key];
        }

        //try to find default culture
        key = this.Keys.FirstOrDefault(s => culture.StartsWith(DefaultCulture));
        if (key != null)
        {
            return this[key];
        }

        // return first in list or null
        return null;
    }

    public override string ToString()
    {
        return Translate() ?? "????";
    }
    
    public static implicit operator string(LangStr? l) => l?.ToString() ?? "null";
    public static implicit operator LangStr(string s) => new LangStr(s);
}