namespace NerScript.RiValuer
{
    public enum RiValuerNodeType
    {
        None = 0,
        Provider = 1000,
        Demander = 2000,
        Add = 3000,
        Multiple = 4000,

        Resource = 10000,
        Convert = 11000,
    }
    public enum RiValuerNodeTypeInfo
    {
        None = 0,
        Resourceable = 100,
        Root = 200,
    }
}