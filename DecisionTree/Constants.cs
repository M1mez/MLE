using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    public static class Constants
    {
        public static string CSVPath => Path.Combine(_projectPath, "CSV");
        private static string _projectPath => Path.GetFullPath(Path.Combine(_assemblyPath, @"..\..\..\"));
        private static string _assemblyPath => Assembly.GetExecutingAssembly().Location;

        
    }
}
