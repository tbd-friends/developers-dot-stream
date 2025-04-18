using System.Net.Http.Json;
using Ardalis.Result;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.Extensions.Options;

namespace Developers.Stream.Infrastructure.YouTube;

public class YouTubeClient(
    IOptions<YouTubeConfiguration> configuration
) : IYouTubeClient
{
    private readonly YouTubeConfiguration _configuration = configuration.Value;
    private GoogleAuthorizationCodeFlow.Initializer _codeFlowInitializer;

    public async Task<Result<string>> FetchChannelNameUsingAuthenticationCode(string code)
    {
        _codeFlowInitializer = new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets
            {
                ClientId = _configuration.ClientId,
                ClientSecret = _configuration.Secret
            },
            Scopes =
            [
                "https://www.googleapis.com/auth/youtube.readonly"
            ]
        };

        var flow = new GoogleAuthorizationCodeFlow(_codeFlowInitializer);

        TokenResponse tokenResponse = await flow.ExchangeCodeForTokenAsync(
            "user",
            code,
            _configuration.CodeFlowRedirectUri,
            CancellationToken.None);

        var channelName = await GetYouTubeChannelInfo(tokenResponse);

        return channelName is null ? Result.NotFound() : Result.Success(channelName);
    }

    private async Task<string?> GetYouTubeChannelInfo(TokenResponse tokenResponse)
    {
        // Create the YouTube service with the access token
        var credential = new UserCredential(
            new GoogleAuthorizationCodeFlow(
                _codeFlowInitializer
            ),
            "user", // Same user ID used above
            tokenResponse);

        var youtubeService = new YouTubeService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "DevelopersStream"
        });

        var channelsListRequest = youtubeService.Channels.List("snippet");
        
        channelsListRequest.Mine = true;

        var channelsListResponse = await channelsListRequest.ExecuteAsync();

        return channelsListResponse.Items.Count > 0 ? channelsListResponse.Items[0]?.Snippet.CustomUrl : null;
    }
}