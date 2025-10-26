using System.ComponentModel.Design;
using System.Net.NetworkInformation;
using System.Threading.Channels;

namespace Bankdomokosalexprojekt
{
    internal class Program
    {
        public static List<Szamla> szamlak = new List<Szamla>();
        public static List<Ertekpapir> tozsde = new List<Ertekpapir>() 
        {
            new Ertekpapir("SP500 ETF", 250000000.0, "Alacsony", 50000.0),
            new Ertekpapir("Coca Cola", 200000000.0, "Kozepes", 10000.0),
            new Ertekpapir("Mastercard", 150000000.0, "Kozepes", 10000.0),
            new Ertekpapir("Arany", 250000000.0, "Alacsony", 20000.0),
            new Ertekpapir("Bitcoin", 50000000.0, "Magas", 63_000_000.0),
            new Ertekpapir("Ethereum", 350000000.0, "Magas", 12000000.0)
        };
        static void Main(string[] args)
        {

            Szamla elso = new Szamla(1, 123, "Domokos", "Alex", 500000, DateTime.Now, new List<Ertekpapir> {
                new Ertekpapir("SP500 ETF", 5, "Alacsony", 50_000),
                new Ertekpapir("Coca Cola", 3, "Kozepes", 10_000),
                new Ertekpapir("Mastercard", 4, "Kozepes", 10_000),
                new Ertekpapir("Arany", 4, "Alacsony", 20_000),
                
            }, 2345);

            Szamla masodik = new Szamla(2, 345, "Gazdag", "Gábor", 5000000, DateTime.Now, new List<Ertekpapir> {
                new Ertekpapir("Ethereum", 2, "Magas", 12_000_000),
                new Ertekpapir("Bitcoin", 1, "Magas", 63_000_000),
                new Ertekpapir("SP500 ETF", 1, "Alacsony", 50_000),
            }, 5677);

            Szamla harmadik = new Szamla(3, 867, "Szegény", "Szandi", 10000, DateTime.Now, new List<Ertekpapir> {
                
            }, 2002);

            Szamla negyedik = new Szamla(4, 465, "Közép", "Pista", 250000, DateTime.Now, new List<Ertekpapir> {
                new Ertekpapir("Arany", 2, "Alacsony", 20_000),
                new Ertekpapir("Ethereum", 0.01, "Magas", 12_000_000)
            },2000);

            Szamla otodik = new Szamla(5, 999, "Kiss", "Pista", 350000, DateTime.Now, new List<Ertekpapir> {

            }, 2025);

            szamlak.Add(elso);
            szamlak.Add(masodik);
            szamlak.Add(harmadik);
            szamlak.Add(negyedik);
            szamlak.Add(otodik);

            //A projekt egy egyzseru bank rendszer
            Console.WriteLine("Üdvözölöm a BrokerBankban!");
        
            while (true)
            {

                //pin -> ahoz a peldanyhoz tud hozzaferni amelyike a pin
                Console.Write("Kérem adja meg a PIN kódot: ");
                int pin = Convert.ToInt32(Console.ReadLine());

                //az akutalis peldany, a firstordefault a konkret elemre mutat a listaban es konnyu valtoztatni
                var szamla = szamlak.FirstOrDefault(x => x.Pin == pin);
                bool kilep = false;

                //ha nem talal olyan szamlat aminek az a pinje akkor olyan szamla nem letezik
                if (szamla == null)
                {
                    Console.WriteLine("Hibás PIN kód!");

                }
                else
                {
                    Console.WriteLine();
                    Console.Write("Mit szeretne csinálni: ");
                    Console.WriteLine();

                    //mit akar csinalni a felhasznalo
                    Console.WriteLine("1: Adatok Kiirasa");
                    Console.WriteLine("2: Penzfelvetel");
                    Console.WriteLine("3: Befizetes");
                    Console.WriteLine("4: Utalas");
                    Console.WriteLine("5: Ertekpapirvasarlas");
                    Console.WriteLine("6: Ertekpapireladas");
                    Console.WriteLine("7: Portfolio elemzes");
                    Console.WriteLine("8: Kilepes");

                    int tev = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine();

                    switch (tev)
                    {

                        //egyserzu adatok kiirasa
                        case 1:
                            {
                                szamla.Adatok();
                                break;
                            }
                        case 2:
                            {
                                //a felhasznalo "kivesz" penzt a szamlabol
                                szamla.Penzfelvetel();
                                break;
                            }
                        case 3:
                            {
                                //a felhasznalo "visszatesz" penzt a szamlabol
                                szamla.Befizetes();
                                break;
                            }
                        case 4:
                            {
                                //a felhasznalo egy masik szamlara utal penzt
                                szamla.Utalas(szamlak);
                                break;
                            }
                        case 5:
                            {
                                //a portfoliohoz ertekpapirt ad hozza ha megtudja venni
                                szamla.ErtekpapirVasarlas(tozsde);
                                break;
                            }
                        case 6:
                            {
                                //elad ertekpapirt
                                szamla.ErtekpapirEladas(tozsde);
                                break;
                            }
                        case 7:
                            {

                                //a felhasznalo kap egy egyszeru portfolioelemzest
                                szamla.PortfolioElemzes();
                                break;
                            }
                        case 8:
                            {
                                //kilepes
                                kilep = true;
                                break;
                            }
                        default: 
                            {
                                Console.WriteLine("Hiba");
                                break;
                            }
                    }

                    if (kilep)
                    {
                        Console.WriteLine("Viszlat!");
                        break;
                    }
                
                }
            }
            
        }
    }

    //Az ertekpapiroknak irtam egy kulon osztalyt hogy konnyu legyen oket kezelni 
    public class Ertekpapir
    {
        //a neve az ertekpapirnak
        private string nev;

        //mennyi van a felhasznalo szamlajan belole (vagy a tozsden mennyi elerheto)
        private double mennyiseg;

        // megmondja hogy az ertekpapir milyen kategoriaba tartozik
        // magas = veszelyes, gyorsan valtozik az ara (bitcoin, ethereum)
        // kozepes = reszveny, biztonsagosabb mint a crypto, de veszelyes lehet (mastercard, coca cola)
        // alacsony = az arfolyama lassan, ritkan valtozik (ETF, arany)
        private string kockazat;

        //1db ertekpapir ara 
        private double ar;

        public string Nev
        {
            get => nev; 
            set => nev = value;
        }

        public double Mennyiseg
        {
            get => mennyiseg;
            set => mennyiseg = value;
        }

        public string Kockazat
        {
            get => kockazat;
            set => kockazat = value;
        }

        public double Ar
        {
            get => ar;
            set => ar = value;
        }

        //konstruktor
        public Ertekpapir(string nev, double mennyiseg, string kockazat, double ar)
        {
            Nev = nev;
            Mennyiseg = mennyiseg;
            Kockazat = kockazat;
            Ar = ar;
        }

    }

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

        public Szamla(int _id, int  _szamlaszam, string _csaladnev, string _adottnev, double _penzmennyiseg, DateTime _letrehozasido, List<Ertekpapir> _portfolio, int _pin)
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

        public void Penzfelvetel ()
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
                    if(portfoliop.Mennyiseg >= men)
                    {
                        
                        double penzmeny = men * tozsdep.Ar;
                        penzmennyiseg += penzmeny;

                        tozsdep.Mennyiseg += men;
                        portfoliop.Mennyiseg -= men;

                        //ha az osszeset eladta akkor torolje a portfoliobol
                        if(portfoliop.Mennyiseg == 0)
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