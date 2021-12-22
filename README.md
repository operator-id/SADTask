# SADTask
SAD task which implements AWS Elastic Search (now OpenSearch)


Requirements:
- .NET 6.0 SDK
- Setup an SSL certificate for the API app

Important: Make sure to provide the keys first in order for the API to function, otherwise the server will drop the error silently.

In order to authorize to the AWS you need to provide the user secrets:
- "AWSOpenSearch:AccessKey" : {ACCESS_KEY}
- "AWSOpenSearch:SecretKey" : {SECRET_KEY}
(by running 'dotnet user-secrets init' inside the project folder, and then setting the keys - 'dotnet user-secrets set "Movies:ServiceApiKey" "12345"')

Also you can change the AWS OpenSearch URL in the appsettings.json to a custom one, in the 'AWSOpenSearch:DomainUrl'.

You can test the indexing API by launching the ConsoleIndexingApp. You will need to provide the URL of the API.
The app will automatically read and send the files to the API.

The search API call can be tested in the SearchDesktopApp. You will need to provide the URL of the API.
