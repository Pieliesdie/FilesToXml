﻿using System.Text;
using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;

namespace ConverterToXml.Winform
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.FormClosing += Form1_FormClosing;
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

        private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}