


using Iv.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Metadata.Domain
{
    public interface IApplicationInstanceDomain
    {
        ApplicationInstance GetApplicationInstance(Guid id);
        ApplicationInstance GetFullApplicationInstance(Guid id);
        IEnumerable<ApplicationInstance> GetApplicationInstances();
        ApplicationInstance CreateApplicationInstance(ApplicationInstance obj);
        ApplicationInstance UpdateApplicationInstance(ApplicationInstance obj);
        void DeleteApplicationInstance(ApplicationInstance obj);
        bool Exists(Guid id);
        void Compile(Guid id);
        void Validate(ApplicationInstance app, bool checkDirExist);
        void CreateWebApiProject(ApplicationInstance app);
    }
}
