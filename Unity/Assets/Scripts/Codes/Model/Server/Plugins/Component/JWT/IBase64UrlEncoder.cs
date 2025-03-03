namespace ET.JWT
{
    /// <summary>
    /// Represents a base64 encoder/decoder.
    /// </summary>
    public interface IBase64UrlEncoder
    {
        /// <summary>
        /// Encodes the byte array to a base64 string.
        /// </summary>
        string Encode(byte[] input);

        /// <summary>
        /// Decodes the base64 string to a byte array.
        /// </summary>
        byte[] Decode(string input);
    }
}