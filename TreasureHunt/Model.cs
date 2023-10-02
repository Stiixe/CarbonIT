using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TreasureHunt
{
    public class MapElement
    {
        protected int positionX;
        protected int positionY;
        private string symbol = ".";
        private bool isObstacle = false;
        protected int movementCount = 0;

        public int PositionX { get { return positionX; } }
        public int PositionY { get { return positionY; } }
        public string Symbol { get { return symbol; } }
        public bool IsObstacle { get { return isObstacle; } }

        public MapElement(int positionX, int positionY, string symbol = ".", bool isObstacle = false)
        {
            this.positionX = positionX;
            this.positionY = positionY;
            this.symbol = symbol;
            this.isObstacle = isObstacle;
            this.movementCount = 0;
        }

        override public string ToString()
        {
            return symbol;
        }
    }

    public class Mountain : MapElement 
    {
        public Mountain(int positionX, int positionY)
            : base(positionX, positionY, "M", true)
        { }
    }

    public class Treasure : MapElement
    {
        private int count = 0;
        public int Count { get { return count; } }

        public Treasure(int positionX, int positionY, int count)
            : base(positionX, positionY, "T", false)
        {
            this.count = count;
        }

        public bool Collect()
        {
            if (count > 0)
            {
                count--;
                return true;
            }
            return false;
        }

        override public string ToString()
        {
            return $"{Symbol}({count})";
        }
    }

    public class Adventurer : MapElement
    {
        Dictionary<int, string> ORIENTATION_ANGLES = new Dictionary<int, string>
        {
            { 0, "N"},
            { 90, "E" },
            { 180, "S" },
            { 270, "O" },
            { -90, "O" },
            { -180, "S" },
            { -270, "E" }
        };

        private readonly string name;
        private string orientation;
        private int orientationAngle = 0;
        private readonly string movementString;
        private int treasureCount = 0;
        private MapElement elementUnder;

        public string Name { get { return name; } }
        public string Orientation { get { return orientation; } }
        public string MovementString { get { return movementString; } }
        public char NextMove { get { return MovementString[movementCount]; } }
        public bool HasMovementLeft { get { return movementCount < movementString.Length; } }
        public int TreasureCount {  get { return treasureCount; } }

        public Adventurer(string name, int positionX, int positionY, string orientation, string movementString)
            : base(positionX, positionY, "A", true)
        {

            this.name = name;
            this.orientation = orientation;
            this.movementString = movementString;
            this.treasureCount = 0;
            orientationAngle = ORIENTATION_ANGLES.First(kvp => kvp.Value == orientation && kvp.Key >= 0).Key;
            elementUnder = new MapElement(positionX, positionY);
        }

        public void Rotate(bool rotateRight)
        {
            int directionFactor = rotateRight ? 1 : -1;
            orientationAngle = (orientationAngle + 90 * directionFactor) % 360;
            orientation = ORIENTATION_ANGLES[orientationAngle];
            movementCount++;
        }

        public void GetNextMovement(out int x, out int y)
        {
            x = PositionX;
            y = PositionY;

            switch (orientation)
            {
                case "N":
                    y -= 1;
                    break;
                case "S":
                    y += 1;
                    break;
                case "E":
                    x += 1;
                    break;
                case "O":
                    x -= 1;
                    break;
            }
        }

        public MapElement? Move(MapElement? nextElementUnder)
        {
            movementCount++;

            if (nextElementUnder == null)
                return null;

            this.positionX = nextElementUnder.PositionX;
            this.positionY = nextElementUnder.PositionY;
            MapElement previousElementUnder = this.elementUnder;
            this.elementUnder = nextElementUnder;
            return previousElementUnder;
        }

        public void GetTreasure()
        {
            treasureCount++;
        }

        override public string ToString()
        {
            return $"{Symbol}({name})";
        }
    }

    public class Map
    {
        private int sizeX = 0;
        private int sizeY = 0;
        private MapElement[,] elements;

        public int SizeX { get { return sizeX; } }
        public int SizeY { get { return sizeY; } }
        public MapElement[,] Elements { get { return elements; } }
        
        public Map(int sizeX, int sizeY)
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.elements = new MapElement[sizeX, sizeY];
            InitMap(sizeX, sizeY);
        }

        public MapElement? GetElement(int positionX, int positionY)
        {
            if (positionX < 0 || positionY < 0
                 || positionX >= sizeX || positionY >= sizeY)
                return null; 
            
            return elements[positionX, positionY];
        }

        public void FillMap(MapElement mapElement)
        {
            elements[mapElement.PositionX, mapElement.PositionY] = mapElement;
        }

        private void InitMap(int sizeX, int sizeY)
        {
            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    elements[x, y] = new MapElement(x, y);
                }
            }
        }

        override public string ToString()
        {
            StringBuilder builder = new StringBuilder();
            for (int y = 0; y < sizeY; y++)
            {
                IEnumerable<MapElement> row = Enumerable.Range(0, sizeX).Select(x => elements[x, y]);
                builder.AppendLine(string.Join('\t', row));
            }
            return builder.ToString().Replace("\t", "\t\t\t");
        }
    }
}
