using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using System;
using System.Configuration;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using PublicClientApplication = Microsoft.Identity.Client.PublicClientApplication;

namespace EmployeeDeactivation.BusinessLayer
{
    public class LoginOperation
    {
        

    //    static async Task<string> GetTokenAsync(string clientApp)
    //    {
    //        //need to pass scope of activity to get token  
    //        string[] Scopes = { "User.Read" };
    //        string token = null;
    //        AuthenticationResult authResult = await clientApp.AcquireTokenAsync(Scopes);
    //        token = authResult.AccessToken;
    //        return token;
    //    }
    //}

    //public class SampleConfiguration
    //{
    //    /// <summary>
    //    /// Authentication options
    //    /// </summary>
    //    public PublicClientApplicationOptions PublicClientApplicationOptions { get; set; }

    //    /// <summary>
    //    /// Base URL for Microsoft Graph (it varies depending on whether the application is ran
    //    /// in Microsoft Azure public clouds or national / sovereign clouds
    //    /// </summary>
    //    public string MicrosoftGraphBaseEndpoint { get; set; }

    //    /// <summary>
    //    /// Reads the configuration from a json file
    //    /// </summary>
    //    /// <param name="path">Path to the configuration json file</param>
    //    /// <returns>SampleConfiguration as read from the json file</returns>
    //    public static SampleConfiguration ReadFromJsonFile(string path)
    //    {
    //        // .NET configuration
    //        IConfigurationRoot Configuration;
    //        var builder = new ConfigurationBuilder()
    //          .SetBasePath(Directory.GetCurrentDirectory())
    //        .AddJsonFile(path);
    //        Configuration = builder.Build();

    //        // Read the auth and graph endpoint config
    //        SampleConfiguration config = new SampleConfiguration()
    //        {
    //            PublicClientApplicationOptions = new PublicClientApplicationOptions()
    //        };
    //        Configuration.Bind("Authentication", config.PublicClientApplicationOptions);
    //        config.MicrosoftGraphBaseEndpoint = Configuration.GetValue<string>("WebAPI:MicrosoftGraphBaseEndpoint");
    //        return config;
    //    }
    }
}
