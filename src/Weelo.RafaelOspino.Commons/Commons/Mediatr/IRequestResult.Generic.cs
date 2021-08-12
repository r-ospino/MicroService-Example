namespace Weelo.RafaelOspino.Commons.Mediatr
{
    /// <inheritdoc cref="IRequestResult" path="/summary"/>
    /// <remarks>For non empty response</remarks>
    /// <typeparam name="T">Command/Query response type</typeparam>
    public interface IRequestResult<T> : IRequestResult
    {
        /// <summary>
        /// Gets the Query/Command respose.
        /// </summary>
        public T Payload { get; } 
    }
}
