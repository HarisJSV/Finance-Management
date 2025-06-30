# Finance-Management
## Hexaware's Coding Challenge (C#)

So, according to the instructions given by hexaware, I am supposed to create a system that allows user to create, delete and update expenses along with generate a expense report with a time range. But as I implemented it, I felt that the users should be able to manipulate their own data rather than everyone else's. So I made an additional role such as Admin that gives them complete control to delete or update any user's info. The users themselves will only be able to delete their own account, create their expenses, etc.

The admin role is completely optional, I made it just so for proper use.


Note: You will not find the admin table queries on the schema because it's purely optional. It will only contain expense,expensecategories and users just as the instructions intended.

(Important)
And as for the Nunit 3 unit tests, I had used version 4.3.2 with a .Net framework of 4.7.2
Sooo, if you are using Nunit 4, you have to use Assert.That(condition, Is.True) rather than Assert.IsTrue(condition), etc.
This project uses Nunit 4, so the code uses the former of the two statements above.

### Entity Framework:
So, the project implements ADO .NET for the most part, however I had also implemented Entity Framework, specifically in the admin menu. It's not mandatory, but try it if you want. 
