//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Churn
{
    using System;
    using System.Collections.Generic;
    
    public partial class Changeset
    {
        public Changeset()
        {
            this.Changes = new HashSet<Change>();
        }
    
        public int Id { get; set; }
        public System.DateTime CreationDate { get; set; }
        public string CommitterDisplayName { get; set; }
        public string Comment { get; set; }
        public ChangesetStatus Status { get; set; }
        public long ChangesetId { get; set; }
    
        public virtual Project Project { get; set; }
        public virtual ICollection<Change> Changes { get; set; }
    }
}