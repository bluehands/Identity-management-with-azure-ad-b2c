# Identity management with Azure AD b2c

This project gives instructions how to build an Azure AD b2c identity management system. 

# Create the b2c

* Goto Azure portal and create a new resource.
  * Create a new  Azure AD B2C Tenant (Erdbeer und Vanille)
  * Link an existing Azure AD B2C Tenant to my Azure subscription
  * Switch to tenant

* Add a new Application "*Web Api*"
  * Add Redirect url "*https://jwt.ms*"
  * Add App Id "*api*"
  * Add a new entry "*vote*" under "*Published scopes*"
  * Create a new entry under "*API access*"
  
* Add a Sign-up Sign-in policy "*SuSi*"
  * Don't forget to adjust the password policy
* Add a Profile editing policy "*Pe*"
* Add a Password Reset policy "*Pr*"

* Test the application with "*Susi*"->"*Run Now*"

* Add a new Application "*Web frontend*"
  * Add Redirect url "*http://localhost:6420/index.html*"  
  * Create a new entry under "*API access*"

* Add identity providers
  * Add Google
  * Goto https://console.developers.google.com
  * Add a new Project
  * Select new Project and goto "*Zugangsdaten*"
  * Goto "*oAuthxxxxx*" and save
  * Goto "*Anmeldedaten*" and create "*oAuth client Id*"
  * Add "*https://login.microsoftonline.com*" to "*Autorisierte JavaScript-Quellen*"
  * Add "*https://login.microsoftonline.com/te/erdbeerundvanille.onmicrosoft.com/oauth2/authresp*" to "*Autorisierte Weiterleitungs-URIs*"
  * Remember the id's

* Goto Policies and add the new provider

* Add a new Application "*Mobile*"
  * Add Redirect url "*msalXXXXXXXX://auth*"  Replace XXXXX with your Application Id
  * Create a new entry under "*API access*"
