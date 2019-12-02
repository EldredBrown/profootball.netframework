using System;
using System.Runtime.Serialization;

namespace EldredBrown.ProFootball.WpfApp
{
    /// <summary>
    /// Exception for reporting data validation errors
    /// </summary>
    public class DataValidationException : Exception
	{
        /// <summary>
        /// Initializes a new instance of the DataValidationException class
        /// </summary>
        /// <param name="message"></param>
		public DataValidationException(string message)
			: base(message)
		{
		}

        /// <summary>
        /// Initializes a new instance of the DataValidationException class
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
		public DataValidationException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

        /// <summary>
        /// Initializes a new instance of the DataValidationException class
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
		public DataValidationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

        /// <summary>
        /// Override of Object.Equals method for equality testing
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var e = obj as DataValidationException;

            return (this.Message == e.Message);
        }

	    /// <summary>
	    /// To prevent build warning, override GetHashCode() when overriding Equals(Object obj).
	    /// </summary>
	    /// <returns></returns>
	    public override int GetHashCode()
	    {
	        return base.GetHashCode();
	    }
	}
}
