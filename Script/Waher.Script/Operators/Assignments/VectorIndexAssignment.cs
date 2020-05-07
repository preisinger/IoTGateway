﻿using System;
using System.Reflection;
using Waher.Script.Abstraction.Elements;
using Waher.Script.Exceptions;
using Waher.Script.Model;
using Waher.Script.Objects;
using Waher.Script.Operators.Vectors;

namespace Waher.Script.Operators.Assignments
{
	/// <summary>
	/// Vector Index Assignment operator.
	/// </summary>
	public class VectorIndexAssignment : TernaryOperator
	{
		/// <summary>
		/// Vector Index Assignment operator.
		/// </summary>
		/// <param name="VectorIndex">Vector Index</param>
		/// <param name="Operand">Operand.</param>
		/// <param name="Start">Start position in script expression.</param>
		/// <param name="Length">Length of expression covered by node.</param>
		/// <param name="Expression">Expression containing script.</param>
		public VectorIndexAssignment(VectorIndex VectorIndex, ScriptNode Operand, int Start, int Length, Expression Expression)
			: base(VectorIndex.LeftOperand, VectorIndex.RightOperand, Operand, Start, Length, Expression)
		{
		}

		/// <summary>
		/// Evaluates the node, using the variables provided in the <paramref name="Variables"/> collection.
		/// </summary>
		/// <param name="Variables">Variables collection.</param>
		/// <returns>Result.</returns>
		public override IElement Evaluate(Variables Variables)
		{
			IElement Left = this.left.Evaluate(Variables);
			IElement Index = this.middle.Evaluate(Variables);
			IElement Value = this.right.Evaluate(Variables);

			if (Left is IVector V)
			{
				double d;

				if (!(Index is DoubleNumber IE) || (d = IE.Value) < 0 || d > int.MaxValue || d != Math.Truncate(d))
					throw new ScriptRuntimeException("Index must be a non-negative integer.", this);

				V.SetElement((int)d, Value);

				return Value;
			}
			else if (Left.IsScalar)
			{
				object Object = Left.AssociatedObjectValue;
				if (Object is null)
					throw new ScriptRuntimeException("Vector is null.", this);

				Type T = Object.GetType();
				PropertyInfo ItemProperty = T.GetRuntimeProperty("Item");
				ParameterInfo[] Parameters = ItemProperty?.GetIndexParameters();

				if (Parameters is null || Parameters.Length != 1)
					throw new ScriptRuntimeException("Vector element assignment operates on vectors.", this);

				if (Index.TryConvertTo(Parameters[0].ParameterType, out object IndexValue))
				{
					ItemProperty.SetValue(Object, Value.AssociatedObjectValue, new object[] { IndexValue });
					return Value;
				}
				else
					throw new ScriptRuntimeException("Provided index value not compatible with expected index type.", this);
			}
			else
				throw new ScriptRuntimeException("Vector element assignment can only be performed on vectors or on objects with a suitable index property defined.", this);
		}

	}
}
