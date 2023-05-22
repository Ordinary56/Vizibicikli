using System.Text;
using System.Threading.Channels;

namespace ConVizibicikli
{
    public class Program
    {
        static void Main(string[] args)
        {
            List<Kolcsonzes> kolcsonzesek = GetKolcsonzes().ToList();
            //5. feladat
            Console.WriteLine($"5. feladat: Napi kölcsönzések száma: {kolcsonzesek.Count()}");
            //6. feladat
            Console.Write($"6. feladat: Kérek egy nevet:");
            string name = Console.ReadLine()!;
            if (kolcsonzesek.Any(x => x.Nev.Equals(name)))
            {
                Console.WriteLine($"{name} kölcsönzései");
                kolcsonzesek.Where(x => x.Nev.Equals(name))
                    .ToList()
                    .ForEach(x => Console.WriteLine($"{x.EOra}:{x.Eperc} - {x.VOra}:{x.Vperc}"));
            }
            else
            {
                Console.WriteLine("Nem volt ilyen kölcsönző!");
            }
            Console.Write("7. feladat: Adjon meg");

        }
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

    }
}