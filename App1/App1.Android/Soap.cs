using RestSharp;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Xamarin.Forms;

namespace App1
{
    class Soap
    {

        public async Task<string> SoapRequest(string xmlIn, CancellationToken token = default)
        {

            for (int i = 0; i < 2; i++)
            {

                try
                {
                    if (i == 0)
                    {
                        var client = new RestClient("http://185.35.128.7:34000");
                        client.Timeout = -1;
                        var request = new RestRequest(Method.POST);
                        request.AddHeader("Content-Type", "application/xml");
                        var body = @"<?xml version=""1.0"" encoding=""UTF-8""?>
                                    " + "\n" +
                                     @"<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns1=""urn:api3"">
                                    " + "\n" +
                                     @"<SOAP-ENV:Body>
                                    " + "\n" +
                                    @"<ns1:Login>
                                    " + "\n" +
                                    @"<login>конфиденциальная информация</login>
                                    " + "\n" +
                                    @"<pass>конфиденциальная информация</pass>
                                    " + "\n" +
                                    @"</ns1:Login>
                                    " + "\n" +
                                    @"</SOAP-ENV:Body>
                                    " + "\n" +
                                    @"</SOAP-ENV:Envelope>";
                        request.AddParameter("application/xml", body, ParameterType.RequestBody);
                        IRestResponse response = client.Execute(request);
                    }

                    if (i == 1)
                    {
                        var client = new RestClient("http://185.35.128.7:34000");

                        client.Timeout = -1;

                        var request = new RestRequest(Method.POST);

                        request.AddHeader("Content-Type", "application/xml");

                        var body = @"<?xml version=""1.0"" encoding=""UTF-8""?>
                                    " + "\n" +
                                    @"<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns1=""urn:api3"">
                                    " + "\n" +
                                    @"<SOAP-ENV:Body>
                                    " + "\n" + xmlIn +
                                    "@ + \n" +
                                    @"</SOAP-ENV:Body>
                                    " + "\n" +
                                    @"</SOAP-ENV:Envelope>";
                        request.AddParameter("application/xml", body, ParameterType.RequestBody);
                        IRestResponse response = client.Execute(request);
                        return response.Content;
                    }
                }
                catch (WebException e)
                {
                    Console.WriteLine(e);
                    return "Не удалось обновить. Повторите попытку.";
                }
            }
            return "Не удалось обновить. Повторите попытку.";
        }
        public String[] xmlDocument(String _result, String whatFound)
        {
            try
            {
                XmlDocument line = new XmlDocument();

                line.LoadXml(_result);

                XmlNodeList listNode = line.GetElementsByTagName(whatFound);

                XmlNode nodeList;

                String[] list = new string[listNode.Count];

                if (listNode.Count > 1)
                {
                    for (int i = 0; i < listNode.Count; i++)
                    {
                        nodeList = listNode.Item(i);

                        list[i] = nodeList.InnerText;
                    }
                }
                else if (listNode.Count == 1)
                {
                    nodeList = listNode.Item(0);

                    list[0] = nodeList.InnerText;
                }
                else
                {
                    return null;
                }
                return list;
            }
            catch (XmlException e)
            {
                return null;
            }
        }
        public StackLayout LabelForm(StackLayout stack)
        {
            Label label = new Label
            {
                Text = "Не удалось обновить. Повторите попытку.",

                TextColor = Color.Black,

                FontFamily = "akzi",

                FontSize = 15,

                HorizontalOptions = LayoutOptions.CenterAndExpand,

                HorizontalTextAlignment = TextAlignment.Center
            };
            stack.Children.Add(label); ;

            return stack;
        }
        public StackLayout Label(StackLayout stack)
        {
            Label label = new Label
            {
                Text = "Отсутствует подключение к интернету.",

                TextColor = Color.Black,

                FontFamily = "akzi",

                FontSize = 15,

                HorizontalOptions = LayoutOptions.CenterAndExpand,

                HorizontalTextAlignment = TextAlignment.Center
            };
            stack.Children.Add(label); ;

            return stack;
        }
    }
}