using System;
using System.IO;
using System.Linq;

namespace TekstitiedostonPilkkoja {
  class Program {

    /*
     * Konffaa tähän itsellesi sopivat arvot outputfilelle
     */
    public const string outputFilePath = @"C:\Temp\pilkotutviitemaksutiedostot";
    public const string outputFileName = "tiedosto";
    public const string outputFileSuffix = ".txt";
    /// <summary>
    /// Montako riviä per tiedosto
    /// </summary>
    public const int maxLinesPerFile = 500;

    static void Main(string[] args) {

      string inputFileName = GetInputFileName(args);

      if(!File.Exists(inputFileName)) {
        Console.WriteLine("Tiedostoa ei löydy annetusta polusta " + inputFileName);
        Console.WriteLine("Exit");
        Console.ReadLine();
        return;
      }

      CreateOutputDirectoryIfNotExists();

      DeleteCurrentOutputfiles();

      SpliceFileToNewFiles(inputFileName);

      Console.WriteLine("Click enter to exit program");
      Console.ReadLine();
    }

    private static string GetInputFileName(string[] args) {
      string inputFileName = GetFileNameFromArgument(args);

      if(string.IsNullOrEmpty(inputFileName)) {
        inputFileName = AskInputFileNameFromConsole(inputFileName);
      }

      return inputFileName;
    }

    private static string AskInputFileNameFromConsole(string inputFileName) {
      Console.WriteLine("Anna tiedoston polku");
      inputFileName = Console.ReadLine();
      return inputFileName.Replace("\"", "");
    }

    private static string GetFileNameFromArgument(string[] args) {
      string inputFileName = "";
      if(args.Length > 0) {
        if(!string.IsNullOrEmpty(args[0].ToString())) {
          inputFileName = args[0].ToString();
        }
      }
      return inputFileName;
    }

    private static void CreateOutputDirectoryIfNotExists() {
      if(!Directory.Exists(outputFilePath)) {
        Directory.CreateDirectory(outputFilePath);
      }
    }

    private static void DeleteCurrentOutputfiles() {
      if(Directory.Exists(outputFilePath)) {
        var directoryInfo = new DirectoryInfo(outputFilePath);
        foreach(FileInfo file in directoryInfo.GetFiles()) {
          file.Delete();
        }
      }
    }

    private static string firstLine = "";
    private static string lastLine = "";

    private static void SpliceFileToNewFiles(string fileName) {

      string[] allFileLines = ReadAllLinesFromFile(fileName);

      firstLine = GetFirstLine(allFileLines);
      lastLine = GetLastLine(allFileLines);

      string[] fileLines = GetLinesWithoutHeaderAndFooterLines(allFileLines);

      int howManyfilesToCreate = CalculateHowManyFilesToCreate(fileLines);

      var lineCounter = 0;
      for(int i = 0; i < howManyfilesToCreate; i++) {
        string newOutputFileName = CalculateNewOutputFileName(i);
        Console.WriteLine("Creating new file " + newOutputFileName);
        AddFirstLine(newOutputFileName);
        lineCounter = AddLinesToFile(fileLines, lineCounter, newOutputFileName);
        AddLastLine(newOutputFileName);
      }
    }

    private static int AddLinesToFile(string[] fileLines, int lineCounter, string newOutputFileName) {
      for(int j = 0; j < maxLinesPerFile; j++) {
        if(fileLines.Length <= lineCounter) {
          break;
        }
        AddLineToFile(fileLines[lineCounter], newOutputFileName);
        lineCounter++;
      }
      return lineCounter;
    }

    private static void AddLastLine(string newOutputFileName) {
      AddLineToFile(lastLine, newOutputFileName);
    }

    private static void AddFirstLine(string newOutputFileName) {
      AddLineToFile(firstLine, newOutputFileName);
    }

    private static int CalculateHowManyFilesToCreate(string[] fileLines) {
      return (fileLines.Length / maxLinesPerFile) + 1;
    }

    private static string[] GetLinesWithoutHeaderAndFooterLines(string[] allFileLines) {
      string[] linesWithoutHeaderAndFooterLines = DeleteFirstLine(allFileLines);
      linesWithoutHeaderAndFooterLines = DeleteLastLine(linesWithoutHeaderAndFooterLines);
      return linesWithoutHeaderAndFooterLines;
    }

    private static string CalculateNewOutputFileName(int i) {
      return outputFilePath + "\\" + outputFileName + (i + 1) + outputFileSuffix;
    }

    private static string[] DeleteLastLine(string[] fileLinesExceptFirstAndLast) {
      fileLinesExceptFirstAndLast = fileLinesExceptFirstAndLast.Take(fileLinesExceptFirstAndLast.Count() - 1).ToArray();
      return fileLinesExceptFirstAndLast;
    }

    private static string[] DeleteFirstLine(string[] fileLines) {
      return fileLines.Skip(1).ToArray();
    }

    private static void AddLineToFile(string line, string fileName) {
      using(StreamWriter sw = File.AppendText(fileName)) {
        sw.WriteLine(line);
      }
    }

    private static string[] ReadAllLinesFromFile(string fileName) {
      return File.ReadAllLines(fileName);
    }

    private static string GetLastLine(string[] fileLines) {
      var lastLine = fileLines.Last();
      return lastLine;
    }

    private static string GetFirstLine(string[] fileLines) {
      var firstLine = fileLines.First(); ;
      return firstLine;
    }
  }
}
