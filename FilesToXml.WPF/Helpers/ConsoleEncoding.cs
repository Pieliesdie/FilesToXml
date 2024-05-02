using System.Text;

namespace FilesToXml.WPF.Helpers;

internal sealed class ConsoleEncoding : Encoding
{
    private readonly Encoding _encoding;
    
    internal ConsoleEncoding(Encoding encoding)
    {
        _encoding = encoding;
    }
    
    public override int CodePage => _encoding.CodePage;
    public override bool IsSingleByte => _encoding.IsSingleByte;
    public override string EncodingName => _encoding.EncodingName;
    public override string WebName => _encoding.WebName;
    
    public override byte[] GetPreamble()
    {
        return Array.Empty<byte>();
    }
    
    public override int GetByteCount(char[] chars)
    {
        return _encoding.GetByteCount(chars);
    }
    
    public override int GetByteCount(char[] chars, int index, int count)
    {
        return _encoding.GetByteCount(chars, index, count);
    }
    
    public override int GetByteCount(string s)
    {
        return _encoding.GetByteCount(s);
    }
    
    public override byte[] GetBytes(char[] chars)
    {
        return _encoding.GetBytes(chars);
    }
    
    public override byte[] GetBytes(char[] chars, int index, int count)
    {
        return _encoding.GetBytes(chars, index, count);
    }
    
    public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
    {
        return _encoding.GetBytes(chars, charIndex, charCount, bytes, byteIndex);
    }
    
    public override byte[] GetBytes(string s)
    {
        return _encoding.GetBytes(s);
    }
    
    public override int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex)
    {
        return _encoding.GetBytes(s, charIndex, charCount, bytes, byteIndex);
    }
    
    public override int GetCharCount(byte[] bytes)
    {
        return _encoding.GetCharCount(bytes);
    }
    
    public override int GetCharCount(byte[] bytes, int index, int count)
    {
        return _encoding.GetCharCount(bytes, index, count);
    }
    
    public override char[] GetChars(byte[] bytes)
    {
        return _encoding.GetChars(bytes);
    }
    
    public override char[] GetChars(byte[] bytes, int index, int count)
    {
        return _encoding.GetChars(bytes, index, count);
    }
    
    public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
    {
        return _encoding.GetChars(bytes, byteIndex, byteCount, chars, charIndex);
    }
    
    public override Decoder GetDecoder()
    {
        return _encoding.GetDecoder();
    }
    
    public override Encoder GetEncoder()
    {
        return _encoding.GetEncoder();
    }
    
    public override int GetMaxByteCount(int charCount)
    {
        return _encoding.GetMaxByteCount(charCount);
    }
    
    public override int GetMaxCharCount(int byteCount)
    {
        return _encoding.GetMaxCharCount(byteCount);
    }
    
    public override string GetString(byte[] bytes)
    {
        return _encoding.GetString(bytes);
    }
    
    public override string GetString(byte[] bytes, int index, int count)
    {
        return _encoding.GetString(bytes, index, count);
    }
}