using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mission", menuName = "Mission")]
public class MissionData : ScriptableObject
{
    public string missionName;
    public string missionDescription;
    public int difficulty; // 0 fácil, 1 medio, 2 difícil
    public Sprite missionIconSprite; // El ícono específico para esta misión
    public bool isCompleted = false;

    // Diccionario para textos graciosos de NPC
    public Dictionary<string, string[]> npcMessages;

    private void OnEnable()
    {
        // Inicializa los mensajes de NPC
        npcMessages = new Dictionary<string, string[]>
        {
            { "letter", new string[]
                {
                    "Hey, you lovely mail carrier! Could you send this letter to my grandma? \n" +
                    "Tell her I’m sending hugs along with it! She needs to know I’m still practicing \n" +
                    "my cookie-baking skills!",

                    "Excuse me, dear post friend! Can you make sure this birthday surprise reaches \n" +
                    "{NPC_NAME} on time? I’ve wrapped it in sparkles and giggles—perfect for a party!",

                    "Oh, wonderful mail carrier! This letter is a heart-shaped secret just for {NPC_NAME}! \n" +
                    "Please deliver it with a sprinkle of fairy dust and a wink from me!",

                    "\"Hey there, speedy mail carrier! Could you pop this note in the mailbox for my buddy, {NPC_NAME}? \n" +
                    "It’s filled with friendship and some silly doodles. Who wouldn’t want that?\"\n",

                    "\"Ahoy, dear postal pal! Can you take this festive letter and ensure it lands safely in {NPC_NAME}’s \n" +
                    "mailbox? It’s stuffed with holiday cheer and some extra jingle bells!\"",

                    "Hello, my fabulous mail carrier! I need you to deliver this thank-you note to {NPC_NAME}. \n" +
                    "It’s brimming with gratitude and a few cat stickers for extra cuteness!",

                    "\"Hey there, amazing mail person! Would you mind taking this invitation to my neighbor, {NPC_NAME}? \n" +
                    "I’m throwing a get-together, and it needs all the fun it can get!\"",

                    "Hey, awesome postal friend! Could you drop this reminder off for me? \n" +
                    "It’s a gentle nudge to {NPC_NAME} to come over for tea and some good old gossip!"
                }
            },

            { "cat", new string[]
                {
                    "Could you help me find my lost cat? \nHe's probably chasing butterflies again!",
                    "Have you seen my cat? \nHe's an expert at hide-and-seek, but this time he’s hiding a bit too well!",
                    "Excuse me, could you help me find my fluffy companion? \nI think he’s busy plotting world domination somewhere!",
                    "I swear my cat has a secret life! \nCan you find him? He was last seen on a top-secret mission to the neighbor’s yard!",
                    "I can’t find my cat! \nI think he’s auditioning for a superhero role—he’s been practicing his rooftop leaps!",
                    "I lost my cat! \nHe’s usually the one supervising my naps, so I’m not sure how I can snooze without him!",
                    "I can't find my mischievous cat! \nI think he’s out there trying to win a game of chess against the squirrels!"
                }
            },

            { "potion", new string[]
                {
                    "Can you whip up a love potion for me? \nI need it to win the heart of my crush—make it extra sparkly!",
                    "I’m in desperate need of a love potion! \nCan you brew one that smells like roses and tastes like happiness?",
                    "I need a potion to help me confess my feelings! \nCan you make it bubble with charm and sprinkle in a little courage?",
                    "I’m stressing about my exam! \nCould you make me a potion that guarantees I’ll remember everything I studied?",
                    "I need a luck potion for my upcoming exam! \nCan you brew it with extra concentration and a side of good vibes?",
                    "Can you prepare a healing potion for me? \nI had a bit of an accident while gathering herbs!",
                    "Could you mix me a healing potion? \nI’ve been feeling a bit under the weather!",
                    "Help me create a potion that turns me into a cat! \nI want to see the world from a feline perspective!",
                    "Can you brew a courage potion for me? \nI want to face my fears and finally ride that roller coaster!"
                }
            }
        };
    }
}
