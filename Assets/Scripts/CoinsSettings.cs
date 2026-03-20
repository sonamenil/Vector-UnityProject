using System.Xml;

public class CoinsSettings
{
    public int nominal;

    public float chance;

    public int count;

    public CoinsSettings(XmlNode node)
    {
        nominal = XmlUtils.ParseInt(node.Attributes["Nominal"]);
        chance = XmlUtils.ParseFloat(node.Attributes["Chance"]);
        count = XmlUtils.ParseInt(node.Attributes["Count"]);
    }

    public CoinsSettings(float chance, int nominal, int count)
    {
        this.nominal = nominal;
        this.chance = chance;
        this.count = count;
    }
}
