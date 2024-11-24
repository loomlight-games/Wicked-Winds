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
    public int timeBonus;

    // Diccionario para textos graciosos de NPC
    public Dictionary<string, string[]> npcMessages;

    // Diccionario para respuestas de NPC
    public Dictionary<string, string[]> npcAnswers;

    private void OnEnable()
    {
        // Inicializa los mensajes de NPC
        npcMessages = new Dictionary<string, string[]>
        {
            { "LetterMision", new string[]
                {
                    "Hey, you lovely mail carrier! Could you send this letter to my grandma? \n" +
                    "Tell her I’m sending hugs along with it! She needs to know I’m still practicing \n" +
                    "my cookie-baking skills!",

                    "Excuse me, dear post friend! Can you make sure this birthday surprise reaches \n" +
                    "{NPC_NAME} on time? I’ve wrapped it in sparkles and giggles—perfect for a party!",

                    "Oh, wonderful mail carrier! This letter is a heart-shaped secret just for {NPC_NAME}! \n" +
                    "Please deliver it with a sprinkle of fairy dust and a wink from me!",

                    "Hey there, speedy mail carrier! Could you pop this note in the mailbox for my buddy, {NPC_NAME}? \n" +
                    "It’s filled with friendship and some silly doodles. Who wouldn’t want that?",

                    "Ahoy, dear postal pal! Can you take this festive letter and ensure it lands safely in {NPC_NAME}’s \n" +
                    "mailbox? It’s stuffed with holiday cheer and some extra jingle bells!",

                    "Hello, my fabulous mail carrier! I need you to deliver this thank-you note to {NPC_NAME}. \n" +
                    "It’s brimming with gratitude and a few cat stickers for extra cuteness!",

                    "Hey there, amazing mail person! Would you mind taking this invitation to my neighbor, {NPC_NAME}? \n" +
                    "I’m throwing a get-together, and it needs all the fun it can get!",

                    "Hey, awesome postal friend! Could you drop this reminder off for me? \n" +
                    "It’s a gentle nudge to {NPC_NAME} to come over for tea and some good old gossip!"
                }
            },

            { "CatMission", new string[]
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

            { "PotionMission", new string[]
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
            },


            { "OwlMission", new string[]
                {

                   "Oh no, my wise owl has flown off again! \nI’m sure he’s giving a lecture to the young birds, teaching them the secrets of the skies! Could you help me find him?",

                    "I believe my owl is having a deep conversation with the moon. \nHe’s always been the mysterious type, but I could use your help in bringing him back before his wisdom spreads too far!",

                    "My owl is out there, sharing his wisdom with the stars! \nCan you find him before he becomes a sage for every bird in the forest?",

                    "Where has my owl gone now? \nI bet he’s out there in the forest, talking to the trees or teaching the birds how to fly with purpose. Please, help me find him!",

                    "My owl is the wisest creature I know, but he’s off giving one of his philosophical talks to the wind! \nCan you track him down before he starts teaching the squirrels about the meaning of life?",

                    "I think my owl has joined the celestial council again—he’s probably up there in the sky, debating the mysteries of the universe with the stars. Help me bring him back!",

                    "Help! My owl has wandered off again. \nHe’s probably sharing his profound wisdom with the moon, telling tales that no one else can understand. I need him back home before he starts writing a book!",

                    "Oh dear, my owl is out there again, passing his wisdom to the night! \nI can’t find him, but I’m sure he’s giving an important lecture to the night birds. Can you help me bring him back?"
                }
            }
        };

        // Inicializa las respuestas de NPC
        npcAnswers = new Dictionary<string, string[]>
        {
            { "LetterMision", new string[]
                {
                    "Oh, what a delightful surprise! \nThank you for delivering this letter from my dear grandchild. \nIt means the world to me!",
                    "Oh, how wonderful!\n This birthday message has made my day\n—thank you so much for bringing it to me!",
                    "Oh, my heart!\n This letter is so sweet and full of love!\n Thank you for delivering it!",
                    "Oh, look at this!\n My friend always knows how to cheer me up.\n Thanks for bringing this to me!",
                    "Oh, what joy!\n This festive letter has filled me with holiday cheer.\n Thank you for delivering it!",
                    "Oh, a thank-you note with cat stickers!\n This made my day. \nThank you for bringing it to me!",
                    "Oh, an invitation! \nHow thoughtful of my neighbor\n—thank you for making sure I got this!",
                    "Oh, a friendly reminder—how kind!\n Thank you for delivering this message to me!"
                }
            },

            { "CatMission", new string[]
                {
                    "Thank you! \nYou’re my only hope in finding my furry friend!",
                    "You’re amazing! \nI knew you’d help me track him down!",
                    "You’ve got a great eye! \nI’m sure we’ll find him in no time!",
                    "I appreciate your help!\n I'm sure he had enough adventures for one day!",
                    "You’re a true friend! \nI can always count on you for support!",
                    "Thanks!\n Without you, I’d be lost without my little buddy!",
                    "You’re the best!\n I can’t wait to snuggle with my cat again!"
                }
            },

            { "PotionMission", new string[]
                {
                    "You’re incredible! \nThis love potion will work wonders!",
                    "Thank you! \nThis potion will make my crush fall head over heels!",
                    "You’re a genius! \nI can feel the charm already bubbling up!",
                    "I appreciate your help! \nThis will make my studying so much easier!",
                    "You’re awesome!\n With this luck potion,\n I’ll ace my exam!",
                    "Thanks! \nThis healing potion will get me back to my herb-gathering adventures!",
                    "You’re fantastic! \nI’m feeling better already with this potion!",
                    "You’re the best! \nI can’t wait to see the world through my cat’s eyes!",
                    "You’re amazing! \nWith this courage potion, \nI’m ready for anything!"
                }
            },


            { "OwlMission", new string[]
                {
                    "Ah, my wise owl is home! \nThank you for bringing him back from his celestial journey!",

                    "You found him! \nHe’s probably already telling the birds about the secrets of the moon. Thank you!",

                    "Ah, you’ve returned my philosopher! \nI’m sure he’s got a new story to tell the stars tonight. Thank you!",

                    "My owl has returned! \nI bet he’s got a thousand new pieces of wisdom to share. Thank you so much!",

                    "Thank you for bringing my wise companion back! \nI’m sure he’ll now have a story to tell every bird in the forest!",

                    "You’ve done it! \nI’ll bet my owl’s already deep in conversation with the wind again. Thanks for bringing him back!",

                    "Oh, thank you! \nMy owl is back, and no doubt, he’s ready to share his infinite wisdom with anyone who listens!",

                    "You’ve brought him back! \nNow I can rest easy knowing he’s not off teaching the stars... for now!"
                }
            }
        };
    }
}
