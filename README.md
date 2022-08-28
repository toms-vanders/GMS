# Project Title

Guild Wars 2 - Guild Management System

## Project Description

Guild Wars 2 guild management app aims to help guild organization regarding social events, raiding, loot distribution and other activities. The solution implements n-tier architecture consisting of a web client, a desktop client, a business layer, a data access layer, the API itself, and database. 

* Database
   * Database was implemented using MSSQL with database-first approach. To deal with concurrency problems we used the optimistic approach by implementing rowversion column in our MSSQL tables.

* Data Access layer
    * This layer provides access to our stored data. We used Dapper as our ORM framework. We use database transactions in places where several database operations need to be processed in an atomic manner.
    
* Web API
   * Our solution’s clients are not concerned with the processing of data passed by users, for this we have a dedicated WebAPI. We provide the WebAPI which allows for various functionalities our app offers but we also catch the external Guild Wars 2 API calls by wrapping the API routes in our WebAPI routes.
    
 * Authentication/Authorization
   * We used Bcrypt.net library to deal with password hashing and salting mechanisms and secure user authentication. For authorization we used JWT. So, our API generates a token and returns it as a JSON token when a user logs in with valid credentials. The client stores this token client-side and sends it in the authorization header every time a user asks for a resource from our API.
    
* Desktop Client
    * For the desktop client implementation, we used WPF and MahApps.Metro framework to give our application a refreshing metro look with minimal effort. A large part of our information pointed towards using the Model-View-ViewModel (MVVM) pattern for our WPF project, but it was not implemented due to time constraints.
    
    <p align="center">
    <img src="https://user-images.githubusercontent.com/40015488/187066230-b725339c-d555-4218-abe2-8caa174dd573.png" width="600">
    </p>
    
    <p align="center">
    <img src="https://user-images.githubusercontent.com/40015488/187066266-6954186c-1dfa-48af-b994-2cc2590a3c4f.png" width="600">
    </p>
    
    <p align="center">
    <img src="https://user-images.githubusercontent.com/40015488/187066273-e80856da-a29e-4ead-b783-0f0b3dc3e72e.png" width="600">
    </p>

    * Web Client
    For the Web client implementation, we used ASP.NET MVC which includes Bootstrap styling framework. To further improve the interactive interface of the web client, the team included the methods from jQuery and AJAX calls to deal with asynchronous operations.
    
    <p align="center">
    <img src="https://user-images.githubusercontent.com/40015488/187066012-1a39608e-3d9c-44ab-8d3e-0f20c5ce9dc3.png" width="600">
    </p>
    
    <p align="center">
    <img src="https://user-images.githubusercontent.com/40015488/187066939-a52938bc-fa2f-4ffb-b443-2649a59792df.png" width="600">
    </p>
    
    <p align="center">
    <img src="https://user-images.githubusercontent.com/40015488/187066995-09c472ca-2d3c-41c3-963b-40bc1b92b42b.png" width="600">
    </p>    

<!-- CONTRIBUTORS -->
## Acknowledgments

This group project was done as part of the 3rd Semeter project for the Computer Science AP Degree at UCN.
