using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AspNetCore.Components.WebView.WindowsForms;

using Microsoft.Extensions.DependencyInjection;

using MudBlazor.Services;

namespace ConverterUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Text = "Converter";
            var services = new ServiceCollection();
            services.AddWindowsFormsBlazorWebView();
            services.AddMudServices();
            services.AddMudExtensions();
            services.AddBlazorWebViewDeveloperTools();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            blazorWebView1.HostPage = "wwwroot\\index.html";
            blazorWebView1.Services = services.BuildServiceProvider();
            blazorWebView1.RootComponents.Add<Main>("#app");
        }
    }
}
