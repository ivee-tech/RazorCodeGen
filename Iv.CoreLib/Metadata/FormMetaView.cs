using Iv.Common;
using System;

namespace Iv.Metadata
{
    public class FormMetaView : ObjectDefBase<Guid>
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Template { get; set; }
        public Guid FormMetaId { get; set; }
        // public string Name { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public ViewType Type { get; set; }
        public string SType
        {
            get
            {
                return Type.ToString();
            }
            set
            {
                ViewType vt = ViewType.none;
                Type = Enum.TryParse<ViewType>(value, out vt) ? vt : ViewType.none;
            }
        }
        public string Bridge { get; set; } // au, ng, vue
    }

    public enum ViewType
    {
        none = 0,
        view = 1,
        form = 2,
        grid = 3,
        report = 4,
        chart = 5
    }
}