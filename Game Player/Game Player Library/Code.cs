using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace Game_Player
{
    public static class Code
    {
        private static List<string> assemblies;
        public static List<string> Assemblies
        {
            get { return assemblies; }
            set { assemblies = value; }
        }

        private static List<string> imports;
        public static List<string> Imports
        {
            get { return imports; }
            set { imports = value; }
        }

        static Code()
        {
            assemblies = new List<string>();
            assemblies.Add("system.dll");
            assemblies.Add("system.xml.dll");
            assemblies.Add("system.data.dll");
            assemblies.Add("Game Player.exe");

            imports = new List<string>();
            imports.Add("System");
            imports.Add("System.Xml");
            imports.Add("System.Data");
            imports.Add("System.Collections.Generic"); 
            imports.Add("System.Text");
            imports.Add("Game_Player");
        }

        public static object Eval(string code)
        {
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            CompilerParameters compilerParams = new CompilerParameters();

            CompilerResults compilerResults;
            System.Reflection.Assembly assembly;
            object execultableInstance = null;
            object returnObject = null;
            MethodInfo methodInfo;
            Type objectType;

            try
            {
                foreach (string a in assemblies)
                    compilerParams.ReferencedAssemblies.Add(a);

                compilerParams.CompilerOptions = "/t:library";
                compilerParams.GenerateInMemory = true;

                StringBuilder sb = new StringBuilder();

                foreach (string import in imports)
                {
                    sb.Append("using " + import + ";");
                }

                sb.Append("namespace Evaluate {\n");
                sb.Append("public class EClass {\n");
                sb.Append("public object EMethod() {\n");
                sb.Append(
                    (code.IndexOf("return") == -1 ? "return " : "") + 
                    code + 
                    (code.IndexOf(';') == -1 ? "; " : "")
                    );
                sb.Append("}}}");

                try
                {
                    compilerResults = codeProvider.CompileAssemblyFromSource(compilerParams, sb.ToString());

                    if (compilerResults.Errors.Count != 0)
                    {
                        StringBuilder errors = new StringBuilder();

                        errors.Append("There were compile errors when compiling the following code:\n");
                        errors.Append(code + "\n\nErrors:\n");

                        foreach (CompilerError error in compilerResults.Errors)
                            errors.Append(error.ErrorText + "\n");
                        throw new Exception(errors.ToString());
                    }
                    else
                    {
                        assembly = compilerResults.CompiledAssembly;
                        execultableInstance = assembly.CreateInstance("Evaluate.EClass");

                        objectType = execultableInstance.GetType();
                        methodInfo = objectType.GetMethod("EMethod");

                        returnObject = methodInfo.Invoke(execultableInstance, null);
                        return returnObject;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

