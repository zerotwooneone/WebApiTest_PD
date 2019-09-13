# WebApiTest_PD
A simple test of data access, aggregation, and retrieval. This project was written in Visual Studio 2019 and is meant to run on .Net Core 2.2

## Solution Description
* Api

   This will likely have 3 endpoints 
   * POST ~/api/Reload   

      Will call HTTP GET https://sampledata.petdesk.com/api/appointments and store the results in a sqlite database.   
	  This will completely overwrite the database on disk.
   * GET ~/api/Appointments/ByType   

      Requires a bearer token   
	  Answers the question "What is the distribution in types of appointments we have?"
   * GET ~/api/Appointments/ByMonth   

      Requires a bearer token   
	  Answers the question "How many appointment requests do we get a month?"

* TokenGenerator

   Will simply generate Json Web Token that can be used to access the api via some client
   Here is a token that does not expire: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDMxOS8iLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo0NDMxOS8ifQ.5PcK928Rqvmty1_LpofHbWTkIOxk497-CrbEh4Wi9J8

* PetDeskProject1.postman_collection.json   

   A postman collection that was used to test the 3 api endpoints above. This is handy because it already has the auth token included.

## Branch Description

* v0.0.1 Just gets data from the endpoint and (re)creates a database
* v0.0.2 Filled out TokenGenerator and wired up JWT requirement in Api project
   * Changed reload controller to use hardcoded data for now
* v0.0.3 Added queries on the database for ByMonth and ByType
* v0.1.0 Cleaned things up for submission
   * Changed reload controller to real data again

## Api/Reload/*Model Classes

   Value types are declared as nullable (?) so that if the api fails to return a value we can detect it rather than the value reverting to default.
