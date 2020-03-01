# HRapp

Web application written in ASP.NET Core MVC with usage of Entity Framework and Azure (BlobStorage, SendGrid, Database, Azure AD B2C), designed to publish job offers and apply for them.

# Getting Started 

To run this app you have to add your azure services credentials (credentials applied in code are deactivated), especially databse connection string in appsettings.json file. Additionaly you have to type "update-database -verbose" in Package Manager Console to create proper tables in database.

# Functionality

* publishing job offers
* applying for job offers
* adding CV file
* HR receiving email on new application
