namespace HomeHub.Adapters.Lifx
{
    using System.Xml.Linq;

    /// <summary>
    /// The lifx meta data.
    /// </summary>
    internal class LifxMetaData
    {
        public LifxMetaData(string id, string uuid)
        {
            this.Id = id;
            this.UUID = uuid;
        }

        public string Id { get; private set; }

        public string UUID { get; private set; }

        public string GetXmlString()
        {
            return new XDocument(new XElement("root", new XElement("id", this.Id), new XElement("uuid", this.UUID))).ToString();
        }

        public static LifxMetaData FromString(string xml)
        {
            var doc = XDocument.Parse(xml).Element("root");

            var id = doc.Element("id");
            var uuid = doc.Element("uuid");

            return new LifxMetaData(id.Value, uuid.Value);
        }
    }
}
