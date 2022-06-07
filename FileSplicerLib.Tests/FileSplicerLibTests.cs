using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileSplicerLib.Tests {

  [TestClass]
  public class CalculateHowManyFilesToCreateTests {

    [TestMethod]
    public void CalculateHowManyFilesToCreate_ZeroLinesPerFile_Test() {
      var linesPerFile = 0;
      var splicer = new FileSplicer(linesPerFile, "");
      var fileLines = GetFileLines();
      var res = splicer.CalculateHowManyFilesToCreate(fileLines);
      Assert.AreEqual(0, res);
    }

    [TestMethod]
    public void CalculateHowManyFilesToCreate_1LinesPerFile_Test() {
      var linesPerFile = 1;
      var splicer = new FileSplicer(linesPerFile, "");
      var fileLines = GetFileLines();
      var res = splicer.CalculateHowManyFilesToCreate(fileLines);
      Assert.AreEqual(3, res);
    }

    [TestMethod]
    public void CalculateHowManyFilesToCreate_2LinesPerFile_Test() {
      var linesPerFile = 2;
      var splicer = new FileSplicer(linesPerFile, "");
      var fileLines = GetFileLines();
      var res = splicer.CalculateHowManyFilesToCreate(fileLines);
      Assert.AreEqual(2, res);
    }

    [TestMethod]
    public void CalculateHowManyFilesToCreate_3LinesPerFile_Test() {
      var linesPerFile = 3;
      var splicer = new FileSplicer(linesPerFile, "");
      var fileLines = GetFileLines();
      var res = splicer.CalculateHowManyFilesToCreate(fileLines);
      Assert.AreEqual(1, res);
    }

    [TestMethod]
    public void CalculateHowManyFilesToCreate_4LinesPerFile_Test() {
      var linesPerFile = 4;
      var splicer = new FileSplicer(linesPerFile, "");
      var fileLines = GetFileLines();
      var res = splicer.CalculateHowManyFilesToCreate(fileLines);
      Assert.AreEqual(1, res);
    }

    private static string[] GetFileLines() {
      string[] fileLines = { "row1", "row2", "row3" };
      return fileLines;
    }

    [TestMethod]
    public void CalculateHowManyFilesToCreate_EmptyLines_Test() {
      var linesPerFile = 250;
      var splicer = new FileSplicer(linesPerFile, "");
      string[] fileLines = { };
      var res = splicer.CalculateHowManyFilesToCreate(fileLines);
      Assert.AreEqual(0, res);
    }
  }
}
