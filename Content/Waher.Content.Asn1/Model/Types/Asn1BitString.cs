﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Waher.Content.Asn1.Model.Types
{
	/// <summary>
	/// BIT STRING
	/// </summary>
	public class Asn1BitString : Asn1Type
	{
		/// <summary>
		/// BIT STRING
		/// </summary>
		/// <param name="Implicit">Implicit type definition</param>
		public Asn1BitString(bool Implicit)
			: base(Implicit)
		{
		}

		/// <summary>
		/// C# type reference.
		/// </summary>
		public override string CSharpTypeReference => "byte[]";

		/// <summary>
		/// If type is nullable.
		/// </summary>
		public override bool CSharpTypeNullable => true;
	}
}