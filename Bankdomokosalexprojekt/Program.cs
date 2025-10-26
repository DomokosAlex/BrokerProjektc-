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
                    

                    //mit akar csinalni a felhasznalo
                    Console.WriteLine("1: Adatok Kiirasa");
                    Console.WriteLine("2: Penzfelvetel");
                    Console.WriteLine("3: Befizetes");
                    Console.WriteLine("4: Utalas");
                    Console.WriteLine("5: Ertekpapirvasarlas");
                    Console.WriteLine("6: Ertekpapireladas");
                    Console.WriteLine("7: Portfolio elemzes");
                    Console.WriteLine("8: Kilepes");

                    Console.Write("Mit szeretne csinálni: ");
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


}