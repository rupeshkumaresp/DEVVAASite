using System.Xml;
using VAA.BusinessComponents.ChiliWebService;

namespace VAA.BusinessComponents
{
    public class ChiliIntegration
    {
        public string Environment { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string APIKey { get; set; }
        public mainSoapClient WebService { get; set; }

        public ChiliIntegration()
        {
            WebService = new mainSoapClient();
        }

        public void Connect(string environment)
        {
            Connect(environment, "WebServices", "C@ps1cum123ws");
        }

        public void Connect(string environment, string username, string password)
        {
            if (environment == Environment && username == Username && password == Password) return; // Don't reconnect if not needed
            this.Environment = environment;
            this.Username = username;
            this.Password = password;
            var generateAPIKeyResponse = new XmlDocument();
            generateAPIKeyResponse.LoadXml(WebService.GenerateApiKey(environment, "TestUser", "TestUser"));
            if (generateAPIKeyResponse.DocumentElement.GetAttribute("succeeded") == "true")
            {
                APIKey = generateAPIKeyResponse.DocumentElement.GetAttribute("key");

                //WebService.SetWorkingEnvironment(APIKey, environment);
            }
        }

    }
}
