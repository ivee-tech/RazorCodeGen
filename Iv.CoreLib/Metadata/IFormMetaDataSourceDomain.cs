using Iv.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Metadata
{
    public interface IFormMetaDataSourceDomain
    {

        IEnumerable<FormMetaDataSource> GetDataSources();
        FormMetaDataSource GetDataSource(Guid id);
        FormMetaDataSource CreateDataSource(FormMetaDataSource dataSource);
        FormMetaDataSource UpdateDataSource(FormMetaDataSource dataSource);
        void DeleteDataSource(FormMetaDataSource dataSource);
        void DeactivateDataSource(FormMetaDataSource dataSource);
        bool Exists(Guid id);
        IEnumerable<KV<object, object>> GetValues(Guid id);

    }
}
