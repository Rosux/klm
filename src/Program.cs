using System;
using System.ComponentModel;

class Program
{
    public static User? CurrentUser = null;
    public static void Main()
    {
        LoadEnvironment();

        if(Environment.GetEnvironmentVariable("DEBUG")?.ToLower() == "true"){
            Menu.TestStart();
        }else{
            Menu.Start();
        }
    }

    private static void LoadEnvironment()
    {
        // get the current working directory .env file path
        string environmentFilePath = Path.Combine(Directory.GetCurrentDirectory(), ".env");

        // if the file doesnt exist throw an error
        if(!File.Exists(environmentFilePath))
        {
            throw new Exception($"Environment file not found at path: {environmentFilePath}.");
        }

        // go over each line and set the programs environment variables
        foreach (var line in File.ReadAllLines(environmentFilePath))
        {
            // split "ENV_NAME=ENV_VALUE" at the = resulting in an array of {"ENV_NAME", "ENV_VALUE"}
            string[] parts = line.Split('=', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            // if there are no 2 values skip this entry
            if(parts.Length != 2){
                continue;
            }

            Environment.SetEnvironmentVariable(parts[0], parts[1]);
        }
    }
}
