using System.Reflection;

interface IFileable {
    public string Save();
    public void Load(string to_load);
    public string GetToken();
}

interface IMenuItem
{
    public string GetName();
    public void RunOption();
}