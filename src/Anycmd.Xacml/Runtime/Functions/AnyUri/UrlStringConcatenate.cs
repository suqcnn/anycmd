using Anycmd.Xacml.Interfaces;
using System;
using System.IO;

// ReSharper disable once CheckNamespace
namespace Anycmd.Xacml.Runtime.Functions
{
    /// <summary>
    /// Function implementation, in order to check the function behavior use the value of the Id
    /// property to search the function in the specification document.
    /// </summary>
    public class UrlStringConcatenate : FunctionBase
    {
        #region IFunction Members

        /// <summary>
        /// The id of the function, used only for notification.
        /// </summary>
        public override string Id
        {
            get { return Consts.Schema2.InternalFunctions.UrlStringConcatenate; }
        }

        /// <summary>
        /// Evaluates the function.
        /// </summary>
        /// <param name="context">The evaluation context instance.</param>
        /// <param name="args">The function arguments.</param>
        /// <returns>The result value of the function evaluation.</returns>
        public override EvaluationValue Evaluate(EvaluationContext context, params IFunctionParameter[] args)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (args == null) throw new ArgumentNullException("args");
            var uri = new UriBuilder(GetAnyUriArgument(args, 0));
            for (int i = 1; i < args.Length; i++)
            {
                uri.Path = Path.Combine(uri.Path, GetStringArgument(args, i));
            }
            return new EvaluationValue(uri.Uri, DataTypeDescriptor.AnyUri);
        }

        /// <summary>
        /// The data type of the return value.
        /// </summary>
        public override IDataType Returns
        {
            get { return DataTypeDescriptor.AnyUri; }
        }

        /// <summary>
        /// Whether the function defines variable arguments. The data type of the variable arguments will be the 
        /// data type of the last parameter.
        /// </summary>
        public override bool VarArgs
        {
            get { return true; }
        }

        /// <summary>
        /// Defines the data types for the function arguments.
        /// </summary>
        public override IDataType[] Arguments
        {
            get
            {
                return new IDataType[] { DataTypeDescriptor.AnyUri, DataTypeDescriptor.String };
            }
        }

        #endregion
    }
}
