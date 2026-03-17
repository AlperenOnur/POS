using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schiffe_versenken_selber.Models
{
    public class Ship
    {
        public int Size { get; }
        public List<Cell> Cells { get; } = new();
        public bool IsSunk => Cells.All(c => c.State == CellState.Hit || c.State == CellState.Sunk);
        public Ship(int size) { Size = size; }
    }
}
