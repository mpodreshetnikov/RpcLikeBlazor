namespace RpcLikeBlazor.ApiServiceSetup.Abstractions
{
    /// <summary>
    /// Abstraction for Object Converter.
    /// </summary>
    public interface IObjectConverter
    {
        /// <summary>
        /// Convert a string to an object of certain type.
        /// </summary>
        /// <typeparam name="TOut">Type of the object.</typeparam>
        /// <param name="stringValue">String value of the object.</param>
        /// <returns>Object.</returns>
        TOut ConvertToObject<TOut>(string stringValue);

        /// <summary>
        /// Convert an object to a string value.
        /// </summary>
        /// <param name="objectValue">Object.</param>
        /// <returns>String value.</returns>
        string ConvertToString(object objectValue);
    }
}
