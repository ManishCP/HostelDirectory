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
            if (item is StudentViewModel)
            {
                return StudentEditorViewTemplate;
            }
            // Show EmptyViewTemplate when no student is selected
            return EmptyViewTemplate;
        }

    }
}
