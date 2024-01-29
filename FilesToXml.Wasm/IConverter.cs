namespace FilesToXml.Wasm;

public interface IConverter
{
    string Beautify(string xml);
    string GetBackendName();
    ConvertResult Convert(Input data);
}