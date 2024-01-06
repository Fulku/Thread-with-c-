using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    private const int BOYUT = 1000000;
    private const int THREAD_SAYISI = 4;

    private static List<int> sayilar = new List<int>(BOYUT);
    private static List<int> ciftSayilar = new List<int>();
    private static List<int> tekSayilar = new List<int>();
    private static List<int> asalSayilar = new List<int>();

    private static readonly object kilitlemeNesnesi = new object();

    static void Main()
    {
        ListeyiHazirla();

        Thread[] threadler = new Thread[THREAD_SAYISI];

        for (int i = 0; i < THREAD_SAYISI; i++)
        {
            int threadIndex = i;
            threadler[i] = new Thread(() => SayilariIsle(threadIndex));
            threadler[i].Start();
        }

        foreach (Thread thread in threadler)
        {
            thread.Join();
        }

        Console.WriteLine("Çift Sayılar: " + string.Join(", ", ciftSayilar));
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Tek Sayılar: " + string.Join(", ", tekSayilar));
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Asal Sayılar: " + string.Join(", ", asalSayilar));
        Console.WriteLine();
        Console.WriteLine();

    }

    private static void ListeyiHazirla()
    {
        for (int i = 1; i <= BOYUT; i++)
        {
            sayilar.Add(i);
        }
    }

    private static void SayilariIsle(int threadIndex)
    {
        int baslangic = threadIndex * (BOYUT / THREAD_SAYISI);
        int bitis = (threadIndex + 1) * (BOYUT / THREAD_SAYISI);

        for (int i = baslangic; i < bitis; i++)
        {
            int sayi = sayilar[i];

            if (sayi % 2 == 0)
            {
                lock (kilitlemeNesnesi)
                {
                    ciftSayilar.Add(sayi);
                }
            }
            else
            {
                lock (kilitlemeNesnesi)
                {
                    tekSayilar.Add(sayi);
                }
            }

            if (AsalSayiMi(sayi))
            {
                lock (kilitlemeNesnesi)
                {
                    asalSayilar.Add(sayi);
                }
            }
        }
    }

    private static bool AsalSayiMi(int sayi)
    {
        if (sayi <= 1)
        {
            return false;
        }

        for (int i = 2; i <= Math.Sqrt(sayi); i++)
        {
            if (sayi % i == 0)
            {
                return false;
            }
        }

        return true;
    }
}
