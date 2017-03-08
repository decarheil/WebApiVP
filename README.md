# README #

Web API using AWS signature V2 requests authentication.

### SERVICES ### 
##### AUTHENTICATE #####

Authenticate user by email and password

POST /api/authenticate

Body :

| KEY | VALUE EXAMPLE |
| ------ | ------ |
| Email | test@exemple.com |
| Password | mdp |

##### AUTHORIZE ##### 

Get request authorization (AWS signature V2 like).

GET /api/confidentials/{email}

Headers :

| KEY | VALUE EXAMPLE |
| ------ | ------ |
| Authorization | AWS AKIAIOSFODNN7EXAMPLE:UbAQNRMJQtsQUAArKxxgeDsAyXc= |
| X-Amz-Date | Wed, 08 Mar 2017 10:30:29 GMT |

##### GENERATE API KEY ##### 

Generate AWS API key.

GET /api/generateapikey/{accessKey?}

Headers :

| KEY | VALUE EXAMPLE |
| ------ | ------ |
| X-Amz-Date | Wed, 08 Mar 2017 10:30:29 GMT |

### USERS ###

| ID | EMAIL | PASSWORD | ACCESSKEY |
| ------ | ------ | ------ | ------ |
| 1 | test@exemple.com | mdp | AKIAIOSFODNN7EXAMPLE |
| 2 | test2@exemple.com | password | BITNHQ402LCLBPWTTLWZ |