using Newtonsoft.Json;
using RestSharp;
using System;
using System.Security.Cryptography;
using System.Text;

namespace p2pson
{
    public class Kutuphane
    {
        public double IslemHesaplaFiyat(string altUst, double aralikDeger)
        {
            double altIslemFiyati = Math.Round(ArzTalepFiyat2("Bid",0,0),4);
            double ustIslemFiyati = Math.Round(ArzTalepFiyat2("Ask",0,0),4);
            double aralik = Math.Round(ustIslemFiyati - altIslemFiyati,4);
            var rand = new Random();
            int aralikint = Convert.ToInt32(ustIslemFiyati - altIslemFiyati);
            double rastgeleAralik = 0;
            if (altUst == "Alt")
                rastgeleAralik = Math.Round(altIslemFiyati + ((rand.Next(0, aralikint + 1) + rand.NextDouble()) % aralik),4);
            if (altUst == "Ust")
                rastgeleAralik = Math.Round(ustIslemFiyati - ((rand.Next(0, aralikint + 1) + rand.NextDouble()) % aralik),4);
            Console.WriteLine("Alt İşlem Fiyatı: " + altIslemFiyati + " Üst İşlem Fiyatı: " + ustIslemFiyati + " Aralık: " + aralik);
            Console.WriteLine("İşlem Fiyatı: " + rastgeleAralik);
            double hesaplaFiyat = rastgeleAralik;
            if (aralik <= aralikDeger)
                hesaplaFiyat = 0;
            if (aralik > aralikDeger && rastgeleAralik>altIslemFiyati && rastgeleAralik<ustIslemFiyati)
                hesaplaFiyat = rastgeleAralik;
            return hesaplaFiyat;
        }
        public double IslemHesaplaLot(int enAzLot, int enFazlaLot)
        {
            var rand = new Random();
            double rastgeleLot = Math.Round((rand.Next(enAzLot, enFazlaLot)) + (rand.NextDouble()),2);
            Console.WriteLine("İşlem Miktarı: " + rastgeleLot);
            return rastgeleLot;
        }
        public string Body(string islemYonu, double amount, double price, int nonce)
        {
            return JsonConvert.SerializeObject(new Body()
            {
                market = "NVX_USDT",
                side = islemYonu,
                amount = amount.ToString().Replace(',', '.'),
                price = price.ToString().Replace(',', '.'),
                request = "/api/v2/order/new",
                nonce = nonce.ToString()
            });
        }
        public void IslemAc(string body)
        {
            string apikey = "ca649cff5a9b964e2a4c591b2a5927a8";
            string secretkey = "834bcce82d75c5b3850858719a42328c";
            var client = new RestClient("https://api.p2pb2b.io/api/v2/order/new");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("X-TXC-APIKEY", apikey);
            request.AddHeader("X-TXC-PAYLOAD", Base64Encode(body));
            request.AddHeader("X-TXC-SIGNATURE", SHA512_ComputeHash(Base64Encode(body), secretkey));
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
        }
        public double ArzTalepFiyat(string askBid)
        {
            var client = new RestClient("http://api.p2pb2b.io/api/v2/public/ticker?market=NVX_USDT");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            ArzTalep arzTalep = JsonConvert.DeserializeObject<ArzTalep>(response.Content);
            double fiyat = 0;
            if (askBid == "Ask")
                fiyat = Convert.ToDouble(arzTalep.result.ask);
            if (askBid == "Bid")
                fiyat = Convert.ToDouble(arzTalep.result.bid);
            return fiyat;
        }
        public double ArzTalepFiyat2(string askBid, int deger1, int deger2)
        {
            var client = new RestClient("https://api.p2pb2b.io/api/v2/public/depth/result?market=NVX_USDT&limit=100");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            ArzTalep2 arzTalep2 = JsonConvert.DeserializeObject<ArzTalep2>(response.Content);
            double fiyat = 0;
            if (askBid == "Ask")
            {
                string fiyatYazi = arzTalep2.result.asks[deger1][deger2].Replace('.', ',');
                fiyat = Convert.ToDouble(fiyatYazi);
            }
            if (askBid == "Bid")
            {
                string fiyatYazi = arzTalep2.result.bids[deger1][deger2].Replace('.', ',');
                fiyat = Convert.ToDouble(fiyatYazi);
            }
            return fiyat;
        }
        public string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
        public string SHA512_ComputeHash(string text, string secretKey)
        {
            var hash = new StringBuilder(); ;
            byte[] secretkeyBytes = Encoding.UTF8.GetBytes(secretKey);
            byte[] inputBytes = Encoding.UTF8.GetBytes(text);
            using (var hmac = new HMACSHA512(secretkeyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }
            return hash.ToString();
        }
    }
}