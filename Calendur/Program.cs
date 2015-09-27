using System;

namespace Calendur
{
    class Program
    {
        static void Main(string[] args)
        {
            var @params = new InputParams(args);

            if (String.IsNullOrWhiteSpace(@params.InputPath))
            {
                Console.Error.WriteLine("Pass path to docx file for parsing.");
                return;
            }

            Calendar calendar = new DocxParser(@params.Year).ParseYear(@params.InputPath);

            new TextFileFormatter(calendar).SaveToFile(@params.OutputPath);
        }
    }
}
