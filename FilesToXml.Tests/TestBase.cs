using System.Text;

namespace FilesToXml.Tests;

public class TestBase
{
    protected TestBase()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        // Do "global" initialization here; Called before every test method.
    }

    public void Dispose()
    {
        // Do "global" teardown here; Called after every test method.
    }
}