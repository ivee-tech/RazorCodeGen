using Iv.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Metadata
{
    public interface IFormMetaDomain
    {
        FormMeta GetFormMeta(Guid id);
        IEnumerable<FormMeta> GetFormMetas();
        FormMeta CreateFormMeta(FormMeta form);
        FormMeta UpdateFormMeta(FormMeta form);
        void DeleteFormMeta(FormMeta form);
        void DeactivateFormMeta(FormMeta form);
        bool Exists(Guid id);
        IEnumerable<FormMeta> GetFormMetas(Guid? applicationInsatnceId);
        FormMeta GenerateNewFormMeta();

    }
}
