//-----------------------------------------------------------------------
// <copyright file="CSharpGeneratorSettings.cs" company="NJsonSchema">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/rsuter/NJsonSchema/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using System;
using System.Linq;
using NJsonSchema.CodeGeneration.TypeScript.Templates;

namespace NJsonSchema.CodeGeneration.TypeScript
{
    /// <summary>The generator settings.</summary>
    public class TypeScriptGeneratorSettings : CodeGeneratorSettingsBase
    {
        private string _extensionCode;
        private string[] _extendedClasses;
        private ExtensionCode _processedExtensionCode;

        /// <summary>Initializes a new instance of the <see cref="TypeScriptGeneratorSettings"/> class.</summary>
        public TypeScriptGeneratorSettings()
        {
            ModuleName = "";
            GenerateReadOnlyKeywords = true;
            TypeStyle = TypeScriptTypeStyle.Interface;
            DateTimeType = TypeScriptDateTimeType.Date;
            ExtensionCode = string.Empty;
        }

        /// <summary>Gets or sets a value indicating whether to generate the readonly keywords (only available in TS 2.0+, default: true).</summary>
        public bool GenerateReadOnlyKeywords { get; set; }

        /// <summary>Gets or sets the type style (experimental, default: Interface).</summary>
        public TypeScriptTypeStyle TypeStyle { get; set; }

        /// <summary>Gets or sets the date time type (default: 'Date').</summary>
        public TypeScriptDateTimeType DateTimeType { get; set; }

        /// <summary>Gets or sets the TypeScript module name (default: '', no module).</summary>
        public string ModuleName { get; set; }

        /// <summary>Gets or sets the list of extended classes (the classes must be implemented in the <see cref="ExtensionCode"/>).</summary>
        public string[] ExtendedClasses
        {
            get { return _extendedClasses; }
            set
            {
                if (value != _extendedClasses)
                {
                    _extendedClasses = value;
                    _processedExtensionCode = null;
                }
            }
        }

        /// <summary>Gets or sets the extension code to append to the generated code.</summary>
        public string ExtensionCode
        {
            get { return _extensionCode; }
            set
            {
                if (value != _extensionCode)
                {
                    _extensionCode = value;
                    _processedExtensionCode = null; 
                }
            }
        }

        /// <summary>Gets or sets the type names which always generate plain TypeScript classes.</summary>
        public string[] ClassTypes { get; set; }

        /// <summary>Gets the transformed additional code.</summary>
        public ExtensionCode ProcessedExtensionCode
        {
            get
            {
                if (_processedExtensionCode == null)
                    _processedExtensionCode = new TypeScriptExtensionCode(ExtensionCode ?? string.Empty, ExtendedClasses); 

                return _processedExtensionCode;
            }
        }

        internal ITemplate CreateTemplate(string typeName)
        {
            if (ClassTypes != null && ClassTypes.Contains(typeName))
                return new ClassTemplate();

            if (TypeStyle == TypeScriptTypeStyle.Interface)
                return new InterfaceTemplate();

            if (TypeStyle == TypeScriptTypeStyle.Class)
                return new ClassTemplate();

            if (TypeStyle == TypeScriptTypeStyle.KnockoutClass)
                return new KnockoutClassTemplate();

            throw new NotImplementedException();
        }

        /// <summary>Gets the type style of the given type name.</summary>
        /// <param name="typeName">The type name.</param>
        /// <returns>The type style.</returns>
        public TypeScriptTypeStyle GetTypeStyle(string typeName)
        {
            if (ClassTypes != null && ClassTypes.Contains(typeName))
                return TypeScriptTypeStyle.Class;

            return TypeStyle;
        }
    }
}