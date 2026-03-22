using System.Xml;

public class CoinsSettings
{
    public int nominal;

    public float chance;

    public int count;

    public CoinsSettings(XmlNode node)
    {
        nominal = node.Attributes["Nominal"].ParseInt();
        chance = node.Attributes["Chance"].ParseFloat();
        count = node.Attributes["Count"].ParseInt();
    }

    public CoinsSettings(float chance, int nominal, int count)
    {
        this.nominal = nominal;
        this.chance = chance;
        this.count = count;
    }
}
