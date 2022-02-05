# Blog Engine


##### I create a blog engine with 3 sites:
      1. API site to provide data for persisting data using
               asp.net core api
               entity framework core
               configure with Swagger UI
               authentication and authorization with Identity Server 4
          end point: https://huyblog-api.azurewebsites.net


     2. Identity Server for support single sign-in/sign-out using
               asp.net core mvc
               window identity
               identity server 4
           end point: http://huyblog-identityserver.azurewebsites.net
               
     3. Blog Engine site with public and admin pages using
               asp.net core mvc and razor
               authentication and authorization with Identity Server 4
           URL: https://huyblog.azurewebsites.net



## 1. Public Blog website: 
 ```https://huyblog.azurewebsites.net/```

     User is able to go to home page to see all posts which are order by publish date and paging with page size is 2 posts.

     User is able to go to a post by clicking the link on top of each post
       e.g: https://huyblog.azurewebsites.net/Home/Details/13

     User is able to go to the older posts or newer posts by clicking the older posts or newer post buttons in the end of each main pages
       e.g: https://huyblog.azurewebsites.net/?pageNumber=2


## 2. Admin Page:
```https://huyblog.azurewebsites.net/Panel/Index```


   admin user is able to go to the admin page by using the above link

   after go to the above link, admin user will be redirected to the Login Page of Identity Server 4 then (s)he is able to login by entering the email and password, Identity Server site will redirect (s)he back to the Admin page
 
  the above flow is also applied when user clicks Sign In link on the top of Public Blog site

   email and password for login to Identity Server are below:

    an.huy90@gmail.com / 1qazXSW@

  but because I have no time to implement the authorization feature so any user is also able to register a new account and become an admin user :D

  after logging in, admin user is able to create a new post, edit or delete an existing post.

    