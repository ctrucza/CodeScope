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
    
    public partial class ItemVersion
    {
        public int Id { get; set; }
        public long Version { get; set; }
        public string Stream { get; set; }
        public long Encoding { get; set; }
    
        public virtual Item Item { get; set; }
        public virtual Change Change { get; set; }
    }
}