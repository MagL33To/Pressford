# Pressford
A simple CMS

To run this app, please clone the repository down to your local machine and run the DBscript.sql to create the database. Once the database is created, add a user with read/write privaleges for the site to connect with. Modify the "PressfordModel" connection string in the web.config of the site to use your database and user credentials. Then build the solution (making sure you have nuget auto package download turned on) and run it in your browser.
Authorisation is stored in session. To log in as a different user, start a new session or delete the auth cookie.

Given more time I would:
Write more tests.
Implement password security.
Do more work on the look and feel of the site.
Implement the ability to sign out.

Site logins:
Admins:
u: w.pressford@pressford.com
p: pressford4life
u: a.skywalker@pressford.com
p: darksidelols

Non-admins:
u: l.skywalker@pressford.com
p: xwingrules
u: o.kenobi@pressford.com
p: forcemaster1
