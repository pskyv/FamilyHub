using FamilyAgenda.ViewModels;
using FamilyAgenda.Views;
using Syncfusion.XForms.Chat;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FamilyAgenda.Behaviors
{
    public class ChatPageBehavior : Behavior<ChatPage>
    {
        private ChatPageViewModel viewModel;

        private SfChat sfChat;
        public ChatPageBehavior()
        {

        }

        protected override void OnAttachedTo(ChatPage bindable)
        {
            this.sfChat = bindable.FindByName<SfChat>("sfChat");
            //this.viewModel = bindable.FindByName<ChatPageViewModel>("viewModel");
            this.viewModel = (ChatPageViewModel)bindable.BindingContext;
            this.viewModel.Messages.CollectionChanged += Messages_CollectionChanged;

            base.OnAttachedTo(bindable);
        }

        private async void Messages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var chatItem in e.NewItems)
                {
                    TextMessage textMessage = chatItem as TextMessage;
                    if (textMessage != null && textMessage.Author == this.viewModel.CurrentUser)
                    {
                        textMessage.ShowAvatar = true;
                        this.sfChat.ScrollToMessage(chatItem);
                    }
                    else
                    {
                        await Task.Delay(50);
                        this.sfChat.ScrollToMessage(chatItem);
                    }
                }
            }

        }

        protected override void OnDetachingFrom(ChatPage bindable)
        {
            this.viewModel.Messages.CollectionChanged -= Messages_CollectionChanged;
            this.sfChat = null;
            this.viewModel = null;
            base.OnDetachingFrom(bindable);
        }
    }
}
