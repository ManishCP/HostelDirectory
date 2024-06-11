# Hostel Directory Management System

## Table of Contents
- [Description](#description)
- [Task Achieved](#task-achieved)
- [Installation](#installation)
- [Usage](#usage)
- [Technologies Used](#technologies-used)
- [References](#references)
  
## Description

The Hostel Directory Management System is a desktop application developed using WPF (Windows Presentation Foundation) and MVVM (Model-View-ViewModel) architecture. It allows users to manage student records in a hostel setting, providing features such as adding new students, updating existing records, searching for students by name, deleting records, and filtering through the list of students based on their names, making it easier for users to find specific records. Additionally, the application handles predefined students who cannot be deleted, ensuring data integrity. It utilizes Entity Framework for database operations, ensuring efficient data management and retrieval.


## Task Achieved

- **Naming and Coding Conventions:** Followed C# naming conventions and code conventions as per Microsoft guidelines. Placed fields, constructors, properties, and methods in a structured manner and used regions for better organization.
  
- **Unique Student IDs:** Implemented standard procedure to ensure that each student ID is unique and that no null values are allowed in the application, with read-only access from the outside. The ID is generated programmatically and is guaranteed to be unique within a single execution session. Validation is implemented to ensure that all required fields are filled before a student can be added or updated in the database.

- **No Code Behind in XAML:** Avoided using code-behind files in *.xaml.cs files to separate UI logic from business logic, improving maintainability.

- **Implement Styles:** Implemented styles(implicitly and explicitly) to remove repeating markup elements and ensure a consistent look and feel across the application.

- **Split Main View:** Split the main view into two vertical parts: one for displaying a list of students and the other for editing the selected student. Implemented resizable views with a minimal size for each part.

- **Student List View:** Added a ListBox for displaying the list of students, with a template showing the student's name, room number, age, and a remove button. Used a ViewModel for binding and implemented a command for removing students, ensuring that predefined students cannot be deleted.

- **Messenger (Publish/Subscribe):** Used Messenger for exchanging messages between ViewModels, particularly for deleting students. The Messenger class from the MVVM Light Toolkit is used to facilitate communication between different view models. This class is implemented as a singleton to ensure a single instance is used throughout the application. The Messenger allows view models to send and receive messages without needing direct references to each other, adhering to the principles of loose coupling.

- **Add Student Command:** Implemented a command for adding new students, with a button in the view for user interaction.

- **Base ViewModel:** Implemented a base ViewModel for inheriting common functionality in concrete ViewModels.

- **Object-Oriented Principles:** Applied the four pillars of OOPs (abstraction, encapsulation, inheritance, and polymorphism) where appropriate to ensure a well-structured and maintainable codebase.

- **Filtering:** Added a TextBox for filtering the student list by name, with instant filter applying as the user types.

- **Student Edit View:** Added a view for editing student properties, with TextBoxes for Name, Room, and Age. Implemented warning messages for deselected students.

- **Dynamic Control Sizing:** Used automatic content size adjustment to ensure the application looks visually appealing at any window size.

- **Optional Validation:** Optionally added validation to the student edit view to prevent blank fields.
  
- **Entity Framework:** Entity Framework is used as the ORM (Object-Relational Mapping) tool for data access. It manages the database operations, allowing for CRUD (Create, Read, Update, Delete) operations on student records without needing to write raw SQL queries. The integration with Entity Framework ensures a smooth and efficient interaction with the SQL Server database.


## Installation

**To run the application, follow these steps:**

1. Clone the repository to your local machine.
2. Open the solution in Visual Studio.
3. Build the solution to restore the NuGet packages.
4. Run the application.
   
**Setting up Your DB:**
1. Download Db backup file - https://drive.google.com/file/d/192ckvOVqeW98D5v4Eu5semGClOYMwOcc/view?usp=sharing
2. Open SQL Server Management Studio (SSMS) and connect to the target SQL Server instance.
3. Right-click on the Databases node and select Restore Database.
4. In the Restore Database window, select Device and click Add to browse and select the backup file you downloaded to the target system.
5. Make sure the destination database name is correct (you can change it if necessary).
6. Click OK to start the restore process.

## Usage

- **Adding a Student**: Click on the "ADD" button after entering the student's details in the text boxes to save the student information.
- **Updating a Student**: Select a student from the list, update the details in the text boxes, and click "UPDATE" to save the changes, Student ID is read-only.
- **Searching for a Student**: Enter the student's name in the filter text box to search for a specific student.
- **Deleting a Student**: Select a student from the list and click the "Delete" button. Predefined students cannot be deleted.
- **Clearing Selection**: Click the "CLEAR" button to clear the current selection.
- **Resizable Views:** Use the splitter to resize the views between the student list and the student editor.
- **Scrolling:** Use the scroll functionality to navigate through the student list.
- **Window Management:** Maximize or minimize the main window for better visibility and usability.

## Technologies Used

- C# for the backend logic.
- WPF for the user interface.
- Entity Framework for database operations.

## References

- [Microsoft C# Naming Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names)
- [Microsoft C# Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- [Styles](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/controls/how-to-create-apply-style)
- [WPF .NET](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/overview/?view=netdesktop-8.0)
- [Entity Framework](https://learn.microsoft.com/en-us/aspnet/entity-framework)
