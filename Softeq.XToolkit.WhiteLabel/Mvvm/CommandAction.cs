// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;

namespace Softeq.XToolkit.WhiteLabel.Mvvm
{
    /// <summary>
    ///     Simple <see cref="ICommand"/> wrapper for comfort using with any kind of buttons.
    ///     It contains not only the command itself, but also button title and style.
    /// </summary>
    public class CommandAction
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CommandAction"/> class.
        /// </summary>
        /// <param name="command">The command to execute when the button is tapped.</param>
        /// <param name="title">Button title.</param>
        /// <param name="commandActionStyle">Button style.</param>
        public CommandAction(ICommand command, string title, CommandActionStyle commandActionStyle = default)
        {
            Command = command;
            Title = title;
            CommandActionStyle = commandActionStyle;
        }

        /// <summary>
        ///     Gets or sets the command to execute when the button is tapped.
        /// </summary>
        public ICommand Command { get; set; }

        /// <summary>
        ///     Gets or sets the button title.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        ///     Gets or sets the button style.
        /// </summary>
        public CommandActionStyle CommandActionStyle { get; set; }
    }
}