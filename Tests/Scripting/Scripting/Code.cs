using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.CSharp;
using System;
using System.Text;
using System.CodeDom.Compiler;
using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace Scripting
{
    public static class Code
    {
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
                compilerParams.ReferencedAssemblies.Add("system.dll");
                compilerParams.ReferencedAssemblies.Add("system.xml.dll");
                compilerParams.ReferencedAssemblies.Add("system.data.dll");
                compilerParams.ReferencedAssemblies.Add("Scripting.exe");
                compilerParams.CompilerOptions = "/t:library";
                compilerParams.GenerateInMemory = true;

                StringBuilder sb = new StringBuilder();

                sb.Append("using System;\n");
                sb.Append("using System.Xml;\n");
                sb.Append("using System.Data;\n");
                sb.Append("using Scripting;\n");
                sb.Append("namespace Evaluate {\n");
                sb.Append("public class EClass {\n");
                sb.Append("public object EMethod() {\n");
                sb.Append(code);
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

