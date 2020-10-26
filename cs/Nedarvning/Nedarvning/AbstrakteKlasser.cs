using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nedarvning
{
    abstract class Shape
    {
        public abstract int GetArea();
    }

    class Square : Shape
    {
        int side;

        public Square(int n) => side = n;

        // GetArea method is required to avoid a compile-time error.
        public override int GetArea() => side * side;
    }
    // Output: Area of the square = 144
    class AbstrakteKlasser
    {
        //https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/abstract
        public AbstrakteKlasser()
        {
            var sq = new Square(12);
            Console.WriteLine($"Area of the square = {sq.GetArea()}");
        }
    }
}
