using System;
using System.Text;
using System.Xml.Linq;

namespace TreasureHunt
{
    public class Controller
    {

        bool isGraphic = false;

        internal View View { get; set; }

        private Map map;
        public Map Map { get { return map; } }


        public Controller(bool isVisual) 
        {
            View = new View();
            this.isGraphic = isVisual;
        }

        public void Initialize(string fileName)
        {
            List<string> lstInstructions = View.Read(fileName);
            if (lstInstructions.Count == 0)
            {
                Console.WriteLine("File empty.");
                return;
            }

            foreach (string instruction in lstInstructions)
            {
                if (instruction.StartsWith("C") && map == null)
                    map = CreateMap(instruction);
                else
                    map.FillMap(CreateMapElement(instruction));
            }
        }

        private static string[] ParseInstruction(string instruction)
        {
            if (string.IsNullOrWhiteSpace(instruction))
                throw new ArgumentException($"Instruction is empty: {instruction}");

            char firstChar = instruction[0];

            if (!Constants.AUTHORIZED_ELEMENTS.Contains(firstChar))
                throw new ArgumentException($"Instruction is invalid: {instruction}");

            string[] parts = instruction.Split(new char[] { '-' }).Select(instr => instr.Trim()).ToArray();

            switch (firstChar)
            {
                case 'C':
                case 'M':
                    if (parts.Length != 3 
                        || !int.TryParse(parts[1], out int _)
                        || !int.TryParse(parts[2], out int _))
                        throw new ArgumentException($"Instruction is not well formatted: {instruction}");
                    break;

                case 'T':
                    if (parts.Length != 4
                        || !int.TryParse(parts[1], out int _)
                        || !int.TryParse(parts[2], out int _)
                        || !int.TryParse(parts[3], out int _))
                        throw new ArgumentException($"Instruction is not well formatted: {instruction}");
                    break;

                case 'A':
                    if (parts.Length != 6
                        || !int.TryParse(parts[2], out int _)
                        || !int.TryParse(parts[3], out int _))
                        throw new ArgumentException($"Instruction is not well formatted: {instruction}");
                    break;

                default:
                    throw new ArgumentException($"Instruction is invalid: {instruction}");
            }

            return parts;
        }

        public Map CreateMap(string instruction)
        {
            if (!instruction.StartsWith("C"))
                throw new ArgumentException($"Instruction for map does not starts with C : {instruction}");

            var partsInstruction = ParseInstruction(instruction);

            return new Map(int.Parse(partsInstruction[1]), int.Parse(partsInstruction[2]));
        }

        public static MapElement CreateMapElement(string instruction)
        {
            var partsInstruction = ParseInstruction(instruction);

            char firstChar = instruction[0];

            return firstChar switch
            {
                'A' => new Adventurer(
                                        partsInstruction[1],
                                        int.Parse(partsInstruction[2]),
                                        int.Parse(partsInstruction[3]),
                                        partsInstruction[4],
                                        partsInstruction[5]
                                      ),

                'M' => new Mountain(int.Parse(partsInstruction[1]), int.Parse(partsInstruction[2])),

                'T' => new Treasure(int.Parse(partsInstruction[1]), int.Parse(partsInstruction[2]), int.Parse(partsInstruction[3])),

                _ => throw new ArgumentException($"Instruction is invalid: {instruction}"),
            };
        }

        public void MoveAdventurers()
        {
            foreach (Adventurer adventurer in Map.Elements.OfType<Adventurer>().Where(advent => advent.HasMovementLeft))
            {
                char moveAction = adventurer.NextMove;

                switch (moveAction)
                {
                    case Constants.FORWARD_CHARACTER:
                        adventurer.GetNextMovement(out int positionX, out int positionY);

                        MapElement? nextElem = Map.GetElement(positionX, positionY);

                        if (nextElem != null && nextElem.IsObstacle)
                            nextElem = null;

                        if (nextElem != null && nextElem is Treasure treasure)
                        {
                            if (treasure.Collect())
                                adventurer.GetTreasure();
                        }

                        MapElement? previousElem = adventurer.Move(nextElem);

                        if (previousElem != null)
                        {
                            Map.FillMap(adventurer);
                            Map.FillMap(previousElem);
                        }

                        if (isGraphic)
                            Console.WriteLine(Map);

                        break;

                    case Constants.RIGHT_CHARACTER:
                    case Constants.LEFT_CHARACTER:
                        adventurer.Rotate(moveAction == Constants.RIGHT_CHARACTER);
                        break;

                    default:
                        throw new ArgumentException($"Movement is invalid: {adventurer.MovementString}");
                }

            }
        }

        public void RunExpedition()
        {
            while (Map.Elements.OfType<Adventurer>().Any(adventurer => adventurer.HasMovementLeft))
            {
                MoveAdventurers();
            }
        }

        public List<string> GetResult()
        {
            List<string> result = new()
            {
                $"C - {Map.SizeX} - {Map.SizeY}"
            };

            foreach (Mountain mountain in Map.Elements.OfType<Mountain>())
                result.Add($"M - {mountain.PositionX} - {mountain.PositionY}");

            foreach (Treasure treasure in Map.Elements.OfType<Treasure>())
                result.Add($"T - {treasure.PositionX} - {treasure.PositionY} - {treasure.Count}");

            foreach (Adventurer adventurer in Map.Elements.OfType<Adventurer>())
                result.Add($"A - {adventurer.Name} - {adventurer.PositionX} - {adventurer.PositionY} - {adventurer.Orientation} - {adventurer.TreasureCount}");

            return result;
        }

        public void WriteResult()
        {
            List<string> result = GetResult();
            View.Write(result, Path.Combine(Directory.GetCurrentDirectory(), "output.txt"));
        }
    }
}
