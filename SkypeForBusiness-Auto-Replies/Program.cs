using System;
using Microsoft.Lync.Model;
using Microsoft.Lync.Model.Conversation;
using Microsoft.Lync.Model.Group;

namespace SkypeForBusiness_Auto_Replies
{
    class Program
    {
        private static Contact self;
        private static Group noAutoGroup;


        static void Main(string[] args)
        {
        
            LyncClient client = LyncClient.GetClient();
            ConversationManager conversationManager = client.ConversationManager;
            conversationManager.ConversationAdded += onConversationAdded; // Add our handler
            self = client.Self.Contact;

            ContactManager contactManager = client.ContactManager;
            GroupCollection groups = contactManager.Groups;

            foreach(Group group in groups)
            {
                if (group.Name.Equals("NOAUTORESPOND"))
                {
                    noAutoGroup = group;
                }
            }

            // Keep the application alive
            Console.ReadLine();
        }

        private static void onConversationAdded(object sender, ConversationManagerEventArgs args)
        {
            Conversation newConversation = args.Conversation;
            Contact inviter = (Contact) newConversation.Properties[ConversationProperty.Inviter];

            if (inviter.Uri.Equals(self) && newConversation.Participants.Count == 2 && newConversation.Modalities.ContainsKey(ModalityTypes.InstantMessage))
            {
                GroupCollection groups = inviter.CustomGroups;
                InstantMessageModality instantMessageModality = (InstantMessageModality) newConversation.Modalities[ModalityTypes.InstantMessage];
                if (groups == null || groups.Count == 0) // Default response for unregistered contacts.
                {
                    instantMessageModality.BeginSendMessage("Hi", null, null);
                } else if (groups.Contains(noAutoGroup)) // Begin looping through our config
                {
                    instantMessageModality.BeginSendMessage("Hi!", null, null);
                }
            }
        }
    }
}