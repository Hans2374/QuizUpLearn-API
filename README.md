# appsettings.json

{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=Manh-Laptop\\SQLEXPRESS;Uid=sa;Pwd=12345;Database=QuizUpLearnDB;Trusted_Connection=true;TrustServerCertificate=true;MultipleActiveResultSets=True;",
    "PostgreSqlConnection": "Host=ep-fragrant-field-a1w48qp3-pooler.ap-southeast-1.aws.neon.tech;Database=neondb;Username=neondb_owner;Password=npg_RWGbey5x7InM;SSL Mode=Require;Trust Server Certificate=true"
  },
  "Jwt": {
    "Key": "u8hK3pL0sXz9Nd2A4vYb7Rf5Qw9Zx1Cm",
    "Issuer": "QuizUpLearn",
    "Audience": "QuizUpLearnClient",
    "ExpiresMinutes": 120,
    "RefreshExpiresDays": 7
  },
  "Cloudinary": {
    "CloudName": "dh6m4qqhr",
    "APIKey": "739149964496264",
    "APISecret": "C3IgiXbQtwZHFjg7IeSctV59JTs"
  },
  "MailerSend": {
    "ApiKey": "mlsn.e38e2c45857322caa4a1df960b1df802ffe9a9bfbb4dd30b1eeabe3d99c2eadb",
    "FromEmail": "no-reply@test-69oxl5e79rzl785k.mlsender.net",
    "FromName": "QuizUpLearn",
    "BaseUrl": "https://api.mailersend.com",
    "EmailEndpoint": "/v1/email"
  },
  "Gemini": {
    "ApiKey": "AIzaSyBShA_ksvzBrQ6A6AWCpNxjfQT3yTZtr34"
  },
  "OpenRouter": {
      "ApiKey": "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"
  },
  "AllowedHosts": "*"
}




# How to migration
  1. Navigate to Tool -> Nuget package manager -> Package manager console
  2. Change default project to Repository
  3. Create db name QuizUpLearnDB in sql server 
  4. Run command `update-database`
