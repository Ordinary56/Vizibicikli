using System.Linq;
using System.Text;
using System.Threading.Channels;

namespace ConVizibicikli
{
    public class Program
    {
        static void Main(string[] args)
        {
            //.ToList() elhagyása esetén:
            //CA1851: Possible multiple enumerations of IEnumerable collection
            List<Kolcsonzes> kolcsonzesek = GetKolcsonzes().ToList();

            //5. feladat
            Console.WriteLine($"5. feladat: Napi kölcsönzések száma: {kolcsonzesek.Count()}");
            //6. feladat
            Console.Write($"6. feladat: Kérek egy nevet: ");
            string name = Console.ReadLine()!;
            Console.WriteLine($"\t{name} kölcsönzései");
            if (kolcsonzesek.Any(x => x.Nev.Equals(name)))
            {
                kolcsonzesek.Where(x => x.Nev.Equals(name))
                    .ToList()
                    .ForEach(x => Console.WriteLine($"\t{x.EOra}:{x.Eperc} - {x.VOra}:{x.Vperc}"));
            }
            else
            {
                Console.WriteLine("\tNem volt ilyen kölcsönző!");
            }

            //7. feladat
            Console.Write("7. feladat: Adjon meg egy időpontot óra:perc ablakban: ");
            string time = Console.ReadLine()!;
            Console.WriteLine("A vizen lévő járművek:");
            if (DateTime.TryParseExact(time, "HH:mm", null, System.Globalization.DateTimeStyles.None,
                out DateTime dt))
            {
                kolcsonzesek.Where(x => DateTime.Compare(new DateTime(DateTime.Now.Year,
                    DateTime.Now.Month, 
                    DateTime.Now.Day,x.EOra,x.Eperc,0),dt) <=0).ToList()
                    .ForEach(x => Console.WriteLine($"{x.EOra}:{x.Eperc} - {x.VOra}:{x.Vperc} : {x.Nev}"));
            }

            //8. feladat
            Console.WriteLine( "8. feladat: a napi bevétel:" + (kolcsonzesek.Sum(x => x.Idohossz()/30D))*2400 + " Ft");
            //9. feladat, debug/bin/obj mappa!!
            using (StreamWriter sw = new("./F.txt"))
            {
                
            }
            //10. feladat
            Console.WriteLine("10. feladat: statisztika: ");
            kolcsonzesek.OrderBy(x => x.JAzon).GroupBy(x => x.JAzon).ToList()
                .ForEach(x => Console.WriteLine($"\t{x.Key} - {x.Count()}")); 

        }
        //Yield return megoldás
        static IEnumerable<Kolcsonzes> GetKolcsonzes()
        {

            using (StreamReader sr = new("./kolcsonzesek.txt", Encoding.UTF8))
            {
                sr.ReadLine()!.Skip(1);
                while (!sr.EndOfStream)
                {
                    string[] lines = sr.ReadLine()!.Split(';');
                    yield return new Kolcsonzes(lines[0],
                        lines[1][0], int.Parse(lines[2]),
                        int.Parse(lines[3]),
                        int.Parse(lines[4]),
                        int.Parse(lines[^1]));
                }
            }
        }
    }

    public class Kolcsonzes
    {
        public Kolcsonzes(string nev, char jAzon, int eOra, int eperc, int vOra, int vperc)
        {
            Nev = nev;
            JAzon = jAzon;
            EOra = eOra;
            Eperc = eperc;
            VOra = vOra;
            Vperc = vperc;
        }

        public string Nev { get; }
        public char JAzon { get; }
        public int EOra { get; }
        public int Eperc { get; }
        public int VOra { get; }
        public int Vperc { get; }
        public int Idohossz()
        {
            return new DateTime(1,1,1,EOra,Eperc,0).Subtract(new DateTime(1,1,1,VOra,Vperc,0)).Minutes;
        }

    }
}