﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 16.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace CGFCodeGenerator
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using CGFCodeGenerator;
    using CGFCodeGenerator.Core;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public partial class CGFHeaderCodeGenerator : CGFHeaderCodeGeneratorBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write(@"#pragma once

/////////////////////////////////////////////////////////////////////////////////////
// This file is auto generated.
// *Any modifications to this file will be lost.*
/////////////////////////////////////////////////////////////////////////////////////
");
            
            #line 14 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"

    WriteSystemIncludes(SystemIncludes);
    WriteUserIncludes(UserIncludes);

            
            #line default
            #line hidden
            this.Write("\r\n///////////////////////////////////////////////////////////////////////////////" +
                    "//////\r\n\r\n");
            
            #line 21 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"

    CGFDocument cgfDocument = CGFDocument;

    foreach(CGFTypeSymbol cgfTypeSymbol in cgfDocument.Types)
    {
        if (cgfTypeSymbol.IsEnum)
        {

            
            #line default
            #line hidden
            this.Write("enum class ");
            
            #line 29 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(cgfTypeSymbol.Name));
            
            #line default
            #line hidden
            this.Write("\r\n{\r\n");
            
            #line 31 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"

            foreach(CGFFieldSymbol cgfFieldSymbol in cgfTypeSymbol.Fields)
            {

            
            #line default
            #line hidden
            this.Write("    ");
            
            #line 35 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(cgfFieldSymbol.Name));
            
            #line default
            #line hidden
            this.Write(" = ");
            
            #line 35 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(cgfFieldSymbol.ConstantValue));
            
            #line default
            #line hidden
            this.Write(",\r\n");
            
            #line 36 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"

            }

            
            #line default
            #line hidden
            this.Write("};\r\n\r\nstd::istream& operator>>(std::istream& inputStream, ");
            
            #line 41 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(cgfTypeSymbol.Name));
            
            #line default
            #line hidden
            this.Write("& ");
            
            #line 41 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(cgfTypeSymbol.VariableName));
            
            #line default
            #line hidden
            this.Write(");\r\nstd::ostream& operator<<(std::ostream& outputStream, const ");
            
            #line 42 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(cgfTypeSymbol.Name));
            
            #line default
            #line hidden
            this.Write("& ");
            
            #line 42 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(cgfTypeSymbol.VariableName));
            
            #line default
            #line hidden
            this.Write(");\r\n");
            
            #line 43 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"

        }
        else
        {

            
            #line default
            #line hidden
            this.Write("class ");
            
            #line 48 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(cgfTypeSymbol.GeneratedName));
            
            #line default
            #line hidden
            this.Write("\r\n{\r\n");
            
            #line 50 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"

            ExtendAttribute typeExtendAttribute = cgfTypeSymbol.Attributes.GetAttribute<ExtendAttribute>();
            if (typeExtendAttribute == null || !typeExtendAttribute.HasCustomSerialization)
            {

            
            #line default
            #line hidden
            this.Write("public:\r\n    friend std::istream& operator>>(std::istream& inputStream, ");
            
            #line 56 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(cgfTypeSymbol.GeneratedName));
            
            #line default
            #line hidden
            this.Write("& ");
            
            #line 56 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(cgfTypeSymbol.VariableName));
            
            #line default
            #line hidden
            this.Write(");\r\n    friend std::ostream& operator<<(std::ostream& outputStream, const ");
            
            #line 57 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(cgfTypeSymbol.GeneratedName));
            
            #line default
            #line hidden
            this.Write("& ");
            
            #line 57 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(cgfTypeSymbol.VariableName));
            
            #line default
            #line hidden
            this.Write(");\r\n\r\n");
            
            #line 59 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"

            }

            for(int accessibility = 0; accessibility < 3; ++accessibility)
            {
                string accessibilityString;
                List<CGFFieldSymbol> fields;
                switch(accessibility)
                {
                    case 0:
                        fields = cgfTypeSymbol.PublicFields;
                        accessibilityString = "public";
                        break;
                    case 1:
                        fields = cgfTypeSymbol.ProtectedFields;
                        accessibilityString = "protected";
                        break;
                    case 2:
                        fields = cgfTypeSymbol.PrivateFields;
                        accessibilityString = "private";
                        break;
                    default:
                        fields = null;
                        accessibilityString = "error";
                        break;
                }

                if (fields.Count > 0)
                {

            
            #line default
            #line hidden
            
            #line 89 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(accessibilityString));
            
            #line default
            #line hidden
            this.Write(":\r\n");
            
            #line 90 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"

                    foreach(CGFFieldSymbol cgfFieldSymbol in fields)
                    {
                        CGFAttributeDataList fieldAttributes = cgfFieldSymbol.Attributes;

                        string cppType = CGFParserUtils.GetCPPStringForType(cgfFieldSymbol.TypeName);
                        
                        ArrayAttribute arrayAttribute = fieldAttributes.GetAttribute<ArrayAttribute>();
                        if (arrayAttribute != null)
                        {
                            //Start by indenting the line

            
            #line default
            #line hidden
            this.Write("    ");
            
            #line 101 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"


                            //Write the array type
                            List<ArrayDimension> arrayDimensions = arrayAttribute.Dimensions;
                            for (int i = (arrayDimensions.Count - 1); i >= 0; --i)
                            {
                                ArrayDimension dimension = arrayDimensions[i];
                                if (dimension.IsFixedSize)
                                {

            
            #line default
            #line hidden
            this.Write("std::array<");
            
            #line 110 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"

                                }
                                else
                                {

            
            #line default
            #line hidden
            this.Write("std::vector<");
            
            #line 114 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"

                                }
                            }


            
            #line default
            #line hidden
            
            #line 118 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(cppType));
            
            #line default
            #line hidden
            
            #line 118 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"

                            for (int i = 0; i < arrayDimensions.Count; ++i)
                            {
                                ArrayDimension dimension = arrayDimensions[i];
                                if (dimension.IsFixedSize)
                                {

            
            #line default
            #line hidden
            this.Write(", ");
            
            #line 124 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(dimension.SizeString));
            
            #line default
            #line hidden
            this.Write(">");
            
            #line 124 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"

                                }
                                else
                                {

            
            #line default
            #line hidden
            this.Write(">");
            
            #line 128 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"

                                }
                            }

            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 131 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(cgfFieldSymbol.Name));
            
            #line default
            #line hidden
            this.Write(";\r\n");
            
            #line 132 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"

                        }
                        else
                        {

            
            #line default
            #line hidden
            this.Write("    ");
            
            #line 137 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(cppType));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 137 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(cgfFieldSymbol.Name));
            
            #line default
            #line hidden
            this.Write(";\r\n");
            
            #line 138 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"

                    }
                }
            }
        }

            
            #line default
            #line hidden
            this.Write("};\r\n");
            
            #line 145 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"

    }

            
            #line default
            #line hidden
            this.Write("\r\n///////////////////////////////////////////////////////////////////////////////" +
                    "//////\r\n\r\n");
            
            #line 151 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"

    }

            
            #line default
            #line hidden
            this.Write("#include <");
            
            #line 154 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(RelativeInlinePath));
            
            #line default
            #line hidden
            this.Write(">\r\n#include <");
            
            #line 155 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(RelativeUserPath));
            
            #line default
            #line hidden
            this.Write(">\r\n");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 156 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"

    /////////////////////////////////////////////////////////////////////////////////////
    /// Function blocks
    /////////////////////////////////////////////////////////////////////////////////////

    void WriteSystemIncludes(HashSet<string> systemIncludes)
    {
        if (systemIncludes.Count > 0)
        {

        
        #line default
        #line hidden
        
        #line 165 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"
this.Write("\r\n///////////////////////////////////////////////////////////////////////////////" +
        "//////\r\n// System Includes\r\n\r\n#if !defined(CODING_EXLUDESYSTEMHEADERS)\r\n");

        
        #line default
        #line hidden
        
        #line 171 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"

            foreach(string systemInclude in SystemIncludes)
            {

        
        #line default
        #line hidden
        
        #line 174 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"
this.Write("#include <");

        
        #line default
        #line hidden
        
        #line 175 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(systemInclude));

        
        #line default
        #line hidden
        
        #line 175 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"
this.Write(">\r\n");

        
        #line default
        #line hidden
        
        #line 176 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"

            }

        
        #line default
        #line hidden
        
        #line 178 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"
this.Write("#else\r\n");

        
        #line default
        #line hidden
        
        #line 180 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"

            foreach(string systemInclude in SystemIncludes)
            {

        
        #line default
        #line hidden
        
        #line 183 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"
this.Write("CODING_EXLUDESYSTEMHEADERS#include <");

        
        #line default
        #line hidden
        
        #line 184 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(systemInclude));

        
        #line default
        #line hidden
        
        #line 184 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"
this.Write(">\r\n");

        
        #line default
        #line hidden
        
        #line 185 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"

            }

        
        #line default
        #line hidden
        
        #line 187 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"
this.Write("#endif\r\n");

        
        #line default
        #line hidden
        
        #line 189 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"

        }
    }

        
        #line default
        #line hidden
        
        #line 193 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"


    void WriteUserIncludes(HashSet<string> userIncludes)
    {
        if (userIncludes.Count > 0)
        {

        
        #line default
        #line hidden
        
        #line 199 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"
this.Write("\r\n///////////////////////////////////////////////////////////////////////////////" +
        "//////\r\n// Includes\r\n\r\n");

        
        #line default
        #line hidden
        
        #line 204 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"

            foreach(string userInclude in userIncludes)
            {

        
        #line default
        #line hidden
        
        #line 207 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"
this.Write("#include <");

        
        #line default
        #line hidden
        
        #line 208 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(userInclude));

        
        #line default
        #line hidden
        
        #line 208 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"
this.Write(">\r\n");

        
        #line default
        #line hidden
        
        #line 209 "D:\Files\Projects\CodinGameCppFramework-Private\tools\CGFCodeGenerator\CGFCodeGenerator\CGFHeaderCodeGenerator.tt"

            }
        }
    }

        
        #line default
        #line hidden
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public class CGFHeaderCodeGeneratorBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
