# WebApiTest_PD
A simple test of data access, aggregation, and retrieval.

## Solution Description
* Api

   This will likely have 3 endpoints ~/api/Reload, ~/api/Appointments/ByType, and ~/api/Appointments/ByMonth  
   ~/api/Appointments/* will require a bearer token, or else unauthorized will be returned

* TokenGenerator

   Will simply generate Json Web Token that can be used to access the api via some client
