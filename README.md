# Hostel Directory Management System

## Description

The Hostel Directory Management System is a desktop application developed using WPF (Windows Presentation Foundation) and MVVM (Model-View-ViewModel) architecture. It allows users to manage student records in a hostel setting, providing features such as adding new students, updating existing records, searching for students by name, and deleting records.

The application provides a user-friendly interface where users can easily input and manage student information. It utilizes Entity Framework for database operations, ensuring efficient data management and retrieval.

One key feature of the application is its ability to filter the list of students based on their names, making it easier for users to find specific records. Additionally, the application handles predefined students who cannot be deleted, ensuring data integrity.

Overall, the Hostel Directory Management System is designed to streamline the process of managing student records in a hostel, providing a reliable and efficient solution for hostel administrators and staff.

## Features

- **Naming and Coding Conventions:** Followed C# naming conventions and code conventions as per Microsoft guidelines. Placed fields, constructors, properties, and methods in a structured manner and used regions for better organization.

- **No Code Behind in XAML:** Avoided using code-behind files in *.xaml.cs files to separate UI logic from business logic, improving maintainability.

- **Implement Styles:** Implemented styles(implicitly and explicitly) to remove repeating markup elements and ensure a consistent look and feel across the application.

- **Split Main View:** Split the main view into two vertical parts: one for displaying a list of students and the other for editing the selected student. Implemented resizable views with a minimal size for each part.

- **Student List View:** Added a ListBox for displaying the list of students, with a template showing the student's name, room number, age, and a remove button. Used a ViewModel for binding and implemented a command for removing students, ensuring that predefined students cannot be deleted.

- **Messenger (Publish/Subscribe):** Used Messenger for exchanging messages between ViewModels, particularly for deleting students. Implemented as a singleton with methods for registering/receiving messages and sending messages.

- **Add Student Command:** Implemented a command for adding new students, with a button in the view for user interaction.

- **Base ViewModel:** Implemented a base ViewModel for inheriting common functionality in concrete ViewModels.

- **Object-Oriented Principles:** Applied the four pillars of OOPs (abstraction, encapsulation, inheritance, and polymorphism) where appropriate to ensure a well-structured and maintainable codebase.

- **Filtering:** Added a TextBox for filtering the student list by name, with instant filter applying as the user types.

- **Student Edit View:** Added a view for editing student properties, with TextBoxes for Name, Room, and Age. Implemented warning messages for deselected students.

- **Unique Student IDs:** Implemented unique student IDs for each application execution session, with read-only access from the outside.

- **Dynamic Control Sizing:** Used automatic content size adjustment to ensure the application looks visually appealing at any window size.

- **Optional Validation:** Optionally added validation to the student edit view to prevent blank fields.

The Hostel Directory Management System provides a user-friendly interface for managing hostel student records, following best practices and conventions for a robust and maintainable application.

## Installation

To run the application, follow these steps:

1. Clone the repository to your local machine.
2. Open the solution in Visual Studio.
3. Build the solution to restore the NuGet packages.
4. Run the application.

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

## Contributing

Contributions are welcome! Please feel free to open a pull request or submit an issue if you have any suggestions or improvements.

## References

- [Microsoft C# Naming Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names)
- [Microsoft C# Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)


## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
