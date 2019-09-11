# WebApiTest_PD
A simple test of data access, aggregation, and retrieval.

## Solution Description
* Api

   This will likely have 3 endpoints 
   * POST ~/api/Reload   

      Will call HTTP GET https://sampledata.petdesk.com/api/appointments and store the results in a sqlite database.   
	  This will completely overwrite the database on disk.
   * ~/api/Appointments/ByType   
   * ~/api/Appointments/ByMonth   
   * ~/api/Appointments/* will require a bearer token, or else unauthorized will be returned

* TokenGenerator

   Will simply generate Json Web Token that can be used to access the api via some client

## Api/Reload

   Value types are declared as nullable (?) so that if the api fails to return a value we can detect it rather than the value reverting to default.
