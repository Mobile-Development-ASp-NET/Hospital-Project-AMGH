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
  
## Features
  - Volunteer Application Form - Estevan Cordero
    - This feature allows a form to be filled out to apply for a position in a department. The application is saved in a database where an admin can review a list of applicants, review a selected applicant, update the status of the applicant, or delete the application that was submitted. 
    - Models
      - Application
      - Position

  - Greeting Cards-Alby Baby
    - This feature allows a person to send a greeting card to a patient admitted to the hospital.The admin can do the crud operations on the greeting card and also on the                admissions of the hospital.
