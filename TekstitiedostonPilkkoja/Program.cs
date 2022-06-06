using System;
using System.IO;

namespace TekstitiedostonPilkkoja {
  class Program {

    /*
     * Konffaa tähän itsellesi sopivat arvot outputfilelle
     * 
     * Argumentit exelle 
     * 1. tiedostonimi, esim KTL_DANSKE_20220531155353.001
     * 2. rivejä per tiedosto, esim 300 (optional). Default 250
     * 3. outputFilePath, esim C:\Temp\pilkotutviitemaksutiedostot (optional). Default C:\Temp\pilkotutviitemaksutiedostot
     */
    private const string _defaultOutputFilePath = @"C:\Temp\pilkotutviitemaksutiedostot";
    /// <summary>
    /// Montako riviä per tiedosto (oletus)
    /// </summary>
    private const int _defaultLinesPerFile = 250;

    private static string _outputFilePath;
    /// <summary>
    /// Montako riviä per tiedosto
    /// </summary>
    private static int _linesPerFile;
    /// <summary>
    /// Poistetaanko nykyiset output tiedostot?
    /// </summary>
    private static bool _deleteCurrentOutputFiles = true;

    static void Main(string[] args) {

      string inputFileName = GetFileNameByArgs(args);

      if(!File.Exists(inputFileName)) {
        Console.WriteLine("Tiedostoa ei löydy annetusta polusta " + inputFileName);
        Console.WriteLine("Exit");
        Console.ReadLine();
        return;
      }

      SetLinesPerFile(args);

      SetOutputFilePath(args);

      CreateOutputDirectoryIfNotExists(_outputFilePath);

      if(_deleteCurrentOutputFiles) {
        DeleteCurrentOutputfiles(_outputFilePath);
      }

      //Do the file splices

      var fileSplicer = new FileSplicer(_linesPerFile, _outputFilePath);

      fileSplicer.SpliceFileToNewFiles(inputFileName);

      Console.WriteLine("Click enter to exit program");
      Console.ReadLine();
    }

    private static void SetOutputFilePath(string[] args) {
      if(args.Length > 2) {
        _outputFilePath = GetOutputFilePathByArgs(args);
      } else {
        _outputFilePath = _defaultOutputFilePath;
      }
    }

    private static void SetLinesPerFile(string[] args) {
      if(args.Length > 1) {
        _linesPerFile = GetMaxLinesPerFileByArgs(args);
      } else {
        _linesPerFile = _defaultLinesPerFile;
      }
    }

    private static int GetMaxLinesPerFileByArgs(string[] args) {
      return Convert.ToInt32(args[1].ToString());
    }

    private static string GetOutputFilePathByArgs(string[] args) {
      return args[2].ToString();
    }

    private static string GetFileNameByArgs(string[] args) {
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

    private static void CreateOutputDirectoryIfNotExists(string outputFilePath) {
      if(!Directory.Exists(outputFilePath)) {
        Console.WriteLine($"Creating output directory to path {outputFilePath}");
        Directory.CreateDirectory(outputFilePath);
      }
    }

    private static void DeleteCurrentOutputfiles(string outputFilePath) {
      if(Directory.Exists(outputFilePath)) {
        Console.WriteLine($"Deleting current output files from {outputFilePath}");
        var directoryInfo = new DirectoryInfo(outputFilePath);
        foreach(FileInfo file in directoryInfo.GetFiles()) {
          file.Delete();
        }
      }
    }

  }
}
