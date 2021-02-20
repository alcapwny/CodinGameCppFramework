using Sharpmake; // contains the entire Sharpmake object library.

using System;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;

[module: Sharpmake.Include("cgfpuzzle.sharpmake.cs")]

class CGFPuzzleProjectGenerationParam
{
    public Type ProjectType { get; set; }
    public string FileSearchPattern { get; set; }
}

class CGFPuzzleProjectGenerator : CGFProject
{
    public CGFPuzzleProjectGenerator()
    {
        FolderName = CGFPuzzleProject.RootFolderName;
    }

    //Generate a list of types that corresponds Sharpmake projects. One project is created for each file matching the search pattern (.cpp, .cs, etc).
    // For example, this allows each cpp file to have its own main() function.
    // To do this we create a dynamic assembly and generate custom types at runtime. These types are children of the passed in types, 
    // such as CGFPuzzleProject. This is needed as you cannot add multiple instances of the same project type in a Sharpmake solution, 
    // which makes sense to resolve project dependencies.
    // Projects can be placed in sub-folders which will be used to create filters within the project.
    // rootPath: Expected to point to the path where all project folders are. Expected structure:
    //      rootPath/ProjectA/source/ProjectA.cpp
    //      rootPath/ProjectB/source/ProjectB.cpp
    //      rootPath/FilterA/ProjectC/source/ProjectC.cpp
    //      rootPath/FilterB/FilterC/ProjectD/source/ProjectD.cpp
    //      ...
    // projectGenerationParam: The list of project types to generate with their search pattern
    //      CGFPuzzleProject - *.cpp
    //      CGFPuzzleCSharpProject - *.cs
    //      Note: This assumes a constructor of type ProjectType(string projectName, string folderName, string filterName) 
    public List<Type>[] GenerateCodinGamePuzzleProjects(string rootPath, CGFPuzzleProjectGenerationParam[] projectGenerationParam)
    {
        //Create a dynamic assembly to which will host a child type of the types passed in projectGenerationParam for each file found matching the provided search pattern
        // Example, one CGFPuzzleProject for each cpp
        AssemblyName assemblyName = new AssemblyName("CGFGeneratedPuzzlesProjects");
        AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);
        List<Type>[] generatedTypes = new List<Type>[projectGenerationParam.Length];
        for(int i = 0; i < generatedTypes.Length; ++i)
        {
            generatedTypes[i] = new List<Type>();
        }

        //Get path to search for CodinGame puzzle files from
        string codingamePuzzlesPath = rootPath + @"\" + SourceRootPath;
        {
            Resolver resolver = new Resolver();
            resolver.SetParameter("project", this);
            codingamePuzzlesPath = resolver.Resolve(codingamePuzzlesPath);
        }
        string resolvedCodinGamePuzzlesPath = Util.GetCapitalizedPath(codingamePuzzlesPath);
        DirectoryInfo codingamePuzzlesDirectoryInfo = new DirectoryInfo(resolvedCodinGamePuzzlesPath);
        string resolvedCodinGamePuzzlesDirectoryPath = Util.GetCapitalizedPath(codingamePuzzlesDirectoryInfo.FullName);

        //Create a csharpProvider to validate generated project type names
        CodeDomProvider csharpProvider = CodeDomProvider.CreateProvider("C#");

        //Create the attribute builder that will add the [Generate] attribute to the project
        CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(typeof(Sharpmake.Generate).GetConstructor(Type.EmptyTypes), new object[] { });

        for (int i = 0; i < projectGenerationParam.Length; ++i)
        {
            CGFPuzzleProjectGenerationParam generationParam = projectGenerationParam[i];
            // Query files in path puzzle directory
            string[] filePathList = Util.DirectoryGetFiles(resolvedCodinGamePuzzlesDirectoryPath, generationParam.FileSearchPattern, SearchOption.AllDirectories);
            List<string> relativeFilePathList = Util.PathGetRelative(resolvedCodinGamePuzzlesDirectoryPath, new Strings(filePathList));

            //Get ProjectType(string projectName, string folderName, string filterName) constructor
            ConstructorInfo codingamePuzzleProjectConstructor = generationParam.ProjectType.GetConstructor(new Type[] { typeof(string), typeof(string), typeof(string) });

            //Generate project types
            foreach (string relativeFilePath in relativeFilePathList)
            {
                string fileName;
                string pathName;
                Util.PathSplitFileNameFromPath(relativeFilePath, out fileName, out pathName);
                fileName = Path.GetFileNameWithoutExtension(fileName);

                //Remove /source from pathName
                if (pathName.EndsWith(Path.DirectorySeparatorChar + "source") || pathName.EndsWith(Path.AltDirectorySeparatorChar + "source"))
                {
                    pathName = Path.GetDirectoryName(pathName);
                }

                string filterName = Path.GetDirectoryName(pathName);
                string projectName = Path.GetFileName(pathName); //Use GetFileName to get the last directory name...

                string projectTypeName = projectName + fileName; //Note: This means that conflicts can occur if the same project folder/filename combination in different sub-folders exists
                if (!csharpProvider.IsValidIdentifier(projectTypeName))
                {
                    throw new Error("Project name ({0}) generated from ({1}) does not comply with C# identifer rules. Update your path/filename to remove illegal characters in order to create a valid type name.", projectTypeName, relativeFilePath);
                }

                //Generate a type of the following format:
                //[Generate]
                //class GeneratedProjectType : ProjectType
                //{
                //    GeneratedProjectType() : ProjectType("fileName", "pathName", "filterName") { }
                //}
                TypeBuilder typeBuilder = moduleBuilder.DefineType(projectTypeName, TypeAttributes.Public | TypeAttributes.Class, generationParam.ProjectType);
                typeBuilder.SetCustomAttribute(customAttributeBuilder);

                //Generate constructor + the constructor code
                ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.HasThis, Type.EmptyTypes);
                ILGenerator constructorIL = constructorBuilder.GetILGenerator();

                // Generate constructor code
                constructorIL.Emit(OpCodes.Ldarg_0);         //push "this" onto stack.
                constructorIL.Emit(OpCodes.Ldstr, projectName); //push the project name onto the stack
                constructorIL.Emit(OpCodes.Ldstr, pathName); //push the folder name onto the stack
                constructorIL.Emit(OpCodes.Ldstr, filterName); //push the filterName name onto the stack
                constructorIL.Emit(OpCodes.Call, codingamePuzzleProjectConstructor); //call ProjectType(string projectName, string folderName, string filterName) constructor
                constructorIL.Emit(OpCodes.Ret);             //return

                Type generatedProjectType = typeBuilder.CreateType();
                generatedTypes[i].Add(generatedProjectType);
            }
        }

        return generatedTypes;
    }
}
