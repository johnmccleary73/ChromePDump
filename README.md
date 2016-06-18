# ChrPwdDmp - Chrome Password Dumper
A little application to view the saved passwords from Google Chrome.

## A Little Background
Google Chrome saves passwords in a SQLite database file in 
`%userprofile%\AppData\Local\Google\Chrome\User Data\Default\Login Data`

This field is encrypted using [Windows Data Protection API] (http://msdn.microsoft.com/en-us/library/ms995355.aspx). It is a symetric encryption scheme making it impossible (theoretically) to retrieve the password as another user on the same machine or even the same user on another machine.

## WARNING
Once this project is compiled it has the potential to be flagged by Anti-Virus as malicious.
