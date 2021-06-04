using System;
using System.Threading;

namespace p2pson
{
    class Program
    {
        static void Main(string[] args)
        {
            Kutuphane kh = new Kutuphane();
            double lot = 0;
            double fiyat = 0;
            int nonce = 0;
            int dongu = 100;
            for (int i = 1; i <= dongu; i++)
            {
                if (i % 2 == 1)
                {
                    try
                    {
                        nonce += 1;
                        fiyat = kh.IslemHesaplaFiyat("Alt", 0.0003);
                        lot = kh.IslemHesaplaLot(10, 20);
                        double karsiFiyat = kh.ArzTalepFiyat2("Bid",0,0); //1 işlem sırası,(0 fiyat,1 lot)
                        double karsiLot = kh.ArzTalepFiyat2("Bid",0,1);
                        double karsiFiyat1 = kh.ArzTalepFiyat2("Bid", 1, 0);//2 işlem sırası,(0 fiyat,1 lot)
                        double karsiLot1 = kh.ArzTalepFiyat2("Bid", 1, 1);

                        if (karsiLot <= 0.03 && (karsiFiyat-karsiFiyat1>0.0002) && fiyat==0)
                        {
                            fiyat = karsiFiyat1+0.0001;
                            Console.WriteLine(karsiFiyat1+"-"+fiyat);
                        }

                        string body = kh.Body("buy", lot, fiyat, nonce);
                        if(fiyat==0)
                            Console.WriteLine("Aralik kapandı.");
                        else
                            kh.IslemAc(body);

                    }
                    catch
                    {
                        Console.WriteLine("1. Hata Oluştu..");
                    }
                }
                if (i % 2 == 0)
                {
                    try
                    {
                        nonce += 1;
                        string body = kh.Body("sell", lot, fiyat, nonce);
                        if (fiyat == 0)
                            Console.WriteLine("Aralik kapandı.");
                        else
                            kh.IslemAc(body);

                        var rand = new Random();
                        Thread.Sleep(rand.Next(60, 100) * 1000);
                    }
                    catch
                    {
                        Console.WriteLine("2. Hata Oluştu...");
                    }
                }
                if (i == dongu)
                {
                    Console.WriteLine("İşlem tamamlandı.");
                    nonce = 0;
                    i = 0;
                    //yasinipek
                }
            }
        }
    }
}