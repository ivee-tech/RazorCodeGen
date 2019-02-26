using Iv.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Iv.Metadata
{
    public class FormMeta : ObjectDefBase<Guid>
    {

        public FormMeta()
        {
            this.Fields = new List<FormMetaField>();
            this.Views = new List<FormMetaView>();
        }

        public Guid Id { get; set; } = Guid.Empty;
        public override Guid Key
        {
            get
            {
                return Id;
            }

            set
            {
                Id = value;
            }
        }
        // public string Name { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public string TableName { get; set; }
        public Guid? ApplicationInstanceId { get; set; }
        public string ListName { get; set; }
        public List<FormMetaField> Fields { get; set; }
        public List<FormMetaView> Views { get; set; }

        public override void SetRefRelationship()
        {
            SetFormMetaIdFields();
            SetFormMetaIdViews();
        }

        public bool HasKey
        {
            get
            {
                return this.Fields.Any(fld => fld.IsKey);
            }
        }

        public FormMetaField KeyField
        {
            get
            {
                return this.Fields.FirstOrDefault(fld => fld.IsKey);
            }
        }

        private void SetFormMetaIdFields()
        {
            foreach (var f in Fields)
            {
                f.FormMetaId = Id;
                f.SetRefRelationship();
            }
        }

        private void SetFormMetaIdViews()
        {
            foreach (var v in Views)
            {
                v.FormMetaId = Id;
                v.SetRefRelationship();
            }
        }

    }
}