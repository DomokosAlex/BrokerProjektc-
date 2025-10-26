using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankdomokosalexprojekt
{

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

}
