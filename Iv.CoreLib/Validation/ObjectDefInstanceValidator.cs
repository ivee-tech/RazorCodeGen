using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

using Iv.Common;

namespace Iv.Validation
{
    public class ObjectDefInstanceValidator
    {
        private ObjectDef _m;
        private ObjectDefInstance _mv;

        public ObjectDefInstanceValidator(ObjectDef m, ObjectDefInstance mv)
        {
            if (m == null)
                throw new ArgumentNullException("Object Def", "The Object Definition cannot be null.");
            if (mv == null)
                throw new ArgumentNullException("Object Def Instance", "The Object Definition instance cannot be null.");
            _m = m;
            _mv = mv;
        }

        public virtual bool IsValid(string propertyName, ValidationType validation)
        {
            var mp = _m.GetProperty(propertyName);
            if (mp == null) return false;
            var mpv = _m.Validations.Where(p => p.ValidationType == validation).FirstOrDefault();
            if (mpv == null) return true; //if no such validation, continue
            var bResult = false;
            switch (mpv.ValidationType)
            {
                case ValidationType.Required:
                    bResult = ValidateRequired(propertyName);
                    break;
                case ValidationType.Range:
                    bResult = ValidateRange(propertyName, mpv);
                    break;
                case ValidationType.StringLength:
                    bResult = ValidateStringLength(propertyName, mpv);
                    break;
                case ValidationType.RegularExpression:
                    bResult = ValidateRegularExpression(propertyName, mpv);
                    break;
                case ValidationType.Identifier:
                    bResult = ValidateIdentifier(propertyName, mpv);
                    break;
                case ValidationType.Email:
                    bResult = ValidateEmail(propertyName, mpv);
                    break;
                default:
                    break;
            }
            return bResult;
        }

        public virtual string GetErrorMessage(string propertyName, ValidationType validation)
        {
            var mp = _m.GetProperty(propertyName);
            if (mp == null) return string.Empty;
            var mpv = new ObjectDefPropertyValidation(); // mp.GetValidation(validation);
            if (mpv == null) return string.Empty; 
            return mpv.ValidationMessage;
        }

        public virtual void Validate(string propertyName, ValidationType validation, out string errorMessage)
        {
            errorMessage = string.Empty;
            if(!IsValid(propertyName, validation))
            {
                errorMessage = GetErrorMessage(propertyName, validation);
            }
        }

        public virtual void Validate(string propertyName, out string errorMessages)
        {
            errorMessages = string.Empty;
            StringBuilder sb = new StringBuilder();
            var mp = _m.GetProperty(propertyName);
            if (mp == null) return;
            foreach (var mpv in _m.Validations.Where(p => p.PropertyName == propertyName))
            {
                if (!IsValid(propertyName, (ValidationType)mpv.ValidationType))
                {
                    sb.AppendLine(mpv.ValidationMessage);
                }
            }
            errorMessages = sb.ToString().Trim();
        }

        public virtual void Validate(out string errorMessages)
        {
            errorMessages = string.Empty;
            StringBuilder sb = new StringBuilder();
            var mpvs = _m.Validations;
            foreach (var mpv in mpvs)
            {
                if (!IsValid(mpv.PropertyName, (ValidationType)mpv.ValidationType))
                {
                    sb.AppendLine(mpv.ValidationMessage);
                }
            }
            errorMessages = sb.ToString().Trim();
        }

        private bool ValidateRequired(string propertyName)
        {
            ValidationAttribute attr = new RequiredAttribute();
            ObjectDefInstanceValidationAttribute target = new ObjectDefInstanceValidationAttribute(attr, _mv);
            return target.IsValid(propertyName);
        }

        private bool ValidateRange(string propertyName, ObjectDefPropertyValidation mpv)
        {
            ValidationAttribute attr = new RangeAttribute((double)mpv.MinValue, (double)mpv.MaxValue);
            ObjectDefInstanceValidationAttribute target = new ObjectDefInstanceValidationAttribute(attr, _mv);
            return target.IsValid(propertyName);
        }

        private bool ValidateStringLength(string propertyName, ObjectDefPropertyValidation mpv)
        {
            int maxLength = -1;
            if (!int.TryParse(mpv.Expression, out maxLength)) return true; //if validator expression is wrong (misconfiguration), continue
            ValidationAttribute attr = new StringLengthAttribute(maxLength);
            ObjectDefInstanceValidationAttribute target = new ObjectDefInstanceValidationAttribute(attr, _mv);
            return target.IsValid(propertyName);
        }

        private bool ValidateRegularExpression(string propertyName, ObjectDefPropertyValidation mpv)
        {
            ValidationAttribute attr = new RegularExpressionAttribute(mpv.Expression);
            ObjectDefInstanceValidationAttribute target = new ObjectDefInstanceValidationAttribute(attr, _mv);
            return target.IsValid(propertyName);
        }

        private bool ValidateEmail(string propertyName, ObjectDefPropertyValidation mpv)
        {
            ValidationAttribute attr = new EmailAttribute();
            ObjectDefInstanceValidationAttribute target = new ObjectDefInstanceValidationAttribute(attr, _mv);
            return target.IsValid(propertyName);
        }

        private bool ValidateIdentifier(string propertyName, ObjectDefPropertyValidation mpv)
        {
            ValidationAttribute attr = new IdentifierAttribute();
            ObjectDefInstanceValidationAttribute target = new ObjectDefInstanceValidationAttribute(attr, _mv);
            return target.IsValid(propertyName);
        }
    }
}
