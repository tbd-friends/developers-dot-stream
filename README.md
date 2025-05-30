# developers-dot-stream

## Requirements

If you want to run the project, you're going to need;

- .NET 9 (latest)
- Docker
- Twitch & YouTube accounts

# Things to remember

- The password is hard-coded in the Aspire setup to allow us to maintain data between restarts. 
- There are other ways, see https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/persist-data-volumes

# Steps to get started

### Create Twitch Application

- Goto dev.twitch.tv, sign-in with your Twitch Account. 
- Navigate to Applications, and register a new application
- Configure as follows;
  - Name; Whatever you like
  - Category; Other
  - ClientType; Confidential
  - Callback URLs; 
      - https://localhost:7102/signin-twitch (required to sign-in)
      - https://localhost:7173/link-account (required to allow users to link their twitch account)
      - https://localhost:7173/profile (required to allow navigation back to the application after linking)

Save, and then navigate back to Developers.Stream.Web

- Right click on project in VS, find Manage User Secrets
- Capture Client Id and generate new secret
- Create a section in your secrets file;
```json
"twitch": { 
	"clientId": "your-client-id-here.apps.googleusercontent.com",
	"secret": "your-client-secret-here"
}
```

You should now get a Twitch button appear on Register and Login screens

### Create YouTube (Google) Application

- Go to [console.cloud.google.com](https://console.cloud.google.com), sign in with your Google Account
- Create a new project or select an existing one from the dropdown at the top
- Navigate to "APIs & Services" > "Credentials" in the left sidebar
- Click "Create Credentials" > "OAuth 2.0 Client IDs"
- If prompted, configure the OAuth consent screen first:
  - Choose "External" user type (unless you have a Google Workspace)
  - Fill in required fields (App name, User support email, Developer contact email)
  - Add your domain to authorized domains if deploying to production
  - Save and continue through the scopes and test users sections
- Back in Credentials, create OAuth 2.0 Client ID:
  - Application type: Web application
  - Name: Whatever you like (e.g., "Stream App YouTube Auth")
  - Authorized redirect URIs:
    - `https://localhost:7102/signin-google` (required to sign-in)
    - `https://localhost:7102/signin-oidc` (Google may have switched to use this for sign-in)
    - `https://localhost:7173/link-account` (required to allow users to link their YouTube account)
    - `https://localhost:7173/profile` (required to allow navigation back to the application after linking)
- Click "Create"
- Enable the YouTube Data API v3:
  - Go to "APIs & Services" > "Library"
  - Search for "YouTube Data API v3"
  - Click on it and press "Enable"

Save your credentials, then navigate back to Developers.Stream.Web

- Right click on project in VS, find "Manage User Secrets"
- Copy the Client ID and Client Secret from the Google Cloud Console
- Create a section in your secrets file:

```json
"google": {
    "clientId": "your-client-id-here.apps.googleusercontent.com",
    "clientSecret": "your-client-secret-here"
}
```

You should now get a YouTube/Google button appear on Register and Login screens

#### Allow Linking Accounts

After configuring the ability to login, you need to configure the API to allow linking accounts. 

- Navigate to Developers.Stream.Api in Backend.
- Repeat the previous steps for adding the client id/secret
