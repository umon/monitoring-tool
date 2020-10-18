# monitoring-tool
Monitoring Tool Example (.Net Core MVC)

Set your **ConnectionString**, **SendGrid_APIKEY** and **SendGrid_Mail** in *appSettings.json* and run!

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "ConnectionString"
  },
  "SendGrid": {
    "ApiKey": "SendGrid_APIKEY",
    "Mail": "SendGrid_Mail"
  }
}
```

