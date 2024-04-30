Thank you Education Development Trust for giving me this opportunity

How to run the API 
kindly download an Express SQL server from here -> https://www.microsoft.com/en-gb/sql-server/sql-server-downloads

Navigate to the link above
scroll down to where you can find DownLoad Now
Click on Express Download Now
Complete Installation 

Run API
http://localhost:5062/swagger/index.html

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


Seeder:
I have seeded some default Cover Types, Genres, Statuses, Publishers and Authors which are needed for creating a Book

Requests:
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

Create book Reservation Request Payload
{
  "bookId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "reservationDate": "2024-04-29T23:10:17.396Z"
}

Cancel reservation Payload 
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}

Update reservation status status
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "bookId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "reservationDate": "2024-04-29T23:12:40.544Z"
}


What Could have gone better 
Due to time factor as I was baby seating on Saturday, I had only saturday night and sunday to work on this project.  And I believe these are the things that could be gone better if I had time

1) Validating FK before creating a book
2) A user should be able to rent a book
3) A user should be able to process a book return
4) More unit test would have been added covering the infrastructure layer and Application layer
5) A comphrehensive validation of all requests
   

