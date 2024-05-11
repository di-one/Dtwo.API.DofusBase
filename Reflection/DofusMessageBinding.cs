using System.Runtime.Serialization;

namespace Dtwo.API.DofusBase
{
    [DataContract]
    public class DofusMessageBinding
    {
        [DataMember]
        public string? Identifier { get; set; }
        [DataMember]
        public string? ClassName { get; set; }
    }
}
