# Hospital-Project-AMGH

## Adding models to database

1. ADD AN App_Data in the folder found in the File Explore of the project*
2. Create Models you will be needing
3. Go to the identiy model page and add your table name there public DbSet<Model Name> *Table Name* { get; set; }
4. tools->nuget package console-> add-migration *name of table*
5. tools->nuget package console-> update-database
  
## Group Members
  - Estevan Cordero
  - Alby Baby
  - Tingwei Xie
  - Uditesh Jha
  - Majdi Nawfal
  - Manal Solanki
  
## Features
  - Volunteer Application Form - Estevan Cordero
    - This feature allows a form to be filled out to apply for a position in a department. The application is saved in a database where an admin can review a list of applicants, review a selected applicant, update the status of the applicant, or delete the application that was submitted. 
    - Models
      - Application
      - Position
    - Know Bugs
      - Update on Application Error
      - On Methods used for Controllers the following line shows: CS1591 Missing XML Comment ... (Not sure what that means)
  - Greeting Cards-Alby Baby
    - This feature allows a person to send a greeting card to a patient admitted to the hospital.The admin can do the crud operations on the greeting card and also on the                admissions of the hospital.The user can see a list of their greeting cards which they can update and delete.
  
     - Models
       - GreetingCard
       - Admission
     - To do List
       - Include the feature where admin can choose the patient to add the admissions from a drop down.
       - Include a model for the greeting card types with default pictures for all of them
       - Add more styles 
  
  - Survey Form - Tingwei xie
    - This feature allows a user to select a survey, then answer the questions of the selected survey. The responses will be saved in the database. The admin user read, delete, update the information of the selected survey or selected question. The admin user can also add new surveys and new questions. For MVP, surveys and questions models have been created. For the final product, the response model will be added to the project.
    - Models
      -Survey
      -Question
    - Work completed
      - Added CSS to the homepage
      - CRUD of my methods can only be accessed by an admin.
      - Form Validation
      - Added Tiny Editor to this project
    - To do List
      - Trying to make "save responses to the database" method works
      - Fix tiny editor bugs, @Html.raw() is not working at the moment.
  
  - Blogs - Uditesh Jha
    - This feature allows a user to read blogs in the blogs web page. Firstly, to get access to the blogs or updates regarding hospital, user has to subscribe for blogs. This         feature is an admin side functionality so an admin can view the list of subscribed users, create new user or delete them as well. An admin can view the list of blogs,           update the blog, create new blog and delete the particular blog. Here, admmin can approve the users for particular blog in the blog details page.
    - Models
      - Subscribed user
      - Blog
  
  - Patient Feedback - Majdi Nawfal
    - This feature allows a user to provide feedback upon visiting a doctor in the hospital. The feedback will be saved in the database, and all users will be able to see a list of the patient's feedback. Admin and logged in users will be able to add new feedback. Admin will be able to update and delete an existing feedback. 
    - Model
      - Feedback
  
  - Doctor Details - Manal Solanki 
    - This feature would list all the doctors according to their department and their individual details of a dr. Admin can create ,update and delete dr whereas Users can view the list of doctor and their details.

    - Models 
      - Department
      - DoctorDetails
    - Work Completed : MVP thats is CRUD for both the models is completed and in the doctor details view all the information related to department is being display.
    - To Do : List all the dr related to department on the list department View , Styling as well as authorization.
