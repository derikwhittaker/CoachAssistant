using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Dimesoft.CoachAssistant.Behaviors
{
    public class UpdateOnChangeBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            AssociatedObject.TextChanged += OnTextChanged;
        }

        void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            AssociatedObject.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.TextChanged -= OnTextChanged;
        }
    }
}
