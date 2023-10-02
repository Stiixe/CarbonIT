using TreasureHunt;

namespace TreasureHuntTest
{
    [TestClass]
    public class ControllerTest
    {
        [TestMethod]
        public void TestCreateMap()
        {
            string instruction = "C - 3 - 4";
            Controller controller = new Controller(false);
            Map? map = controller.CreateMap(instruction);
            Assert.IsNotNull(map);
            Assert.AreEqual(3, map.SizeX);
            Assert.AreEqual(4, map.SizeY);
            Assert.AreEqual(".", map.GetElement(1,1)?.Symbol);
        }

        [TestMethod]
        public void TestCreateMapWrong()
        {
            string instruction = "M - 1 - 1";
            Controller controller = new Controller(false);
            Assert.ThrowsException<ArgumentException>(() => controller.CreateMap(instruction));
        }

        [TestMethod]
        public void TestCreateMapElementMountain()
        {
            string instruction = "M - 1 - 1";
            Controller controller = new Controller(false);
            MapElement element = Controller.CreateMapElement(instruction);
            Assert.IsTrue(element is Mountain);
            Assert.AreEqual(1, element.PositionY);
            Assert.IsTrue(element.IsObstacle);
        }

        [TestMethod]
        public void TestCreateMapElementTreasure()
        {
            string instruction = "T - 0 - 3 - 2";
            Controller controller = new Controller(false);
            MapElement element = Controller.CreateMapElement(instruction);
            Assert.IsTrue(element is Treasure);
            Assert.AreEqual(3, element.PositionY);
            Assert.IsFalse(element.IsObstacle);
            Assert.AreEqual(2, ((Treasure)element).Count);
        }

        [TestMethod]
        public void TestCreateMapElementAdventurer()
        {
            string instruction = "A - Lara - 1 - 1 - S - AADADAGGA";
            Controller controller = new Controller(false);
            MapElement element = Controller.CreateMapElement(instruction);
            Assert.IsTrue(element is Adventurer);
            Assert.AreEqual(1, element.PositionY);
            Assert.IsTrue(element.IsObstacle);

            Adventurer adventurer = (Adventurer)element;
            Assert.AreEqual("S", adventurer.Orientation);
            Assert.AreEqual("AADADAGGA", adventurer.MovementString);
            Assert.AreEqual("Lara", adventurer.Name);
        }

        [TestMethod]
        public void TestCreateMapElementInvalid()
        {
            string instruction = "B - Lara - 1 - 1 - S - AADADAGGA";
            Controller controller = new Controller(false);
            Assert.ThrowsException<ArgumentException>(() => Controller.CreateMapElement(instruction));
        }

        [TestMethod]
        public void TestCreateMapElementNotWellFormatted()
        {
            string instruction = "M - 1";
            Controller controller = new Controller(false);
            Assert.ThrowsException<ArgumentException>(() => Controller.CreateMapElement(instruction));
        }

        [TestMethod]
        public void TestInitController()
        {
            string fileContent = "C - 3 - 4\nM - 1 - 0\n#Test\nM - 2 - 1";
            string filePath = Path.GetTempPath() + "TestInitController.txt";
            File.WriteAllText(filePath, fileContent);

            Controller controller = new Controller(false);
            controller.Initialize(filePath);

            Assert.AreEqual(4, controller.Map.SizeY);
            Assert.AreEqual("M", controller.Map.GetElement(1,0)?.Symbol);
        }

        [TestMethod]
        public void TestMoveAdventurers()
        {
            string fileContent = "C - 2 - 2\nA - Lara - 1 - 1 - N - AAGAD";
            string filePath = Path.GetTempPath() + "TestMoveAdventurers.txt";
            File.WriteAllText(filePath, fileContent);

            Controller controller = new Controller(false);
            controller.Initialize(filePath);
            Adventurer? adventurer = (Adventurer?)controller.Map.GetElement(1, 1);
            Assert.IsNotNull(adventurer);

            controller.MoveAdventurers();
            Assert.AreEqual(1, adventurer.PositionX);
            Assert.AreEqual(0, adventurer.PositionY);
            Assert.AreEqual("N", adventurer.Orientation);

            controller.MoveAdventurers();
            Assert.AreEqual(1, adventurer.PositionX);
            Assert.AreEqual(0, adventurer.PositionY);
            Assert.AreEqual("N", adventurer.Orientation);

            controller.MoveAdventurers();
            Assert.AreEqual(1, adventurer.PositionX);
            Assert.AreEqual(0, adventurer.PositionY);
            Assert.AreEqual("O", adventurer.Orientation);

            controller.MoveAdventurers();
            Assert.AreEqual(0, adventurer.PositionX);
            Assert.AreEqual(0, adventurer.PositionY);
            Assert.AreEqual("O", adventurer.Orientation);

            controller.MoveAdventurers();
            Assert.AreEqual(0, adventurer.PositionX);
            Assert.AreEqual(0, adventurer.PositionY);
            Assert.AreEqual("N", adventurer.Orientation);
        }

        [TestMethod]
        public void TestMoveAdventurersWithMountain()
        {
            string fileContent = "C - 2 - 2\nA - Lara - 1 - 1 - N - AAGAD\nM - 0 - 0";
            string filePath = Path.GetTempPath() + "TestMoveAdventurersWithMountain.txt";
            File.WriteAllText(filePath, fileContent);

            Controller controller = new Controller(false);
            controller.Initialize(filePath);
            Adventurer? adventurer = (Adventurer?)controller.Map.GetElement(1, 1);
            Assert.IsNotNull(adventurer);

            controller.MoveAdventurers();
            Assert.AreEqual(1, adventurer.PositionX);
            Assert.AreEqual(0, adventurer.PositionY);
            Assert.AreEqual("N", adventurer.Orientation);

            controller.MoveAdventurers();
            Assert.AreEqual(1, adventurer.PositionX);
            Assert.AreEqual(0, adventurer.PositionY);
            Assert.AreEqual("N", adventurer.Orientation);

            controller.MoveAdventurers();
            Assert.AreEqual(1, adventurer.PositionX);
            Assert.AreEqual(0, adventurer.PositionY);
            Assert.AreEqual("O", adventurer.Orientation);

            controller.MoveAdventurers();
            Assert.AreEqual(1, adventurer.PositionX);
            Assert.AreEqual(0, adventurer.PositionY);
            Assert.AreEqual("O", adventurer.Orientation);

            controller.MoveAdventurers();
            Assert.AreEqual(1, adventurer.PositionX);
            Assert.AreEqual(0, adventurer.PositionY);
            Assert.AreEqual("N", adventurer.Orientation);
        }

        [TestMethod]
        public void TestMoveAdventurersWithTreasures()
        {
            string fileContent = "C - 2 - 2\nA - Lara - 1 - 1 - N - AGADDA\nT - 1 - 0 - 2";
            string filePath = Path.GetTempPath() + "TestMoveAdventurersWithTreasures.txt";
            File.WriteAllText(filePath, fileContent);

            Controller controller = new Controller(false);
            controller.Initialize(filePath);
            Adventurer? adventurer = (Adventurer?)controller.Map.GetElement(1, 1);
            Assert.IsNotNull(adventurer);
            Treasure? treasure = (Treasure?)controller.Map.GetElement(1, 0);
            Assert.IsNotNull(treasure);
            Assert.AreEqual(2, treasure.Count);
            Assert.AreEqual(0, adventurer.TreasureCount);

            controller.MoveAdventurers();
            Assert.AreEqual(1, adventurer.PositionX);
            Assert.AreEqual(0, adventurer.PositionY);
            Assert.AreEqual("N", adventurer.Orientation);
            Assert.AreEqual(1, adventurer.TreasureCount);
            Assert.AreEqual(1, treasure.Count);

            controller.MoveAdventurers();
            Assert.AreEqual(1, adventurer.PositionX);
            Assert.AreEqual(0, adventurer.PositionY);
            Assert.AreEqual("O", adventurer.Orientation);
            Assert.AreEqual(1, adventurer.TreasureCount);
            Assert.AreEqual(1, treasure.Count);

            controller.MoveAdventurers();
            Assert.AreEqual(0, adventurer.PositionX);
            Assert.AreEqual(0, adventurer.PositionY);
            Assert.AreEqual("O", adventurer.Orientation);
            Assert.AreEqual(1, adventurer.TreasureCount);
            Assert.AreEqual(1, treasure.Count);

            controller.MoveAdventurers();
            Assert.AreEqual(0, adventurer.PositionX);
            Assert.AreEqual(0, adventurer.PositionY);
            Assert.AreEqual("N", adventurer.Orientation);
            Assert.AreEqual(1, adventurer.TreasureCount);
            Assert.AreEqual(1, treasure.Count);

            controller.MoveAdventurers();
            Assert.AreEqual(0, adventurer.PositionX);
            Assert.AreEqual(0, adventurer.PositionY);
            Assert.AreEqual("E", adventurer.Orientation);
            Assert.AreEqual(1, adventurer.TreasureCount);
            Assert.AreEqual(1, treasure.Count);

            controller.MoveAdventurers();
            Assert.AreEqual(1, adventurer.PositionX);
            Assert.AreEqual(0, adventurer.PositionY);
            Assert.AreEqual("E", adventurer.Orientation);
            Assert.AreEqual(2, adventurer.TreasureCount);
            Assert.AreEqual(0, treasure.Count);
        }

        [TestMethod]
        public void TestMoveAdventurersWithEmptyTreasures()
        {
            string fileContent = "C - 2 - 2\nA - Lara - 1 - 1 - N - AGADDA\nT - 1 - 0 - 1";
            string filePath = Path.GetTempPath() + "TestMoveAdventurersWithTreasures.txt";
            File.WriteAllText(filePath, fileContent);

            Controller controller = new Controller(false);
            controller.Initialize(filePath);
            Adventurer? adventurer = (Adventurer?)controller.Map.GetElement(1, 1);
            Assert.IsNotNull(adventurer);
            Treasure? treasure = (Treasure?)controller.Map.GetElement(1, 0);
            Assert.IsNotNull(treasure);
            Assert.AreEqual(1, treasure.Count);
            Assert.AreEqual(0, adventurer.TreasureCount);

            controller.MoveAdventurers();
            Assert.AreEqual(1, adventurer.PositionX);
            Assert.AreEqual(0, adventurer.PositionY);
            Assert.AreEqual("N", adventurer.Orientation);
            Assert.AreEqual(1, adventurer.TreasureCount);
            Assert.AreEqual(0, treasure.Count);

            controller.MoveAdventurers();
            Assert.AreEqual(1, adventurer.PositionX);
            Assert.AreEqual(0, adventurer.PositionY);
            Assert.AreEqual("O", adventurer.Orientation);
            Assert.AreEqual(1, adventurer.TreasureCount);
            Assert.AreEqual(0, treasure.Count);

            controller.MoveAdventurers();
            Assert.AreEqual(0, adventurer.PositionX);
            Assert.AreEqual(0, adventurer.PositionY);
            Assert.AreEqual("O", adventurer.Orientation);
            Assert.AreEqual(1, adventurer.TreasureCount);
            Assert.AreEqual(0, treasure.Count);

            controller.MoveAdventurers();
            Assert.AreEqual(0, adventurer.PositionX);
            Assert.AreEqual(0, adventurer.PositionY);
            Assert.AreEqual("N", adventurer.Orientation);
            Assert.AreEqual(1, adventurer.TreasureCount);
            Assert.AreEqual(0, treasure.Count);

            controller.MoveAdventurers();
            Assert.AreEqual(0, adventurer.PositionX);
            Assert.AreEqual(0, adventurer.PositionY);
            Assert.AreEqual("E", adventurer.Orientation);
            Assert.AreEqual(1, adventurer.TreasureCount);
            Assert.AreEqual(0, treasure.Count);

            controller.MoveAdventurers();
            Assert.AreEqual(1, adventurer.PositionX);
            Assert.AreEqual(0, adventurer.PositionY);
            Assert.AreEqual("E", adventurer.Orientation);
            Assert.AreEqual(1, adventurer.TreasureCount);
            Assert.AreEqual(0, treasure.Count);
        }

        [TestMethod]
        public void TestGetResult()
        {
            string fileContent = "C - 2 - 2\nA - Lara - 0 - 1 - N - AGADDA\nT - 1 - 0 - 2";
            string filePath = Path.GetTempPath() + "TestGetResult.txt";
            File.WriteAllText(filePath, fileContent);

            Controller controller = new Controller(false);
            controller.Initialize(filePath);

            List<string> result = controller.GetResult();
            Assert.AreEqual(3, result.Count);
            Assert.IsTrue(result[0].StartsWith("C"));
            Assert.IsTrue(result[2].EndsWith("0"));
        }

    }

}
