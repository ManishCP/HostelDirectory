using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;

namespace HostelDirectoryMvvM.ViewModels
{
    public class StudentDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate StudentEditorViewTemplate { get; set; }
        public DataTemplate EmptyViewTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            // Debugging to understand the selection process
            if (item != null && item is StudentViewModel studentViewModel && !string.IsNullOrEmpty(studentViewModel.StudentID))
            {
                Debug.WriteLine($"Selecting template for {((StudentViewModel)item).GetType().Name}");
                return StudentEditorViewTemplate;
            }
            else
            {
                Debug.WriteLine($"Selecting EmptyViewTemplate");
                return EmptyViewTemplate;
            }
        }
    }
}
