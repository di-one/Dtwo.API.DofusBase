using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dtwo.API.DofusBase
{
	[DataContract]
	public class DofusTypeBinding
	{
		[DataMember]
		public string? Identifier { get; set; }
		[DataMember]
		public string? ClassName { get; set; }
	}
}
