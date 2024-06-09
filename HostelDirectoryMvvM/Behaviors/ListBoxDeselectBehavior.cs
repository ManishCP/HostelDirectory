using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using HostelDirectoryMvvM.ViewModels;
using Microsoft.Xaml.Behaviors;

namespace HostelDirectoryMvvM.Behaviors
{
    public class ListBoxDeselectBehavior : Behavior<ListBox>
    {
        protected override void OnAttached()
        {
            AssociatedObject.PreviewMouseLeftButtonDown += ListBox_PreviewMouseLeftButtonDown;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PreviewMouseLeftButtonDown -= ListBox_PreviewMouseLeftButtonDown;
        }

        private void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBox listBox)
            {
                var originalSource = e.OriginalSource as DependencyObject;
                if (originalSource != null && FindAncestor<Button>(originalSource) != null)
                {
                    // Ignore clicks on buttons
                    return;
                }

                var clickedItem = ItemsControl.ContainerFromElement(listBox, originalSource) as ListBoxItem;
                if (clickedItem != null && clickedItem.IsSelected)
                {
                    clickedItem.IsSelected = false;
                    (AssociatedObject.DataContext as StudentViewModel)?.ClearCurrentStudent();
                    e.Handled = true; // Prevent further processing to avoid reselection
                }
            }
        }

        private T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            while (current != null)
            {
                if (current is T ancestor)
                {
                    return ancestor;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            return null;
        }
    }
}
