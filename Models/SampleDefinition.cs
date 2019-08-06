using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.Models
{
    public class SampleDefinition
    {
        public string Name { get; set; }
        public int SurveyId { get; set; }
        public string Language { get; set; }
        public MappingDefinition[] VariableMappings { get; set; }
        public byte[] FileData { get; set; }
        public string ImportFormat { get; set; }
        public string ImportationType { get; set; }

        public string UpdateCasesUsingVariableName { get; set; }
        public byte DuplicateEmailsOptions { get; set; }
        public bool HeaderRowIncluded { get; set; }
        public byte FieldDelimiter { get; set; }
        public bool ImportAsInactive { get; set; }
        public int? NbOfCases { get; set; }
        public bool GeneratePIN { get; set; }
        public string PINGenerationMask { get; set; }
        public string TableToImport { get; set; }
    }

    public class MappingDefinition
    {
        public string VariableName { get; set; }
        public string Field { get; set; }
        public int ColumnPosition { get; set; }
    }
}
