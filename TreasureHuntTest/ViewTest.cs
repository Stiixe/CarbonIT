using TreasureHunt;

namespace TreasureHuntTest
{
    [TestClass]
    public class ViewTest
    {
        [TestMethod]
        public void TestRead()
        {
            string fileContent = "C - 3 - 4\nM - 1 - 0\n#Test\nM - 2 - 1";
            string filePath = Path.GetTempPath() + "TestRead.txt";
            File.WriteAllText(filePath, fileContent);

            View view = new View();
            List<string> listInstructions = view.Read(filePath);

            Assert.AreEqual(3, listInstructions.Count);
            Assert.IsTrue(listInstructions[0].StartsWith("C"));
            Assert.IsTrue(listInstructions[1].EndsWith("0"));
            Assert.IsTrue(listInstructions[2].StartsWith("M"));
        }

        [TestMethod]
        public void TestWrite()
        {
            List<string> result = new List<string>
            {
                "C - 3 - 4",
                "M - 1 - 0",
                "A - Lara - 0 - 3 - S - 3",
                "T - 1 - 3 - 2"
            };

            View view = new View();
            string path = Path.Combine(Path.GetTempPath(), "TestWrite.txt");
            view.Write(result, path);
            Assert.IsTrue(File.Exists(path));

            string[] writtenContent = File.ReadAllLines(path);
            Assert.AreEqual(4, writtenContent.Length);
            Assert.IsTrue(writtenContent[0].StartsWith("C"));
            Assert.IsTrue(writtenContent[1].EndsWith("0"));
        }
    }
}