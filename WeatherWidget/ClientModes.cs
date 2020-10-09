using RestSharp;

namespace WeatherWidget
{
    interface IClientMode
    {
        IDataParser DataParser { get; }

        DataFormat DataFormat { get; }

        string Mode { get; }
    }

    class XmlMode : IClientMode
    {
        public IDataParser DataParser => new XmlParser();

        public DataFormat DataFormat => DataFormat.Xml;

        public string Mode => "xml";
    }

    class JsonMode : IClientMode
    {
        public IDataParser DataParser => new JsonParser();

        public DataFormat DataFormat => DataFormat.Json;

        public string Mode => "json";
    }
}
