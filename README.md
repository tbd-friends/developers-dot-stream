# developers-dot-stream


# Things to remember

- The password is hard-coded in the Aspire setup to allow us to maintain data between restarts. 
- There are other ways, see https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/persist-data-volumes

# Steps to get started


# Twitch Application

- Goto dev.twitch.tv, sign-in with your Twitch Account. 
- Navigate to Applications, and register a new application
- Call it what you want, but set the callback url to https://localhost:7102/signin-twitch 
- Category; Other
- ClientType; Confidential

Save, and then navigate back to the application

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