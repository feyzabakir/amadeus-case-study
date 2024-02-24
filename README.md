# Case Study: Flight Search Application QA Tests  (QA Engineer)
This repository encompasses QA tests for both the front-end and back-end of the Flight Search application. Test scenarios have been developed using xUnit, Moq, Selenium, NewtonsoftJson, and C#.

## Backend Tests

- HTTP Status Code Verification:
 The GET request should return the required 200 status code.

- Intervention Content Check:
 The response content from the API needs to be tested.
 
- Header Verification:
The response from GET requests should contain a "Content-Type" header with a value of "application/json".

## Frontend  Tests

### Search Tests:

- Verify that the same value cannot be entered in the "From" and "To" input fields.
- The test should fail as it will be observed that these fields can be entered with the same value in the application. A bug has been identified.
- Since there are no flights between some cities, responses will not be received for certain queries. Tip: When selecting "From: Istanbul" and "To: Los Angeles," you should see that two flights are listed.

 ### Listing Tests:

- Test that the number of listed flights matches the value in the "Found X items" message.
✅ This test should pass successfully.

## TEST RESULTS
![flightsearchtest](https://github.com/feyzabakir/amadeus-case-study/assets/120409251/8e265273-72f4-4061-aeb7-0cbe276dd150)

## VIDEO
<a href="https://github.com/feyzabakir/amadeus-case-study/issues/1">Video Link</a>
