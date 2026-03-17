using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Bildverwaltungsprogramm.CustomCommands
{
    public static class CustomCommands
    {
        public static readonly RoutedUICommand Move = new RoutedUICommand
            (
                "Move",
                "Move",
                typeof(CustomCommands),
                new InputGestureCollection
                {
                    new KeyGesture(Key.M, ModifierKeys.Control)
                }
            );

        public static readonly RoutedUICommand Add = new RoutedUICommand
            (
                "Add",
                "Add",
                typeof(CustomCommands),
                new InputGestureCollection
                {
                    new KeyGesture(Key.A, ModifierKeys.Control)
                }
            );

        public static readonly RoutedUICommand RotatePlus90 = new RoutedUICommand
            (
                "RotatePlus90",
                "RotatePlus90",
                typeof(CustomCommands),
                new InputGestureCollection
                {
                    new KeyGesture(Key.NumPad1, ModifierKeys.Control)
                }
            );

        public static readonly RoutedUICommand RotateMinus90 = new RoutedUICommand
            (
                "RotateMinus90",
                "RotateMinus90",
                typeof(CustomCommands),
                new InputGestureCollection
                {
                    new KeyGesture(Key.NumPad2, ModifierKeys.Control)
                }
            );

        public static readonly RoutedUICommand Rotate180 = new RoutedUICommand
            (
                "Rotate180",
                "Rotate180",
                typeof(CustomCommands),
                new InputGestureCollection
                {
                    new KeyGesture(Key.NumPad3, ModifierKeys.Control)
                }
            );
    }
}
