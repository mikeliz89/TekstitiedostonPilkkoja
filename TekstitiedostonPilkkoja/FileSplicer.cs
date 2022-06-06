using System;
using System.IO;
using System.Linq;

namespace TekstitiedostonPilkkoja {
  public class FileSplicer {

    /// <summary>
    /// Output tiedoston oletusnimi
    /// </summary>
    public const string DefaultOutputFileName = "tiedosto";

    /// <summary>
    /// Output tiedoston tiedostopääte
    /// </summary>
    private const string _outputFileSuffix = ".txt";

    //properties
    public int LinesPerFile {
      get {
        return _linesPerFile;
      }
      set {
        _linesPerFile = value;
      }
    }
    private int _linesPerFile { get; set; }

    public string OutputFilePath {
      get {
        return _outputFilePath;
      }
      set {
        _outputFilePath = value;
      }
    }
    private string _outputFilePath { get; set; }

    public FileSplicer(int linesPerFile, string outputFilePath) {
      _linesPerFile = linesPerFile;
      _outputFilePath = outputFilePath;
    }

    private string firstLine = "";
    private string lastLine = "";

    public void SpliceFileToNewFiles(string fileName) {

      Console.WriteLine($"Starting to splice file {fileName}");

      string[] allFileLines = ReadAllLinesFromFile(fileName);

      firstLine = GetFirstLine(allFileLines);
      lastLine = GetLastLine(allFileLines);

      string[] fileLines = GetLinesWithoutHeaderAndFooterLines(allFileLines);

      Console.WriteLine($"Settings: {_linesPerFile} lines per file, outputFileSuffix = {_outputFileSuffix}");
      Console.WriteLine($"File has {fileLines.Length} rows (without header and footer lines)");

      int howManyfilesToCreate = CalculateHowManyFilesToCreate(fileLines);

      Console.WriteLine($"{howManyfilesToCreate} new files to create");

      var lineCounter = 0;
      for(int i = 0; i < howManyfilesToCreate; i++) {
        string newOutputFileName = CalculateNewOutputFileName(i, fileName);
        Console.WriteLine("Creating new file " + newOutputFileName);
        AddFirstLine(newOutputFileName);
        lineCounter = AddLinesToFile(fileLines, lineCounter, newOutputFileName);
        AddLastLine(newOutputFileName);
      }
    }

    private int AddLinesToFile(string[] fileLines, int lineCounter, string newOutputFileName) {
      for(int j = 0; j < _linesPerFile; j++) {
        if(fileLines.Length <= lineCounter) {
          break;
        }
        AddLineToFile(fileLines[lineCounter], newOutputFileName);
        lineCounter++;
      }
      return lineCounter;
    }

    private void AddLastLine(string newOutputFileName) {
      AddLineToFile(lastLine, newOutputFileName);
    }

    private void AddFirstLine(string newOutputFileName) {
      AddLineToFile(firstLine, newOutputFileName);
    }

    private int CalculateHowManyFilesToCreate(string[] fileLines) {
      return (fileLines.Length / _linesPerFile) + 1;
    }

    private static string[] GetLinesWithoutHeaderAndFooterLines(string[] allFileLines) {
      string[] lines = DeleteFirstLine(allFileLines);
      lines = DeleteLastLine(lines);
      return lines;
    }

    private string CalculateNewOutputFileName(int i, string fileName = "") {
      var fn = string.IsNullOrEmpty(fileName) ? DefaultOutputFileName : fileName;
      return $"{_outputFilePath }\\{fn}_{i + 1}{_outputFileSuffix}";
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
      return fileLines.Last();
    }

    private static string GetFirstLine(string[] fileLines) {
      return fileLines.First(); ;
    }
  }
}
