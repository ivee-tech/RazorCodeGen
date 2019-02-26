
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

using Iv.Validation;
using Iv.Data;

namespace Iv.Common
{
	public partial class ObjectDefPropertyValidation : ObjectDefBase<int>
    {

		public ObjectDefPropertyValidation()
	    {
	    }
			
		public static ObjectDefPropertyValidation Create(
			System.Int32 id,
			System.String objDefName,
			System.String propertyName,
            ValidationType validationType,
			System.String validationMessage,
			System.Decimal? minValue,
			System.Decimal? maxValue,
			System.String expression
		)
		{
            ObjectDefPropertyValidation model = new ObjectDefPropertyValidation();
			model.Id = id;
			model.ObjectDefName = objDefName;
			model.PropertyName = propertyName;
			model.ValidationType = validationType;
			model.ValidationMessage = validationMessage;
			model.MinValue = minValue;
			model.MaxValue = maxValue;
			model.Expression = expression;
			return model;
		}

        public static ObjectDefPropertyValidation CreateDefault(string modelName, string propertyName)
        {
            ObjectDefPropertyValidation m = new ObjectDefPropertyValidation();
            m.Id = KeyGenerator.GetRandomNumber(1, 10000);
            if(Validator.IsIdentifier(modelName)) m.ObjectDefName = modelName;
            if(Validator.IsIdentifier(propertyName)) m.PropertyName = propertyName;
            m.ValidationMessage = "Invalid property name.";
            m.ValidationType = Validator.RequiredValidationType;
            return m;
        }

        [IgnoreColumn(DataOperation.Create)]
		public System.Int32 Id { get; set; }
		public System.String ObjectDefName { get; set; }
		public System.String PropertyName { get; set; }
		public ValidationType ValidationType { get; set; }
		public System.String ValidationMessage { get; set; }
		public System.Decimal? MinValue { get; set; }
		public System.Decimal? MaxValue { get; set; }
		public System.String Expression { get; set; }
        [IgnoreColumn(DataOperation.All)]
        public string ValidationTypeText { get; set; }

		public override void SetDeleted()
		{
			base.SetDeleted();
		}
    }
}

 
