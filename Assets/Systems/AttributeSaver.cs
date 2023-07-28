using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class AttributeSaver : MonoBehaviour
{
    public static AttributeSaver instance;
    public CharacterAttributes savedAttributes;

    CharacterAttributes scores = new CharacterAttributes();
    void Awake()
    {
        instance = this;
        if (!Directory.Exists(Application.persistentDataPath + "/CharacterAttributes/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/CharacterAttributes/");
        }
    }
    public void SaveScores(CharacterAttributes attributesToSave)
    {
        savedAttributes = attributesToSave;
        XmlSerializer serializer = new XmlSerializer(typeof(Leaderboard));
        FileStream stream = new FileStream(Application.persistentDataPath + "/CharacterAttributes/characterattributes.xml", FileMode.Create);
        serializer.Serialize(stream, savedAttributes);
        stream.Close();
    }
    public CharacterAttributes LoadScores()
    {
        if (File.Exists(Application.persistentDataPath + "/CharacterAttributes/characterattributes.xml"))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Leaderboard));
            FileStream stream = new FileStream(Application.persistentDataPath + "/CharacterAttributes/characterattributes.xml", FileMode.Open);
            savedAttributes = serializer.Deserialize(stream) as CharacterAttributes;
        }
        return savedAttributes;
    }

    public void AddNewScore(float attackSpeed, float movementSpeed, int armor, int attackDamage, int maxHealth, int pointsOfPower)
    {
        scores = new CharacterAttributes
        {
            AttackSpeed = attackSpeed,
            MovementSpeed = movementSpeed,
            Armor = armor,
            AttackDamage = attackDamage,
            MaxHealth = maxHealth,
            PointsOfPower = pointsOfPower
        };
    }
}
[System.Serializable]
public class Leaderboard
{
    public CharacterAttributes attributes = new CharacterAttributes();
}