using System;
using System.Runtime.Serialization;

namespace Weelo.RafaelOspino.Domain
{
    /// <summary>
    /// Represents errors that occur during external services integration.
    /// </summary>
    [Serializable]
    public class InfrastructureException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InfrastructureException"/> class.
        /// </summary>
        public InfrastructureException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainException"/> class.
        /// </summary>
        /// <param name="message"><inheritdoc path="/param[@name='message']"/></param>
        public InfrastructureException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainException"/> class.
        /// </summary>
        /// <param name="message"><inheritdoc path="/param[@name='message']"/></param>
        /// <param name="innerException"><inheritdoc path="/param[@name='innerException']"/></param>
        public InfrastructureException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainException"/> class.
        /// </summary>
        /// <param name="info"><inheritdoc path="/param[@name='info']"/></param>
        /// <param name="context"><inheritdoc path="/param[@name='context']"/></param>
        protected InfrastructureException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
