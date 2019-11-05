# NBCNewsMock
Retrieves news headers from NBC's own website.

## Setup
This project uses .Net Core 2.1 and Microsoft Sql Server Express 2017. To run this project locally, it is assumed that you have already installed Visual Studio 2019 and Microsoft Sql Server Express 2017. If it is not installed, you can follow the links below:

[Microsoft Visual Studio 2019 Community](https://visualstudio.microsoft.com/tr/vs/community/?rr=https%3A%2F%2Fwww.google.com%2F)
[Microsoft Sql Server Express 2017](https://www.microsoft.com/tr-tr/sql-server/sql-server-editions-express)

Once you have cloned the repository, you can build the solution. 
Navigate to Package Manager Console (View > Other Windows > Package Manager Console) and select the "Default Project" as NNNDataContext as shown in the image below.

![enter image description here](https://i.ibb.co/X5RNH2x/Default-Project.png)

Enter the following command to create a database that will be used in the project.

> update-database

![enter image description here](https://i.ibb.co/cYHTfKz/Update-Database.png)

Before starting the project, make sure **NBCNewsNow** project is selected as the Start Up Project and it is configured to run in IIS Express.

![enter image description here](https://i.ibb.co/7g24MRK/Configuration.png)

Now that we have successfully setup our project, we can run it on locally by pressing ***IIS Express*** button.

## Project
When you start the project, swagger page will be displayed for easy access to endpoints.
![enter image description here](https://i.ibb.co/HqFQHfg/NNNSwagger.png)

All endpoints are protected by a Basic Authentication scheme. When you try to request a url, a prompt will be shown to enter username and password information. You can also enter your username and password with ***Authorize*** button as well.

![enter image description here](https://i.ibb.co/7YkRWyR/Authorization-1.png)
![enter image description here](https://i.ibb.co/LkMwSDD/Authorization-2.png)

Default username and password is as follows:

    Username: CreaInc
    Password: 12345

Once you have successfully authorized, you no longer will be asked for any authentication for subsequent requests. 

In the requested project, there were 2 endpoints needed; one for retrieving data and other for showing the available data.

 - Retrieve Endpoint: POST api/News
 - Show Endpoint: 		GET api/News

### Retrieving News
POST api/News endpoint doesn't require any additional parameters. When it is called, current news will be retrieved and saved to the database. According info message will be returned. 
![enter image description here](https://i.ibb.co/xs5Vsm0/Retrieve-Endpoint.png)

### Showing News
GET api/News endpoint can take optional parameters to return desired news from the database. It has 6 optional parameters:

 1. Skip: Used for pagination, determines how many news will be skipped
 2. Take: Used for pagination, determines how many news will be returned
 3. Query: A string literal that will be searched in the news titles
 4. StartDate: When entered, news that obtained later than this date will be returned
 5. EndDate: When entered, news that obtained earlier than this date will be returned
 6. Descending: Orders news according to obtained date descending or ascending
![enter image description here](https://i.ibb.co/xs5Vsm0/Retrieve-Endpoint.png)


