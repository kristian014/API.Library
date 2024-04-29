Thank you Education Development Trust for giving me this opportunity

How to run the API 
kindly download an Express SQL server from here -> https://www.microsoft.com/en-gb/sql-server/sql-server-downloads

Navigate to the link above
scroll down to where you can find DownLoad Now
Click on Express Download Now
Complete Installation 

Run API
http://localhost:5062/swagger/index.html

Seeder:
I have seeded some default Cover Types, Genres, Statuses, Publishers and Authors which are needed for creating a Book

Create a Book Payload example -> 
{
  "title": "string",
  "isbn": "string",
  "description": "string",
  "price": 0,
  "publishedDate": "2024-04-29T22:42:36.147Z",
  "coverTypeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "genreId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "authorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "publisherId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}

SQL query to get neccesary Ids 
GenreId
SELECT TOP 1 l.id, l.Label FROM [Catalog].LookupTypes AS lt
LEFT JOIN [Catalog].Lookups As l
ON l.TypeId = lt.Id
WHERE lt.Name LIKE '%Genre%'

CoverTypeId
SELECT TOP 1 l.id, l.Label FROM [Catalog].LookupTypes AS lt
LEFT JOIN [Catalog].Lookups As l
ON l.TypeId = lt.Id
WHERE lt.Name LIKE '%Cover Type%'

AuthorId
SELECT TOP 1 Id FROM [Catalog].Authors

PublisherId 
SELECT TOP 1 Id FROM [Catalog].Publishers


Authentication: You will need to be Authorized to access the Reservation and Book endpoints. This can be done by getting a valid bearer token
Go to api/identity/login and use this payload to access a bearer token
{
  "email": "root@admin.com",
  "password": "Admin123Pa$$word!",
}
or 
{
  "email": "root@user.com",
  "password": "User123Pa$$word!",
}

Connecting to your local database
Server name: localhost\SQLEXPRESS
Click Login


