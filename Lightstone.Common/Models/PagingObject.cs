using Lightstone.Common.Enums;

namespace Lightstone.Common.Models
{
    public class PagingObject
    {
        public PagingObject() { }

        public string SortColumn { get; set; }
        public SortOrder SortOrder { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Skip { get; set; }
        public bool IncludeAllDataInd { get; set; }
    }
}
