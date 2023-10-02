using TreasureHunt;

namespace TreasureHuntTest
{
    [TestClass]
    public class MapElementTests
    {
        [TestMethod]
        public void MapElementConstructor_ShouldInitializeProperties()
        {
            MapElement mapElement = new MapElement(1, 2, "M", true);

            Assert.AreEqual(1, mapElement.PositionX);
            Assert.AreEqual(2, mapElement.PositionY);
            Assert.AreEqual("M", mapElement.Symbol);
            Assert.IsTrue(mapElement.IsObstacle);
        }

        [TestMethod]
        public void MapElementToString_ShouldReturnSymbol()
        {
            MapElement mapElement = new MapElement(1, 2, "M", true);

            string result = mapElement.ToString();

            Assert.AreEqual("M", result);
        }
    }

    [TestClass]
    public class MountainTests
    {
        [TestMethod]
        public void MountainConstructor_ShouldInitializeProperties()
        {
            Mountain mountain = new Mountain(3, 4);

            Assert.AreEqual(3, mountain.PositionX);
            Assert.AreEqual(4, mountain.PositionY);
            Assert.AreEqual("M", mountain.Symbol);
            Assert.IsTrue(mountain.IsObstacle);
        }
    }

    [TestClass]
    public class TreasureTests
    {
        [TestMethod]
        public void TreasureConstructor_ShouldInitializeProperties()
        {
            Treasure treasure = new Treasure(5, 6, 10);

            Assert.AreEqual(5, treasure.PositionX);
            Assert.AreEqual(6, treasure.PositionY);
            Assert.AreEqual("T", treasure.Symbol);
            Assert.IsFalse(treasure.IsObstacle);
            Assert.AreEqual(10, treasure.Count);
        }

        [TestMethod]
        public void Collect_ShouldDecrementCount()
        {
            Treasure treasure = new Treasure(5, 6, 10);

            bool result = treasure.Collect();

            Assert.IsTrue(result);
            Assert.AreEqual(9, treasure.Count);
        }

        [TestMethod]
        public void Collect_ShouldReturnFalseIfCountIsZero()
        {
            Treasure treasure = new Treasure(5, 6, 0);

            bool result = treasure.Collect();

            Assert.IsFalse(result);
            Assert.AreEqual(0, treasure.Count);
        }
    }

    [TestClass]
    public class AdventurerTests
    {
        [TestMethod]
        public void AdventurerConstructor_ShouldInitializeProperties()
        {
            Adventurer adventurer = new Adventurer("John", 1, 2, Constants.NORTH, "AAGD");

            Assert.AreEqual("John", adventurer.Name);
            Assert.AreEqual(1, adventurer.PositionX);
            Assert.AreEqual(2, adventurer.PositionY);
            Assert.AreEqual(Constants.NORTH, adventurer.Orientation);
            Assert.AreEqual("AAGD", adventurer.MovementString);
            Assert.AreEqual(0, adventurer.TreasureCount);
        }

        [TestMethod]
        public void Rotate_ShouldChangeOrientation()
        {
            Adventurer adventurer = new Adventurer("John", 1, 2, Constants.NORTH, "AAGD");

            adventurer.Rotate(true);

            Assert.AreEqual(Constants.EAST, adventurer.Orientation);
        }

        [TestMethod]
        public void Rotate_ShouldWrapAroundWhenReaching360Degrees()
        {
            Adventurer adventurer = new Adventurer("John", 1, 2, Constants.WEST, "AAGD");

            adventurer.Rotate(true);

            Assert.AreEqual(Constants.NORTH, adventurer.Orientation);
        }

        [TestMethod]
        public void GetNextMovement_ShouldReturnCorrectCoordinates()
        {
            Adventurer adventurer = new Adventurer("John", 1, 2, Constants.EAST, "AAGD");
            int nextX, nextY;

            adventurer.GetNextMovement(out nextX, out nextY);

            Assert.AreEqual(2, nextX);
            Assert.AreEqual(2, nextY);
        }

        [TestMethod]
        public void Move_ShouldUpdatePositionAndReturnPreviousElement()
        {
            MapElement nextElement = new MapElement(2, 2);
            Adventurer adventurer = new Adventurer("John", 1, 2, Constants.EAST, "A");

            MapElement? result = adventurer.Move(nextElement);

            Assert.AreEqual(2, adventurer.PositionX);
            Assert.AreEqual(2, adventurer.PositionY);
            Assert.AreEqual(1, result?.PositionX);
            Assert.AreEqual(2, result?.PositionY);
        }

        [TestMethod]
        public void GetTreasure_ShouldIncrementTreasureCount()
        {
            Adventurer adventurer = new Adventurer("John", 1, 2, Constants.EAST, "A");

            adventurer.GetTreasure();

            Assert.AreEqual(1, adventurer.TreasureCount);
        }

        [TestClass]
        public class MapTests
        {
            [TestMethod]
            public void MapConstructor_ShouldInitializeSizeAndElements()
            {
                Map map = new Map(3, 4);

                Assert.AreEqual(3, map.SizeX);
                Assert.AreEqual(4, map.SizeY);
                Assert.IsNotNull(map.Elements);
                Assert.AreEqual(3, map.Elements.GetLength(0));
                Assert.AreEqual(4, map.Elements.GetLength(1));
            }

            [TestMethod]
            public void GetElement_ShouldReturnElementIfExists()
            {
                Map map = new Map(3, 4);
                MapElement element = new MapElement(1, 2);
                map.FillMap(element);

                MapElement? result = map.GetElement(1, 2);

                Assert.IsNotNull(result);
                Assert.AreSame(element, result);
            }

            [TestMethod]
            public void GetElement_ShouldReturnNullIfOutOfBoundaries()
            {
                Map map = new Map(3, 4);

                MapElement? result = map.GetElement(5, 5);

                Assert.IsNull(result);
            }
        }

    }
}
