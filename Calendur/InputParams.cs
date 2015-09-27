using System;
using System.IO;

namespace Calendur
{
    public class InputParams
    {
        private readonly string[] _args;

        public InputParams(string[] args)
        {
            _args = args;
        }

        public string InputPath => _args.Length > 0 ? _args[0] : null;

        public string OutputPath => _args.Length > 2 ? _args[2] : Path.ChangeExtension(InputPath, "txt");

        public int Year => _args.Length > 1 ? int.Parse(_args[1]) : DateTime.Now.Year + 1;
    }
}
