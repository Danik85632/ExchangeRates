using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExchangeRates
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var Client = new RestClient("https://www.cbr-xml-daily.ru");
                var request = new RestRequest("https://www.cbr-xml-daily.ru/daily_utf8.xml");
                IRestResponse response = Client.Execute(request);
                XmlSerializer serializer = new XmlSerializer(typeof(ValCurs));
                using (StringReader reader = new StringReader(response.Content))
                {
                    ((ValCurs)serializer.Deserialize(reader)).Valute.ToList().ForEach(x =>
                    {
                        var oneUnit = Convert.ToDouble(x.Value) / Convert.ToDouble(x.Nominal);
                        Console.WriteLine($"За одну единицу {x.CharCode} ({x.Name}) вы получите {oneUnit} рублей");
                    });
                }
                Console.ReadKey();
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (InvalidCastException ex) 
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
