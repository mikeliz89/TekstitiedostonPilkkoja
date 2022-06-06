using System;
using System.IO;

namespace TekstitiedostonPilkkoja {
  class Program {

    /*
     * Konffaa tähän itsellesi sopivat arvot outputfilelle
     * 
     * Argumentit exelle 
     * 1. tiedostonimi, esim KTL_DANSKE_20220531155353.001
     * 2. rivejä per tiedosto, esim 300 (optional)
     */
    private const string _outputFilePath = @"C:\Temp\pilkotutviitemaksutiedostot";
    /// <summary>
    /// Montako riviä per tiedosto (oletus)
    /// </summary>
    public const int DefaultLinesPerFile = 250;
    /// <summary>
    /// Montako riviä per tiedosto
    /// </summary>
    private static int _linesPerFile;
    /// <summary>
    /// Poistetaanko nykyiset output tiedostot?
    /// </summary>
    private static bool _deleteCurrentOutputFiles = true;

    static void Main(string[] args) {

      string inputFileName = GetInputFileName(args);

      if(args.Length > 1) {
        _linesPerFile = GetMaxLinesPerFileByInput(args);
      } else {
        _linesPerFile = DefaultLinesPerFile;
      }

      if(!File.Exists(inputFileName)) {
        Console.WriteLine("Tiedostoa ei löydy annetusta polusta " + inputFileName);
        Console.WriteLine("Exit");
        Console.ReadLine();
        return;
      }

      CreateOutputDirectoryIfNotExists();

      if(_deleteCurrentOutputFiles) {
        DeleteCurrentOutputfiles();
      }

      var fileSplicer = new FileSplicer(_linesPerFile, _outputFilePath);

      fileSplicer.SpliceFileToNewFiles(inputFileName);

      Console.WriteLine("Click enter to exit program");
      Console.ReadLine();
    }

    private static int GetMaxLinesPerFileByInput(string[] args) {
      return Convert.ToInt32(args[1].ToString());
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
      if(!Directory.Exists(_outputFilePath)) {
        Directory.CreateDirectory(_outputFilePath);
      }
    }

    private static void DeleteCurrentOutputfiles() {
      if(Directory.Exists(_outputFilePath)) {
        var directoryInfo = new DirectoryInfo(_outputFilePath);
        foreach(FileInfo file in directoryInfo.GetFiles()) {
          file.Delete();
        }
      }
    }

  }
}
