using System.Collections.Generic;

namespace CertificatesOfDeposit.Models.Utils
{
    public class GroupResult<TItem>
    {
        public object Key { get; set; }
        public int Count { get; set; }
        public IEnumerable<TItem> Item { get; set; }
        public IEnumerable<GroupResult<TItem>> SubGroups { get; set; }
        public override string ToString() { return $"{Key} ({Count})"; }
    }
}
