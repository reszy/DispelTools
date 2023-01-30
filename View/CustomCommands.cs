using System.Windows.Input;

namespace View
{
    public static class CustomCommands
    {
        public static RoutedUICommand Exit { get; } = new
            (
                "Exit",
                "Exit",
                typeof(CustomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.F4, ModifierKeys.Alt)
                }
            );
        public static RoutedUICommand OpenView { get; } = new RoutedUICommand
        (
            "View",
            "View",
            typeof(CustomCommands)
        );
        
    }
}
