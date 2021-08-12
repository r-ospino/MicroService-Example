using System;
using System.Runtime.Serialization;

namespace Weelo.RafaelOspino.Domain
{
    /// <summary>
    /// Represents errors that occur in business logic.
    /// </summary>
    [Serializable]
    public class DomainException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainException"/> class.
        /// </summary>
        public DomainException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainException"/> class.
        /// </summary>
        /// <param name="message"><inheritdoc path="/param[@name='message']"/></param>
        public DomainException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainException"/> class.
        /// </summary>
        /// <param name="message"><inheritdoc path="/param[@name='message']"/></param>
        /// <param name="innerException"><inheritdoc path="/param[@name='innerException']"/></param>
        public DomainException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainException"/> class.
        /// </summary>
        /// <param name="info"><inheritdoc path="/param[@name='info']"/></param>
        /// <param name="context"><inheritdoc path="/param[@name='context']"/></param>
        protected DomainException(SerializationInfo info,StreamingContext context) : base(info, context) { }
    }
}
