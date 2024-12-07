using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mission", menuName = "Mission")]
public class MissionData : ScriptableObject
{
    public string type; // TODO make this an enum
    public string missionDescription;
    public int difficulty; // 0 easy, 1 medium, 2 hard
    public Sprite missionIconSprite;
    public bool isCompleted = false;
    public int timeBonus;

    public Dictionary<string, string[]> npcMessages;

    public Dictionary<string, string[]> npcAnswers;

    private void OnEnable()
    {
        npcMessages = new Dictionary<string, string[]>
        {
            { "LetterMision", new string[]
                {
                    "Hey, you lovely mail carrier! Could you send this letter to my grandma? \n" +
                    "Tell her I'm sending hugs along with it! She needs to know I'm still practicing \n" +
                    "my cookie-baking skills!",

                    "Excuse me, dear post friend! Can you make sure this birthday surprise reaches \n" +
                    "my friend on time? I've wrapped it in sparkles and giggles'perfect for a party!",

                    "Oh, wonderful mail carrier! This letter is a heart-shaped secret just for my lover! \n" +
                    "Please deliver it with a sprinkle of fairy dust and a wink from me!",

                    "Hey there, speedy mail carrier! Could you pop this note in the mailbox for my buddy \n" +
                    "It's filled with friendship and some silly doodles. Who wouldn't want that?",

                    "Ahoy, dear postal pal! Can you take this festive letter and ensure it lands safely in my cousin's \n" +
                    "mailbox? It's stuffed with holiday cheer and some extra jingle bells!",

                    "Hello, my fabulous mail carrier! I need you to deliver this thank-you note to my friend. \n" +
                    "It's brimming with gratitude and a few cat stickers for extra cuteness!",

                    "Hey there, amazing mail person! Would you mind taking this invitation to my neighbor \n" +
                    "I'm throwing a get-together, and it needs all the fun it can get!",

                    "Hey, awesome postal friend! Could you drop this reminder off for me? \n" +
                    "It's a gentle nudge to my friend to come over for tea and some good old gossip!"
                }
            },

            { "CatMission", new string[]
                {
                    "Could you help me find my lost cat? \n" +
                    " He's probably chasing butterflies again!",

                    "Have you seen my cat? \n" +
                    "He's an expert at hide-and-seek, but this time he's hiding a bit too well!",

                    "Excuse me, could you help me find my fluffy companion? \n" +
                    "I think he's busy plotting world domination somewhere!",

                    "I swear my cat has a secret life! \n" +
                    "Can you find him? He was last seen on a top-secret mission to the neighbor's yard!",

                    "I can't find my cat! \n" +
                    "I think he's auditioning for a superhero role, he's been practicing his rooftop leaps!",

                    "I lost my cat! \n" +
                    "He's usually the one supervising my naps, so I'm not sure how I can snooze without him!",

                    "I can't find my mischievous cat! \n" +
                    "I think he's out there trying to win a game of chess against the squirrels!"
                }
            },

            { "PotionMission", new string[]
                {
                    "Can you whip up a love potion for me? \n" +
                    "I need it to win the heart of my crush! Make it extra sparkly!",

                    "I'm in desperate need of a love potion! \n" +
                    "Can you brew one that smells like roses and tastes like happiness?",

                    "I need a potion to help me confess my feelings! \n" +
                    "Can you make it bubble with charm and sprinkle in a little courage?",

                    "I'm stressing about my exam! \n" +
                    "Could you make me a potion that guarantees I'll remember everything I studied?",

                    "I need a luck potion for my upcoming exam! \n" +
                    "Can you brew it with extra concentration and a side of good vibes?",

                    "Can you prepare a healing potion for me? \n" +
                    "I had a bit of an accident while gathering herbs!",

                    "Could you mix me a healing potion? \n" +
                    "I've been feeling a bit under the weather!",

                    "Help me create a potion that turns me into a cat! \n" +
                    "I want to see the world from a feline perspective!",

                    "Can you brew a courage potion for me? \n" +
                    "I want to face my fears and finally ride that roller coaster!"
                }
            },


            { "OwlMission", new string[]
                {

                    "Oh no, my wise owl has flown off again! \n" +
                    "I'm sure he's giving a lecture to the young birds, \n" +
                    "teaching them the secrets of the skies! Could you help me find him?",

                    "I believe my owl is having a deep conversation with the moon. \n" +
                    "He's always been the mysterious type, but I could use your help. \n" +
                    "Bring him back before his wisdom spreads too far!",

                    "My owl is out there, sharing his wisdom with the stars! \n" +
                    "Can you find him before he becomes a sage for every bird in the forest?",

                    "Where has my owl gone now? \n" +
                    "I bet he's out there in the forest teaching the birds how to fly with purpose. \n" +
                    "Please, help me find him!",

                    "My owl is the wisest creature I know, but he's off giving one of his philosophical talks to the wind! \n" +
                    "Can you track him down before he starts teaching the squirrels about the meaning of life?",

                    "I think my owl has joined the celestial council again! He's probably up there in the sky, \n" +
                    "debating the mysteries of the universe with the stars. Help me bring him back!",

                    "Help! My owl has wandered off again. \n" +
                    "He's probably sharing his profound wisdom with the moon, \n" +
                    "telling tales that no one else can understand. I need him back home before he starts writing a book!",

                    "Oh dear, my owl is out there again, passing his wisdom to the night! \n" +
                    "I can't find him, but I'm sure he's giving an important lecture to the night birds. Can you help me bring him back?"
                }
            }
        };

        npcAnswers = new Dictionary<string, string[]>
        {
            { "LetterMision", new string[]
                {
                    "Oh, what a delightful surprise! \n" +
                    "Thank you for delivering this letter from my dear grandchild. \n" +
                    "It means the world to me!",

                    "Oh, how wonderful!\n" +
                    "This birthday message has made my day\n" +
                    "Thank you so much for bringing it to me!",

                    "Oh, my heart!\n" +
                    " This letter is so sweet and full of love!\n" +
                    " Thank you for delivering it!",

                    "Oh, look at this!\n" +
                    " My friend always knows how to cheer me up.\n" +
                    " Thanks for bringing this to me!",

                    "Oh, what joy!\n" +
                    " This festive letter has filled me with holiday cheer.\n" +
                    " Thank you for delivering it!",

                    "Oh, a thank-you note with cat stickers!\n" +
                    " This made my day. \n" +
                    "Thank you for bringing it to me!",

                    "Oh, an invitation! \n" +
                    "How thoughtful of my neighbor\n" +
                    "'thank you for making sure I got this!",

                    "Oh, a friendly reminder'how kind!\n" +
                    " Thank you for delivering this message to me!"
                }
            },

            { "CatMission", new string[]
                {
                    "Thank you! \n" +
                    "You're my only hope in finding my furry friend!",

                    "You're amazing! \n" +
                    "I knew you'd help me track him down!",

                    "You've got a great eye! \n" +
                    "I'm sure we'll find him in no time!",

                    "I appreciate your help!\n" +
                    " I'm sure he had enough adventures for one day!",

                    "You're a true friend! \n" +
                    "I can always count on you for support!",

                    "Thanks!\n" +
                    " Without you, I'd be lost without my little buddy!",

                    "You're the best!\n" +
                    " I can't wait to snuggle with my cat again!"
                }
            },

            { "PotionMission", new string[]
                {
                    "You're incredible! \n" +
                    "This love potion will work wonders!",

                    "Thank you! \n" +
                    "This potion will make my crush fall head over heels!",

                    "You're a genius! \n" +
                    "I can feel the charm already bubbling up!",

                    "I appreciate your help! \n" +
                    "This will make my studying so much easier!",

                    "You're awesome!\n" +
                    " With this luck potion,\n" +
                    " I'll ace my exam!",

                    "Thanks! \n" +
                    "This healing potion will get me back to my herb-gathering adventures!",

                    "You're fantastic! \n" +
                    "I'm feeling better already with this potion!",

                    "You're the best! \n" +
                    "I can't wait to see the world through my cat's eyes!",

                    "You're amazing! \n" +
                    "With this courage potion, \n" +
                    "I'm ready for anything!"
                }
            },

            { "OwlMission", new string[]
                {
                    "Ah, my wise owl is home! \n" +
                    "Thank you for bringing him back from his celestial journey!",

                    "You found him! \n" +
                    "He's probably already telling the birds about the secrets of the moon. Thank you!",

                    "Ah, you've returned my philosopher! \n" +
                    "I'm sure he's got a new story to tell the stars tonight. Thank you!",

                    "My owl has returned! \n" +
                    "I bet he's got a thousand new pieces of wisdom to share. Thank you so much!",

                    "Thank you for bringing my wise companion back! \n" +
                    "I'm sure he'll now have a story to tell every bird in the forest!",

                    "You've done it! \n" +
                    "I'll bet my owl's already deep in conversation with the wind again. Thanks for bringing him back!",

                    "Oh, thank you! \n" +
                    "My owl is back, and no doubt, he's ready to share his infinite wisdom with anyone who listens!",

                    "You've brought him back! \n" +
                    "Now I can rest easy knowing he's not off teaching the stars... for now!"
                }
            }
        };
    }
}
