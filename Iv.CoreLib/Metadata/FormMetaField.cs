using Iv.Common;
using System;
using System.Collections.Generic;

namespace Iv.Metadata
{
    public class FormMetaField : ObjectDefBase<Guid>
    {
        public Guid Id { get; set; } = Guid.Empty;
        // public string Name { get; set; }
        public string Label { get; set; }
        public  string Description { get; set; }
        public FieldType Type { get; set; } = FieldType.none;
        public List<FormMetaValidation> Validations { get; set; } = new List<FormMetaValidation>();
        public Guid FormMetaId { get; set; }
        /*
        public string SType
        {
            get
            {
                return Type.ToString();
            }
            set
            {
                FieldType ft = FieldType.none;
                Type = Enum.TryParse<FieldType>(value, out ft) ? ft : FieldType.none;
            }
        }
        */
        public int? Length { get; set; }

        public string ColumnName { get; set; }
        public bool IsKey { get; set; }

        public Guid? DataSourceId { get; set; }

        public override void SetRefRelationship()
        {
            foreach (var v in Validations)
            {
                v.FormMetaFieldId = Id;
            }
        }
    }

    public enum FieldType
    {
        none = 0,
        text = 1,
        number = 2,
        checkbox = 3,
        date = 4,
        dropdown = 5,
        guid = 6
    }
}