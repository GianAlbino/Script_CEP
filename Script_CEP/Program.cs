using System.Reflection;

namespace Script_CEP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var csvLocation = CreateOrGetRootDirectory();
            var insertHeader = true;

            foreach (string csv in csvLocation)
            {
                using var reader = new StreamReader(csv);

                var keyValuePairs = new Dictionary<string, List<string>>();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    if (insertHeader)
                    {
                        foreach (var v in values)
                        {
                            keyValuePairs.Add(v, new List<string>());
                        }
                    }
                    else
                    {
                        int i = 0;

                        foreach (var v in values)
                        {
                            keyValuePairs.ElementAtOrDefault(i).Value.Add(v);
                            i++;
                        }
                    }

                    insertHeader = false;
                }

                InsertInDB(keyValuePairs);
            }
        }

        private static async void InsertInDB(Dictionary<string, List<string>> keyValuePairs)
        {
            for (int i = keyValuePairs["Id"].Count - 1; i > 0; i--)
            {
                string candidateId = keyValuePairs["Id"][i];
                string candidateZipCode = keyValuePairs["ZipCode"][i];

                Console.WriteLine($"Atualizando candidate: {candidateId}, com zipCode: {candidateZipCode}");

                await ServiceAPI
                    .Instance()
                    .UpdateCandidate(
                    new Guid(candidateId),
                    candidateZipCode);
            }
        }

        private static List<string> CreateOrGetRootDirectory()
        {
            var rootDirectory = $"{Environment.CurrentDirectory}/Logs/";

            if (!Directory.Exists(rootDirectory))
            {
                Directory.CreateDirectory(rootDirectory);
            }

            return Directory.GetFiles(rootDirectory).ToList();
        }
    }
}