# developers-dot-stream

## Requirements

- .NET 9 (latest)
- Docker


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
```
twitch: { 
	clientId: "",
	secret: ""
}
```

You should now get a Twitch button appear on Register and Login screens

#### Allow Linking Accounts

After configuring the ability to login, you need to configure the API to allow linking accounts. 

- Navigate to Developers.Stream.Api in Backend.
- Repeat the previous steps for adding the client id/secret
