# simple-.NET-Crypting-For-PB
This Project will create .NET dll to implement crypto object en|decrypter for Powerbuilder

## Purpose
- Implement simple implementation dll for Powerbuilder Programming
- Also Supplement Assembly for SQL Server will used in SP for Powerbuilder Clients

## Cause of Not Implemented some cryptographic options In Powerbuilder
For Example
- AES Encryption with GCM mode
  - This is important to secure using Symmetric Encrypting Function in morden
- Diffie Hellman Key Exchange or else equivalent
  - for 2nd layer securing to transport sensitive data
- Bcrypt Password encoding or and so on to PW encryption
  - this is the matter of one-way password encryption well
  - just using 1 pass of hash function, eg. SHA-512 isn't secure enough

## Informations
- Target Frameworks
  - .NET Framework 4.8
    - This is ensure Cross compatibility between PB versions
  - .NET 8
    - This is for Powerbuilder 2025
- Implemented Methods
  - Not yet
  
## Milestone
- Implement methods
  - complete on draft.cs 's dreams
- Make a DLL with .NET 4.8
  - for PowerBuilder
  - for SQL Server
- Make a Github action CI-CD
- Make a DLL with .NET 8
- Update Github action
