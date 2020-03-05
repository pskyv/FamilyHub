using Syncfusion.XForms.Chat;
using Xamarin.Forms;

namespace FamilyAgenda.Views
{
    public partial class ChatPage : ContentPage
    {
        public ChatPage()
        {
            InitializeComponent();
        }

        private void sfChat_SendMessage(object sender, Syncfusion.XForms.Chat.SendMessageEventArgs e)
        {
            MessagingCenter.Send<ChatPage, TextMessage>(this, "NewMessage", e.Message);
            e.Handled = true;
        }
    }
}
