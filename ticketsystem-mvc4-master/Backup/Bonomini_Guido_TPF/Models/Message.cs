//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bonomini_Guido_TPF.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Message
    {
        public int MessageID { get; set; }
        public string MessageSubject { get; set; }
        public string MessageContent { get; set; }
        public string MessageType { get; set; }
        public string MessageUserName { get; set; }
        public int MessageReceptorID { get; set; }
    
        public virtual User User { get; set; }
    }
}
