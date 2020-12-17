using System;
using Microsoft.Lync.Model;
using Microsoft.Lync.Model.Conversation;
using Microsoft.Lync.Model.Group;

using IniParser;
using IniParser.Model;

namespace SkypeForBusiness_Auto_Replies
{
    class Program
    {
        private static IniData config;

        static void Main(string[] args)
        {
            LyncClient client = LyncClient.GetClient();
            ConversationManager conversationManager = client.ConversationManager;
            conversationManager.ConversationAdded += OnConversationAdded; // Add our handler

            ContactManager contactManager = client.ContactManager;
            GroupCollection groups = contactManager.Groups;

            var parser = new FileIniDataParser();
            config = parser.ReadFile("AutoReply.ini");

            // Keep the application alive
            Console.ReadLine();
        }

        private static void OnConversationAdded(object sender, ConversationManagerEventArgs args)
        {
            Conversation newConversation = args.Conversation;
            Contact inviter = (Contact) newConversation.Properties[ConversationProperty.Inviter];
            Console.WriteLine("New Window opened with: " + inviter.Uri);

            if (newConversation.Participants.Count == 2 && newConversation.Modalities.ContainsKey(ModalityTypes.InstantMessage))
            {
                GroupCollection groups = inviter.CustomGroups;
                InstantMessageModality instantMessageModality = (InstantMessageModality) newConversation.Modalities[ModalityTypes.InstantMessage];
                String inviteMessage = (String) instantMessageModality.Properties[ModalityProperty.InstantMessageModalityInviteMessage];

                // If we open a chat window ourselves there won't be an initial message, whereas if someone messages us first, the new conversation
                // will come with the property below populated, so we can use this to determine who initiated the conversation.
                if (inviteMessage.Length > 0)
                {
                    Console.WriteLine("Conversation was initiated by other user.");
                    // Begin Processing Group Messages
                    if (groups == null) // Default response for unregistered contacts.
                    {
                        Console.WriteLine("User does not belong to any Custom Group");
                        instantMessageModality.BeginSendMessage(config["Default"]["Message"], null, null);
                    }
                    else // Begin looping through our config
                    {
                        foreach (SectionData section in config.Sections)
                        {
                            bool found = groups.TryGetGroup(section.SectionName, out Group group);
                            if (found)
                            {
                                Console.WriteLine("User was first found in Group " + section.SectionName);
                                instantMessageModality.BeginSendMessage(config[section.SectionName]["Message"], null, null);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}