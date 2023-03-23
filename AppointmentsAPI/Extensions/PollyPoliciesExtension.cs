using Polly;
using Polly.Extensions.Http;

namespace AppointmentsAPI.Extensions;

public static class PollyPoliciesExtension
{
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    
    public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
    
    // public static void GetRetryPolicy(this IHttpClientBuilder builder) => 
    //     builder.AddPolicyHandler(HttpPolicyExtensions
    //         .HandleTransientHttpError()
    //         .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
    //         .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));
    //
    // public static void GetCircuitBreakerPolicy(this IHttpClientBuilder builder) => 
    //     builder.AddPolicyHandler(HttpPolicyExtensions
    //         .HandleTransientHttpError()
    //         .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
}