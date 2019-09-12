# WebApiTest_PD
A simple test of data access, aggregation, and retrieval.

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

## Api/Reload

   Value types are declared as nullable (?) so that if the api fails to return a value we can detect it rather than the value reverting to default.
