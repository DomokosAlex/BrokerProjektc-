using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankdomokosalexprojekt
{

    //a szamla osztaly
    public class Szamla
    {
        // "Adatbazisnak" 
        private int id;

        //felhasznalo neve
        private string adottnev;
        private string csaladnev;

        //mennyi penz van a szamlan
        private double penzmennyiseg;

        //mikor volt letrehozva a szamla
        private DateTime letrehozasido;

        //a lista tartalmazza a szamlahoz tartozo ertekpapirokat
        private List<Ertekpapir> portfolio;

        //utalashoz
        private int szamlaszam;

        //pin kod
        private int pin;

        public Szamla(int _id, int _szamlaszam, string _csaladnev, string _adottnev, double _penzmennyiseg, DateTime _letrehozasido, List<Ertekpapir> _portfolio, int _pin)
        {
            ID = _id;
            Szamlaszam = _szamlaszam;
            Csaladnev = _csaladnev;
            Adottnev = _adottnev;
            Penzmennyiseg = _penzmennyiseg;
            Letrehoz = _letrehozasido;
            Portfolio = _portfolio;
            Pin = _pin;
        }

        public int ID
        {
            get => id;
            set => id = value;
        }

        public int Szamlaszam
        {
            get => szamlaszam;
            set => szamlaszam = value;
        }

        public string Adottnev
        {
            get => adottnev;
            set => adottnev = value;
        }

        public string Csaladnev
        {
            get => csaladnev;
            set => csaladnev = value;
        }


        public double Penzmennyiseg
        {
            get => penzmennyiseg;
            set => penzmennyiseg = value;
        }

        public DateTime Letrehoz
        {
            get => letrehozasido;
            set => letrehozasido = value;
        }


        public List<Ertekpapir> Portfolio
        {
            get => portfolio;
            set => portfolio = value;
        }

        public int Pin
        {
            get => pin;
            set => pin = value;
        }


        //egyszeru kiiras
        public void Adatok()
        {
            Console.WriteLine("Az ön adatai: ");

            Console.WriteLine($"Neve: {csaladnev} {adottnev}");
            Console.WriteLine($"A szamlán: {penzmennyiseg} Ft van ");
            Console.WriteLine($"A számla: {letrehozasido} volt létrehozva ");
            Console.WriteLine();

            Console.WriteLine("A portfoliója:");
            TozsdePortfolioPrint(portfolio);

        }

        public void Penzfelvetel()
        {
            while (true)
            {
                Console.Write("Mennyit szeretne levenni: ");
                double levesz = Convert.ToDouble(Console.ReadLine());

                if (levesz > 0 && penzmennyiseg >= levesz)
                {
                    penzmennyiseg -= levesz;
                    Console.WriteLine($"A számlán: {penzmennyiseg} Ft van");
                    break;

                }
                else
                {
                    Console.WriteLine("Nincs annyi pénz a számlán");
                }
            }
        }

        public void Befizetes()
        {
            while (true)
            {
                Console.Write("Mennyit szeretne befizetni: ");
                double befizet = Convert.ToDouble(Console.ReadLine());

                if (befizet > 0)
                {
                    penzmennyiseg += befizet;
                    Console.WriteLine($"A számlán: {penzmennyiseg} Ft van");
                    break;
                }
                else
                {
                    Console.WriteLine("Hibás értek");

                }
            }
        }

        public void Utalas(List<Szamla> szamlak)
        {
            while (true)
            {
                Console.Write("Kinek szeretne utalni: ");
                int szamlasz = Convert.ToInt32(Console.ReadLine());

                Console.Write("Mennyit: ");
                double mennyiseg = Convert.ToInt32(Console.ReadLine());

                if (penzmennyiseg >= mennyiseg)
                {
                    bool megtalalta = false;
                    foreach (var i in szamlak)
                    {
                        if (szamlasz == i.Szamlaszam)
                        {
                            megtalalta = true;
                            i.penzmennyiseg += mennyiseg;
                            penzmennyiseg -= mennyiseg;
                            Console.WriteLine($"A számlán: {penzmennyiseg} Ft van ");
                        }

                    }

                    if (!megtalalta)
                    {
                        Console.WriteLine("A számlaszám nem elérhető");

                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Nincs annyi pénz a számlán");
                }
            }
        }

        public void ErtekpapirVasarlas(List<Ertekpapir> tozsde)
        {
            //kiirja a tozsden jelenlevo ertekpapirokat, kerestem hogy kell szep tablazatta formalni
            Console.WriteLine("Tőzsde ");
            TozsdePortfolioPrint(tozsde);

            //mit szeretne vasarolni (neve kell)
            Console.WriteLine();
            Console.Write("Melyiket szeretné vásárolni: ");
            string vas = Console.ReadLine();

            //forintban mennyit, a "Bank" engedi a törtrészvény vasarlast
            Console.WriteLine();
            Console.Write("Mennyit szeretne vásárolni (Ft): ");
            double menpenz = Convert.ToDouble(Console.ReadLine());

            //a tozsden az ertekpapir adatai
            var tozsdep = tozsde.FirstOrDefault(n => n.Nev == vas);

            //portfolion ha mar van abbol
            var portfoliop = portfolio.FirstOrDefault(n => n.Nev == vas);

            if (tozsdep != null)
            {
                if (penzmennyiseg >= menpenz && tozsdep.Mennyiseg >= menpenz / tozsdep.Ar)
                {

                    //mennyit kap az ertekpapirbol (ha vesz 5k ft coca cola reszvenyt akkor 0.5-ot kap)
                    double ertekpapirmenny = menpenz / tozsdep.Ar;
                    //leveszi a penzt
                    penzmennyiseg -= menpenz;

                    //tozsden elveszi a reszvenymennyiseget
                    tozsdep.Mennyiseg -= ertekpapirmenny;
                    if (portfoliop != null)
                    {
                        //hozzaadja a portfoliohoz
                        portfoliop.Mennyiseg += ertekpapirmenny;
                    }
                    else
                    {

                        //ha nincs meg a portfolion akkor hozzaadja
                        portfolio.Add(new Ertekpapir(vas, ertekpapirmenny, tozsdep.Kockazat, tozsdep.Ar));
                    }

                }
                else
                {
                    Console.WriteLine("Nincs elég pénz a számlán");
                }
            }
            else
            {
                Console.WriteLine($"Az értékpapir nincs a tőzsdén vagy a tőzsdén nincs annyi értékpapir");
            }
            Console.WriteLine("A portfoliója:");
            TozsdePortfolioPrint(portfolio);

        }

        public void ErtekpapirEladas(List<Ertekpapir> tozsde)
        {
            if (portfolio.Count > 0)
            {
                Console.WriteLine("A portfoliója:");
                TozsdePortfolioPrint(portfolio);

                //melyik reszvenyt szeretne eladni
                Console.Write("Melyiket szeretné eladni: ");
                string elad = Console.ReadLine();

                //mennyit (nem forintban)
                Console.Write("Mennyit szeretne eladni (egység): ");
                double men = Convert.ToDouble(Console.ReadLine());

                var tozsdep = tozsde.FirstOrDefault(n => n.Nev == elad);
                var portfoliop = portfolio.FirstOrDefault(n => n.Nev == elad);

                //ha a portfolioban nincs olyan ertekpapir nem tudja eladni
                if (portfoliop != null)
                {
                    //ha eltudja adni azt a mennyiseget amit el akar adni 
                    if (portfoliop.Mennyiseg >= men)
                    {

                        double penzmeny = men * tozsdep.Ar;
                        penzmennyiseg += penzmeny;

                        tozsdep.Mennyiseg += men;
                        portfoliop.Mennyiseg -= men;

                        //ha az osszeset eladta akkor torolje a portfoliobol
                        if (portfoliop.Mennyiseg == 0)
                        {
                            portfolio.Remove(portfoliop);
                        }

                    }
                    else
                    {
                        Console.WriteLine("Annyit nem tud eladni!");
                    }
                }
                else
                {
                    Console.WriteLine("A portfolióban nincs ilyen értékpapir!");
                }

                Console.WriteLine("A portfoliója:");
                TozsdePortfolioPrint(portfolio);

            }
            else
            {
                Console.WriteLine("A portfolió üres!");
            }
        }

        // portfolio elemzes (kiegyensulyozott, rizikos, ok)

        public void PortfolioElemzes()
        {

            //megszamolja melyik tipusbol hany van
            var rizikos = portfolio.Count(n => n.Kockazat == "Magas");
            var kozepes = portfolio.Count(n => n.Kockazat == "Kozepes");
            var stabil = portfolio.Count(n => n.Kockazat == "Alacsony");

            //portfolio erteke
            double ossz = portfolio.Sum(n => n.Mennyiseg * n.Ar);

            // ellenorzi, hogy a portfolio kiegyensulyozott-e, azaz nem koncentrelodik tul sok penz 1 ertekpapirba
            // ha egy ertekpapir aranya tul nagy az osszertekhez kepest, akkor diverzifikalni kell
            // szabalyok:
            //   - magas kockazatu (pl. kripto) papír: max 30%
            //   - kozepes kockazatu: max 40%
            //   - alacsony kockazatu (stabil) papír: max 50% (inkabb ETF-be kell tenni a penzt mert az a legbiztosagosabb, vagy aranyba)
            // Ez tukrozi a "Ne tedd az összes tojásodat egy kosárba" elvet

            var kiegyensulyozatlan = portfolio.Any(n =>
            (n.Kockazat == "Magas" && (n.Mennyiseg * n.Ar) / ossz > 0.3) ||

            (n.Kockazat == "Kozepes" && (n.Mennyiseg * n.Ar) / ossz > 0.4) ||

            (n.Kockazat == "Alacsony" && (n.Mennyiseg * n.Ar) / ossz > 0.5)


            );

            string rizikoStatus = "";

            if (rizikos >= kozepes && rizikos >= stabil)
            {
                rizikoStatus = "Magas";
            }
            else if (kozepes >= rizikos && kozepes >= stabil)
            {
                rizikoStatus = "Közepes";
            }
            else
            {
                rizikoStatus = "Alacsony";
            }

            string sulyStatus = kiegyensulyozatlan ? "nem kiegyensúlyozott (Diverzifikálni kell)" : "kiegyensúlyozott";

            Console.WriteLine($"Az ön portfoliójának kockázata {rizikoStatus} és {sulyStatus}");
            Console.WriteLine();

            Console.WriteLine("A portfoliója:");
            TozsdePortfolioPrint(portfolio);

        }

        public void TozsdePortfolioPrint(List<Ertekpapir> Kiir)
        {
            if (Kiir.Count > 0)
            {

                Console.WriteLine("{0,-15} {1,10} {2,10} {3,15}", "Név", "Mennyiség", "Kockázat", "Ár");
                foreach (var i in Kiir)
                {
                    Console.WriteLine("{0,-15} {1,10} {2,10} {3,15:N0}", i.Nev, i.Mennyiseg, i.Kockazat, i.Ar);
                }
            }
            else
            {
                Console.WriteLine("A portfolió üres!");
            }

        }

    }
}
